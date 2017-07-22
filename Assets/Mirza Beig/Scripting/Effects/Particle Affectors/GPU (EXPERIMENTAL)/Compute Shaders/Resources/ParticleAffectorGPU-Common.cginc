
// Per-system data.

struct ParticleSystemData
{
	float3 position;
	float externalForcesMultiplier;

	float4 pad0;
};

// Per-particle data.

struct ParticleData
{
	float3 position;
	float3 velocity;

	int particleSystemIndex;

	float pad0;
};

// ...

float lengthSqr(float3 v)
{
	return (v.x * v.x) + (v.y * v.y) + (v.z * v.z);
}

// Assigned from the CPU.

float radiusSqr;

float force;
float deltaTime;

Texture2D scaleForceByDistance;

float3 transformPosition;
float2 distanceScaleFromTo;

// ...

int LUTSteps;

// Particle system data buffer.

RWStructuredBuffer<ParticleSystemData> particleSystemData;

// Particle data buffer.

RWStructuredBuffer<ParticleData> particleData;

// Values in the function calculations.

float3 particlePosition;

float3 scaledDirectionToAffectorCenter;
float distanceToAffectorCenterSqr;

float3 forceVec;

// ...

uint id_x;

// ... 

void init()
{
	particlePosition = particleSystemData[particleData[id_x].particleSystemIndex].position + particleData[id_x].position;

	scaledDirectionToAffectorCenter = particlePosition - transformPosition;
	distanceToAffectorCenterSqr = lengthSqr(scaledDirectionToAffectorCenter);
}

// Pass force vector as parameter.

void process()
{
	//if (distanceToAffectorCenterSqr < radiusSqr)
	//{
		// CUSTOM AFFECTOR CODE [START].
	
		// Assign custom force to forceVec.
		// Done from the main function/kernel.

		// CUSTOM AFFECTOR CODE [END].

		// Distance to center mapped to [0, 1].

		float distanceToCenterNormalized = distanceToAffectorCenterSqr / radiusSqr;
		
		// Scale normalized time alive to [0, LUTResolution].

		float LUTCoordV = distanceToCenterNormalized * (LUTSteps - 1.0f);

		// Floor is time at last coordinate.
		// Ceil is time at next coordinate.

		float LUTCoordV_Floor = floor(LUTCoordV);
		float LUTCoordV_Ceil = LUTCoordV_Floor + 1.0f;

		// Get normalized time between LUT coordinates.

		float timeRange = LUTCoordV_Ceil - LUTCoordV_Floor;
		float LUTSamplerTime = (LUTCoordV - LUTCoordV_Floor) / timeRange;

		float2 UV1 = float2(LUTCoordV_Floor, 0);
		float2 UV2 = float2(LUTCoordV_Ceil, 0);

		// Get the previous and current targets to lerp between.

		float forceA = scaleForceByDistance[UV1].a;
		float forceB = scaleForceByDistance[UV2].a;
		
		forceVec *= lerp(forceA, forceB, LUTSamplerTime);
		forceVec *= particleSystemData[particleData[id_x].particleSystemIndex].externalForcesMultiplier;

		forceVec *= force;
		forceVec *= deltaTime;

		// Instead of using the if-statement, just multiply it.
		// There's a performance gain of about 1-2 FPS from this.

		// Even with all the extra math!
		// Would using "step()" be faster here?

		forceVec *= distanceToAffectorCenterSqr < radiusSqr;

		particleData[id_x].velocity += forceVec;
	//}
}