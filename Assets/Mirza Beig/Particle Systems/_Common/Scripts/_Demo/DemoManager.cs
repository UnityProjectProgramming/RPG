
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public class DemoManager : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                public enum ParticleMode
                {
                    looping,
                    oneshot,
                }

                public enum Level
                {
                    none,
                    basic,
                }

                // =================================	
                // Variables.
                // =================================

                public Transform cameraRotationTransform;
                public Transform cameraTranslationTransform;

                public Vector3 cameraLookAtPosition = new Vector3(0.0f, 3.0f, 0.0f);

                public FollowMouse mouse;

                Vector3 targetCameraPosition;
                Vector3 targetCameraRotation;

                Vector3 cameraPositionStart;
                Vector3 cameraRotationStart;

                Vector2 input;

                // Because Euler angles wrap around 360, I use
                // a separate value to store the full rotation.

                Vector3 cameraRotation;

                public float cameraMoveAmount = 2.0f;
                public float cameraRotateAmount = 2.0f;

                public float cameraMoveSpeed = 12.0f;
                public float cameraRotationSpeed = 12.0f;

                public Vector2 cameraAngleLimits = new Vector2(-8.0f, 60.0f);

                public GameObject[] levels;
                public Level currentLevel = Level.basic;

                public ParticleMode particleMode = ParticleMode.looping;

                public bool lighting = true;
                public bool advancedRendering = true;

                public Toggle frontFacingCameraModeToggle;
                public Toggle interactiveCameraModeToggle;

                public Toggle loopingParticleModeToggle;
                public Toggle oneshotParticleModeToggle;

                public Toggle lightingToggle;
                public Toggle advancedRenderingToggle;

                Toggle[] levelToggles;
                public ToggleGroup levelTogglesContainer;

                public LoopingParticleSystemsManager loopingParticleSystems;
                public OneshotParticleSystemsManager oneshotParticleSystems;

                public Text particleCountText;
                public Text currentParticleSystemText;

                public Text particleSpawnInstructionText;

                public Slider timeScaleSlider;
                public Text timeScaleSliderValueText;

                public Camera UICamera;
                public Camera mainCamera;

                public Camera postEffectsCamera;

                public MonoBehaviour[] mainCameraPostEffects;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {
                    loopingParticleSystems.init();
                    oneshotParticleSystems.init();
                }

                // ...

                void Start()
                {
                    // ...

                    cameraPositionStart = cameraTranslationTransform.localPosition;
                    cameraRotationStart = cameraRotationTransform.localEulerAngles;

                    resetCameraTransformTargets();

                    // ...

                    switch (particleMode)
                    {
                        case ParticleMode.looping:
                            {
                                setToPerpetualParticleMode(true);

                                loopingParticleModeToggle.isOn = true;
                                oneshotParticleModeToggle.isOn = false;

                                break;
                            }
                        case ParticleMode.oneshot:
                            {
                                setToInstancedParticleMode(true);

                                loopingParticleModeToggle.isOn = false;
                                oneshotParticleModeToggle.isOn = true;

                                break;
                            }
                        default:
                            {
                                print("Unknown case.");
                                break;
                            }
                    }

                    // ...

                    setLighting(lighting);
                    setAdvancedRendering(advancedRendering);

                    lightingToggle.isOn = lighting;
                    advancedRenderingToggle.isOn = advancedRendering;

                    // ...

                    levelToggles =
                        levelTogglesContainer.GetComponentsInChildren<Toggle>(true);

                    for (int i = 0; i < levels.Length; i++)
                    {
                        // Toggle's OnValueChanged handles
                        // level state. No need to SetActive().

                        if (i == (int)currentLevel)
                        {
                            levels[i].SetActive(true);
                            levelToggles[i].isOn = true;
                        }
                        else
                        {
                            levels[i].SetActive(false);
                            levelToggles[i].isOn = false;
                        }
                    }

                    // ...

                    updateCurrentParticleSystemNameText();
                    timeScaleSlider.onValueChanged.AddListener(onTimeScaleSliderValueChanged);

                    onTimeScaleSliderValueChanged(timeScaleSlider.value);
                }

                // ...

                public void onTimeScaleSliderValueChanged(float value)
                {
                    Time.timeScale = value;
                    timeScaleSliderValueText.text = value.ToString("0.00");
                }

                // ...

                public void setToPerpetualParticleMode(bool set)
                {
                    if (set)
                    {
                        oneshotParticleSystems.clear();

                        loopingParticleSystems.gameObject.SetActive(true);
                        oneshotParticleSystems.gameObject.SetActive(false);

                        particleSpawnInstructionText.gameObject.SetActive(false);

                        particleMode = ParticleMode.looping;

                        updateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void setToInstancedParticleMode(bool set)
                {
                    if (set)
                    {
                        loopingParticleSystems.gameObject.SetActive(false);
                        oneshotParticleSystems.gameObject.SetActive(true);

                        particleSpawnInstructionText.gameObject.SetActive(true);

                        particleMode = ParticleMode.oneshot;

                        updateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void setLevel(Level level)
                {
                    for (int i = 0; i < levels.Length; i++)
                    {
                        if (i == (int)level)
                        {
                            levels[i].SetActive(true);
                        }
                        else
                        {
                            levels[i].SetActive(false);
                        }
                    }

                    currentLevel = level;
                }

                // ...

                public void setLevelFromToggle(Toggle toggle)
                {
                    if (toggle.isOn)
                    {
                        setLevel((Level)System.Array.IndexOf(levelToggles, toggle));
                    }
                }

                // ...

                public void setLighting(bool value)
                {
                    lighting = value;

                    loopingParticleSystems.setLights(value);
                    oneshotParticleSystems.setLights(value);
                }

                // ...

                public void setAdvancedRendering(bool value)
                {
                    advancedRendering = value;

                    postEffectsCamera.gameObject.SetActive(value);

                    UICamera.hdr = value;
                    mainCamera.hdr = value;

                    if (value)
                    {
                        QualitySettings.SetQualityLevel(32, true);

                        UICamera.renderingPath = RenderingPath.UsePlayerSettings;
                        mainCamera.renderingPath = RenderingPath.UsePlayerSettings;

                        lightingToggle.isOn = true;
                        mouse.gameObject.SetActive(true);
                    }
                    else
                    {
                        QualitySettings.SetQualityLevel(0, true);

                        UICamera.renderingPath = RenderingPath.VertexLit;
                        mainCamera.renderingPath = RenderingPath.VertexLit;

                        // If turning off, also disable lighting automatically.

                        lightingToggle.isOn = false;
                        mouse.gameObject.SetActive(false);
                    }

                    for (int i = 0; i < mainCameraPostEffects.Length; i++)
                    {
                        if (mainCameraPostEffects[i])
                        {
                            mainCameraPostEffects[i].enabled = value;
                        }
                    }
                }

                // ...

                public static Vector3 dampVector3(Vector3 from, Vector3 to, float speed, float dt)
                {
                    return Vector3.Lerp(from, to, 1.0f - Mathf.Exp(-speed * dt));
                }

                // ...

                void Update()
                {
                    // ...

                    input.x = Input.GetAxis("Horizontal");
                    input.y = Input.GetAxis("Vertical");

                    // Get targets.

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        targetCameraPosition.z += input.y * cameraMoveAmount;
                    }
                    else
                    {

                        targetCameraRotation.y += input.x * cameraRotateAmount;
                        targetCameraRotation.x += input.y * cameraRotateAmount;

                        targetCameraRotation.x = Mathf.Clamp(targetCameraRotation.x, cameraAngleLimits.x, cameraAngleLimits.y);
                    }

                    // Camera position.

                    cameraTranslationTransform.localPosition = Vector3.Lerp(
                        cameraTranslationTransform.localPosition, targetCameraPosition, Time.deltaTime * cameraMoveSpeed);

                    // Camera container rotation.

                    cameraRotation = dampVector3(
                        cameraRotation, targetCameraRotation, cameraRotationSpeed, Time.deltaTime);

                    cameraRotationTransform.localEulerAngles = cameraRotation;

                    // Look at origin.

                    cameraTranslationTransform.LookAt(cameraLookAtPosition);

                    // Scroll through systems.

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        next();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        previous();
                    }

                    // Random prefab while holding key.

                    else if (Input.GetKey(KeyCode.R))
                    {
                        if (particleMode == ParticleMode.oneshot)
                        {
                            oneshotParticleSystems.randomize();
                            updateCurrentParticleSystemNameText();

                            // If also holding down, auto-spawn at random point.

                            if (Input.GetKey(KeyCode.T))
                            {
                                //oneshotParticleSystems.instantiateParticlePrefabRandom();
                            }
                        }
                    }

                    // Left-click to spawn once.
                    // Right-click to continously spawn.

                    if (particleMode == ParticleMode.oneshot)
                    {
                        Vector3 mousePosition = Input.mousePosition;

                        if (Input.GetMouseButtonDown(0))
                        {
                            oneshotParticleSystems.instantiateParticlePrefab(mousePosition, mouse.distanceFromCamera);
                        }
                        if (Input.GetMouseButton(1))
                        {
                            oneshotParticleSystems.instantiateParticlePrefab(mousePosition, mouse.distanceFromCamera);
                        }
                    }

                    // Reset.

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        resetCameraTransformTargets();
                    }
                }

                // ...

                void LateUpdate()
                {
                    // Update particle count display.

                    particleCountText.text = "PARTICLE COUNT: ";

                    if (particleMode == ParticleMode.looping)
                    {
                        particleCountText.text += loopingParticleSystems.getParticleCount().ToString();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        particleCountText.text += oneshotParticleSystems.getParticleCount().ToString();
                    }
                }

                // ...

                void resetCameraTransformTargets()
                {
                    targetCameraPosition = cameraPositionStart;
                    targetCameraRotation = cameraRotationStart;
                }

                // ...

                void updateCurrentParticleSystemNameText()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        currentParticleSystemText.text = loopingParticleSystems.getCurrentPrefabName(true);
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        currentParticleSystemText.text = oneshotParticleSystems.getCurrentPrefabName(true);
                    }
                }

                // ...

                public void next()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.next();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.next();
                    }

                    updateCurrentParticleSystemNameText();
                }

                public void previous()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.previous();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.previous();
                    }

                    updateCurrentParticleSystemNameText();
                }

                // =================================	
                // End functions.
                // =================================

            }

            // =================================	
            // End namespace.
            // =================================

        }

    }

}

// =================================	
// --END-- //
// =================================
