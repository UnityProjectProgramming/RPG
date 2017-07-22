
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

using System;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Scripting
    {

        namespace Effects
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public static class Noise
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                // These variables are for simplex noise ->
                // based on Stefan Gustavson's C/C++ implementation.

                // Simple skewing factors for the 3D case.
                // Used for simplex noise.

                static float F3 = 0.333333333f;
                static float G3 = 0.166666667f;

                // =================================	
                // Functions.
                // =================================

                // ...

                // Return -1.0 -> 1.0.

                static float smooth(float t)
                {
                    return t * t * (3.0f - 2.0f * t);
                }
                static float fade(float t)
                {
                    return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
                }

                static int floor(float x)
                {
                    return x > 0 ? (int)x : (int)x - 1;
                }
                static float lerp(float from, float to, float t)
                {
                    return from + (t * (to - from));
                }

                // Mathf is usually just System.Math with a float cast.
                // Saving the extra function call and casting manually
                // has a noticeable (good) impact on performance and FPS.

                // Returns a pseudo-random value based on the input seed (x).

                //static float hash(float x)
                //{
                //    float sine = (float)(Math.Sin(x) * 43758.5453);
                //    float fractionalSine = sine - floor(sine);

                //    return fractionalSine;
                //}

                // ...

                static byte[] perm =
                {
                151,160,137,91,90,15,
                131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
                190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
                88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
                77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
                102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
                135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
                5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
                223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
                129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
                251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
                49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
                138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
                151,160,137,91,90,15,

                131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
                190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
                88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
                77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
                102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
                135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
                5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
                223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
                129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
                251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
                49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
                138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
            };

                // Instead of multiplying by gradient and 
                // getting dot product, get the final value directly.

                static float grad(int hash, float x, float y, float z)
                {
                    switch (hash & 0xF)
                    {
                        case 0x0: return x + y;
                        case 0x1: return -x + y;
                        case 0x2: return x - y;
                        case 0x3: return -x - y;
                        case 0x4: return x + x;
                        case 0x5: return -x + x;
                        case 0x6: return x - x;
                        case 0x7: return -x - x;
                        case 0x8: return y + x;
                        case 0x9: return -y + x;
                        case 0xA: return y - x;
                        case 0xB: return -y - x;
                        case 0xC: return y + z;
                        case 0xD: return -y + x;
                        case 0xE: return y - x;
                        case 0xF: return -y - z;

                        // Not executed.

                        default: return 0.0f;
                    }
                }

                // ...

                public static float perlin(float x, float y, float z)
                {
                    // Integer part (floor).

                    int ix0 = ((x) > 0) ? ((int)x) : ((int)x - 1);
                    int iy0 = ((y) > 0) ? ((int)y) : ((int)y - 1);
                    int iz0 = ((z) > 0) ? ((int)z) : ((int)z - 1);

                    // Fractional part (v - floor).

                    float fx0 = x - ix0;
                    float fy0 = y - iy0;
                    float fz0 = z - iz0;

                    // Fractional part minus one.                        

                    float fx1 = fx0 - 1.0f;
                    float fy1 = fy0 - 1.0f;
                    float fz1 = fz0 - 1.0f;

                    // Wrap to 0...255.

                    int ix1 = (ix0 + 1) & 255;
                    int iy1 = (iy0 + 1) & 255;
                    int iz1 = (iz0 + 1) & 255;

                    ix0 &= 255;
                    iy0 &= 255;
                    iz0 &= 255;

                    // Smooth / fade.

                    float r = fz0 * fz0 * fz0 * (fz0 * (fz0 * 6.0f - 15.0f) + 10.0f);
                    float t = fy0 * fy0 * fy0 * (fy0 * (fy0 * 6.0f - 15.0f) + 10.0f);
                    float s = fx0 * fx0 * fx0 * (fx0 * (fx0 * 6.0f - 15.0f) + 10.0f);

                    // Gradients.

                    int hash;
                    float gradient;

                    float nxy0, nxy1;
                    float nx0, nx1;
                    float n0, n1;

                    // --- 1

                    hash = perm[ix0 + perm[iy0 + perm[iz0]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx0 + fy0; break;
                        case 0x1: gradient = -fx0 + fy0; break;
                        case 0x2: gradient = fx0 - fy0; break;
                        case 0x3: gradient = -fx0 - fy0; break;
                        case 0x4: gradient = fx0 + fx0; break;
                        case 0x5: gradient = -fx0 + fx0; break;
                        case 0x6: gradient = fx0 - fx0; break;
                        case 0x7: gradient = -fx0 - fx0; break;
                        case 0x8: gradient = fy0 + fx0; break;
                        case 0x9: gradient = -fy0 + fx0; break;
                        case 0xA: gradient = fy0 - fx0; break;
                        case 0xB: gradient = -fy0 - fx0; break;
                        case 0xC: gradient = fy0 + fz0; break;
                        case 0xD: gradient = -fy0 + fx0; break;
                        case 0xE: gradient = fy0 - fx0; break;
                        case 0xF: gradient = -fy0 - fz0; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy0 = gradient;

                    // --- 2

                    hash = perm[ix0 + perm[iy0 + perm[iz1]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx0 + fy0; break;
                        case 0x1: gradient = -fx0 + fy0; break;
                        case 0x2: gradient = fx0 - fy0; break;
                        case 0x3: gradient = -fx0 - fy0; break;
                        case 0x4: gradient = fx0 + fx0; break;
                        case 0x5: gradient = -fx0 + fx0; break;
                        case 0x6: gradient = fx0 - fx0; break;
                        case 0x7: gradient = -fx0 - fx0; break;
                        case 0x8: gradient = fy0 + fx0; break;
                        case 0x9: gradient = -fy0 + fx0; break;
                        case 0xA: gradient = fy0 - fx0; break;
                        case 0xB: gradient = -fy0 - fx0; break;
                        case 0xC: gradient = fy0 + fz1; break;
                        case 0xD: gradient = -fy0 + fx0; break;
                        case 0xE: gradient = fy0 - fx0; break;
                        case 0xF: gradient = -fy0 - fz1; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy1 = gradient;

                    // ---

                    nx0 = nxy0 + (r * (nxy1 - nxy0));

                    // --- 3

                    hash = perm[ix0 + perm[iy1 + perm[iz0]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx0 + fy1; break;
                        case 0x1: gradient = -fx0 + fy1; break;
                        case 0x2: gradient = fx0 - fy1; break;
                        case 0x3: gradient = -fx0 - fy1; break;
                        case 0x4: gradient = fx0 + fx0; break;
                        case 0x5: gradient = -fx0 + fx0; break;
                        case 0x6: gradient = fx0 - fx0; break;
                        case 0x7: gradient = -fx0 - fx0; break;
                        case 0x8: gradient = fy1 + fx0; break;
                        case 0x9: gradient = -fy1 + fx0; break;
                        case 0xA: gradient = fy1 - fx0; break;
                        case 0xB: gradient = -fy1 - fx0; break;
                        case 0xC: gradient = fy1 + fz0; break;
                        case 0xD: gradient = -fy1 + fx0; break;
                        case 0xE: gradient = fy1 - fx0; break;
                        case 0xF: gradient = -fy1 - fz0; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy0 = gradient;

                    // --- 4

                    hash = perm[ix0 + perm[iy1 + perm[iz1]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx0 + fy1; break;
                        case 0x1: gradient = -fx0 + fy1; break;
                        case 0x2: gradient = fx0 - fy1; break;
                        case 0x3: gradient = -fx0 - fy1; break;
                        case 0x4: gradient = fx0 + fx0; break;
                        case 0x5: gradient = -fx0 + fx0; break;
                        case 0x6: gradient = fx0 - fx0; break;
                        case 0x7: gradient = -fx0 - fx0; break;
                        case 0x8: gradient = fy1 + fx0; break;
                        case 0x9: gradient = -fy1 + fx0; break;
                        case 0xA: gradient = fy1 - fx0; break;
                        case 0xB: gradient = -fy1 - fx0; break;
                        case 0xC: gradient = fy1 + fz1; break;
                        case 0xD: gradient = -fy1 + fx0; break;
                        case 0xE: gradient = fy1 - fx0; break;
                        case 0xF: gradient = -fy1 - fz1; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy1 = gradient;

                    // ---

                    nx1 = nxy0 + (r * (nxy1 - nxy0));

                    // ---

                    n0 = nx0 + (t * (nx1 - nx0));

                    // --- 5

                    hash = perm[ix1 + perm[iy0 + perm[iz0]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx1 + fy0; break;
                        case 0x1: gradient = -fx1 + fy0; break;
                        case 0x2: gradient = fx1 - fy0; break;
                        case 0x3: gradient = -fx1 - fy0; break;
                        case 0x4: gradient = fx1 + fx1; break;
                        case 0x5: gradient = -fx1 + fx1; break;
                        case 0x6: gradient = fx1 - fx1; break;
                        case 0x7: gradient = -fx1 - fx1; break;
                        case 0x8: gradient = fy0 + fx1; break;
                        case 0x9: gradient = -fy0 + fx1; break;
                        case 0xA: gradient = fy0 - fx1; break;
                        case 0xB: gradient = -fy0 - fx1; break;
                        case 0xC: gradient = fy0 + fz0; break;
                        case 0xD: gradient = -fy0 + fx1; break;
                        case 0xE: gradient = fy0 - fx1; break;
                        case 0xF: gradient = -fy0 - fz0; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy0 = gradient;

                    // --- 6

                    hash = perm[ix1 + perm[iy0 + perm[iz1]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx1 + fy0; break;
                        case 0x1: gradient = -fx1 + fy0; break;
                        case 0x2: gradient = fx1 - fy0; break;
                        case 0x3: gradient = -fx1 - fy0; break;
                        case 0x4: gradient = fx1 + fx1; break;
                        case 0x5: gradient = -fx1 + fx1; break;
                        case 0x6: gradient = fx1 - fx1; break;
                        case 0x7: gradient = -fx1 - fx1; break;
                        case 0x8: gradient = fy0 + fx1; break;
                        case 0x9: gradient = -fy0 + fx1; break;
                        case 0xA: gradient = fy0 - fx1; break;
                        case 0xB: gradient = -fy0 - fx1; break;
                        case 0xC: gradient = fy0 + fz1; break;
                        case 0xD: gradient = -fy0 + fx1; break;
                        case 0xE: gradient = fy0 - fx1; break;
                        case 0xF: gradient = -fy0 - fz1; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy1 = gradient;

                    // --- 7

                    nx0 = nxy0 + (r * (nxy1 - nxy0));

                    // ---

                    hash = perm[ix1 + perm[iy1 + perm[iz0]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx1 + fy1; break;
                        case 0x1: gradient = -fx1 + fy1; break;
                        case 0x2: gradient = fx1 - fy1; break;
                        case 0x3: gradient = -fx1 - fy1; break;
                        case 0x4: gradient = fx1 + fx1; break;
                        case 0x5: gradient = -fx1 + fx1; break;
                        case 0x6: gradient = fx1 - fx1; break;
                        case 0x7: gradient = -fx1 - fx1; break;
                        case 0x8: gradient = fy1 + fx1; break;
                        case 0x9: gradient = -fy1 + fx1; break;
                        case 0xA: gradient = fy1 - fx1; break;
                        case 0xB: gradient = -fy1 - fx1; break;
                        case 0xC: gradient = fy1 + fz0; break;
                        case 0xD: gradient = -fy1 + fx1; break;
                        case 0xE: gradient = fy1 - fx1; break;
                        case 0xF: gradient = -fy1 - fz0; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy0 = gradient;

                    // --- 8

                    hash = perm[ix1 + perm[iy1 + perm[iz1]]];

                    switch (hash & 0xF)
                    {
                        case 0x0: gradient = fx1 + fy1; break;
                        case 0x1: gradient = -fx1 + fy1; break;
                        case 0x2: gradient = fx1 - fy1; break;
                        case 0x3: gradient = -fx1 - fy1; break;
                        case 0x4: gradient = fx1 + fx1; break;
                        case 0x5: gradient = -fx1 + fx1; break;
                        case 0x6: gradient = fx1 - fx1; break;
                        case 0x7: gradient = -fx1 - fx1; break;
                        case 0x8: gradient = fy1 + fx1; break;
                        case 0x9: gradient = -fy1 + fx1; break;
                        case 0xA: gradient = fy1 - fx1; break;
                        case 0xB: gradient = -fy1 - fx1; break;
                        case 0xC: gradient = fy1 + fz1; break;
                        case 0xD: gradient = -fy1 + fx1; break;
                        case 0xE: gradient = fy1 - fx1; break;
                        case 0xF: gradient = -fy1 - fz1; break;

                        default: gradient = 0.0f; break;
                    }

                    nxy1 = gradient;

                    // ---

                    nx1 = nxy0 + (r * (nxy1 - nxy0));

                    // ---

                    n1 = nx0 + (t * (nx1 - nx0));

                    // ---

                    return 0.936f * (n0 + (s * (n1 - n0)));
                }
                
                // Based on Stefan Gustavson's C/C++ implementation.

                public static float simplex(float x, float y, float z)
                {
                    float n0, n1, n2, n3; // Noise contributions from the four corners

                    // Skew the input space to determine which simplex cell we're in.
                    float s = (x + y + z) * F3; // Very nice and simple skew factor for 3D.

                    float xs = x + s;
                    float ys = y + s;
                    float zs = z + s;

                    int i = xs > 0 ? (int)xs : (int)xs - 1;
                    int j = ys > 0 ? (int)ys : (int)ys - 1;
                    int k = zs > 0 ? (int)zs : (int)zs - 1;

                    float t = (i + j + k) * G3;

                    float X0 = i - t; // Unskew the cell origin back to (x, y, z) space
                    float Y0 = j - t;
                    float Z0 = k - t;

                    float x0 = x - X0; // The x, y, z distances from the cell origin.
                    float y0 = y - Y0;
                    float z0 = z - Z0;

                    // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
                    // Determine which simplex we are in.

                    int i1, j1, k1; // Offsets for second corner of simplex in (i, j, k) coords.
                    int i2, j2, k2; // Offsets for third corner of simplex in (i, j, k) coords.

                    /* This code would benefit from a backport from the GLSL version! */

                    if (x0 >= y0)
                    {
                        if (y0 >= z0)       // X Y Z order
                        {
                            i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                        }
                        else if (x0 >= z0)  // X Z Y order
                        {
                            i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;
                        }
                        else                // Z X Y order
                        {
                            i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;
                        }
                    }
                    else
                    {
                        // x0 < y0.

                        if (y0 < z0)        // Z Y X order.
                        {
                            i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;
                        }
                        else if (x0 < z0)   // Y Z X order.
                        {
                            i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;
                        }
                        else                // Y X Z order.
                        {
                            i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                        }
                    }

                    // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
                    // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
                    // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
                    // c = 1/6.

                    float x1 = x0 - i1 + G3;            // Offsets for second corner in (x, y, z) coords.
                    float y1 = y0 - j1 + G3;
                    float z1 = z0 - k1 + G3;

                    float x2 = x0 - i2 + 2.0f * G3;     // Offsets for third corner in (x, y, z) coords.
                    float y2 = y0 - j2 + 2.0f * G3;
                    float z2 = z0 - k2 + 2.0f * G3;

                    float x3 = x0 - 1.0f + 3.0f * G3;   // Offsets for last corner in (x, y, z) coords.
                    float y3 = y0 - 1.0f + 3.0f * G3;
                    float z3 = z0 - 1.0f + 3.0f * G3;

                    // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds.

                    int ii = i & 0xff;
                    int jj = j & 0xff;
                    int kk = k & 0xff;

                    // Calculate the contribution from the four corners.

                    float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;

                    // Gradients.

                    //int h;

                    int hash;
                    float gradient;

                    if (t0 < 0.0f)
                    {
                        n0 = 0.0f;
                    }
                    else
                    {
                        t0 *= t0;

                        // --- 1

                        hash = perm[ii + perm[jj + perm[kk]]];

                        switch (hash & 0xF)
                        {
                            case 0x0: gradient = x0 + y0; break;
                            case 0x1: gradient = -x0 + y0; break;
                            case 0x2: gradient = x0 - y0; break;
                            case 0x3: gradient = -x0 - y0; break;
                            case 0x4: gradient = x0 + x0; break;
                            case 0x5: gradient = -x0 + x0; break;
                            case 0x6: gradient = x0 - x0; break;
                            case 0x7: gradient = -x0 - x0; break;
                            case 0x8: gradient = y0 + x0; break;
                            case 0x9: gradient = -y0 + x0; break;
                            case 0xA: gradient = y0 - x0; break;
                            case 0xB: gradient = -y0 - x0; break;
                            case 0xC: gradient = y0 + z0; break;
                            case 0xD: gradient = -y0 + x0; break;
                            case 0xE: gradient = y0 - x0; break;
                            case 0xF: gradient = -y0 - z0; break;

                            default: gradient = 0.0f; break;
                        }

                        n0 = t0 * t0 * gradient;
                    }

                    float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;

                    if (t1 < 0.0f)
                    {
                        n1 = 0.0f;
                    }
                    else
                    {
                        t1 *= t1;

                        hash = perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]];

                        switch (hash & 0xF)
                        {
                            case 0x0: gradient = x1 + y1; break;
                            case 0x1: gradient = -x1 + y1; break;
                            case 0x2: gradient = x1 - y1; break;
                            case 0x3: gradient = -x1 - y1; break;
                            case 0x4: gradient = x1 + x1; break;
                            case 0x5: gradient = -x1 + x1; break;
                            case 0x6: gradient = x1 - x1; break;
                            case 0x7: gradient = -x1 - x1; break;
                            case 0x8: gradient = y1 + x1; break;
                            case 0x9: gradient = -y1 + x1; break;
                            case 0xA: gradient = y1 - x1; break;
                            case 0xB: gradient = -y1 - x1; break;
                            case 0xC: gradient = y1 + z1; break;
                            case 0xD: gradient = -y1 + x1; break;
                            case 0xE: gradient = y1 - x1; break;
                            case 0xF: gradient = -y1 - z1; break;

                            default: gradient = 0.0f; break;
                        }

                        n1 = t1 * t1 * gradient;
                    }

                    float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;

                    if (t2 < 0.0f)
                    {
                        n2 = 0.0f;
                    }
                    else
                    {
                        t2 *= t2;

                        hash = perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]];

                        switch (hash & 0xF)
                        {
                            case 0x0: gradient = x2 + y2; break;
                            case 0x1: gradient = -x2 + y2; break;
                            case 0x2: gradient = x2 - y2; break;
                            case 0x3: gradient = -x2 - y2; break;
                            case 0x4: gradient = x2 + x2; break;
                            case 0x5: gradient = -x2 + x2; break;
                            case 0x6: gradient = x2 - x2; break;
                            case 0x7: gradient = -x2 - x2; break;
                            case 0x8: gradient = y2 + x2; break;
                            case 0x9: gradient = -y2 + x2; break;
                            case 0xA: gradient = y2 - x2; break;
                            case 0xB: gradient = -y2 - x2; break;
                            case 0xC: gradient = y2 + z2; break;
                            case 0xD: gradient = -y2 + x2; break;
                            case 0xE: gradient = y2 - x2; break;
                            case 0xF: gradient = -y2 - z2; break;

                            default: gradient = 0.0f; break;
                        }

                        n2 = t2 * t2 * gradient;
                    }

                    float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;

                    if (t3 < 0.0f)
                    {
                        n3 = 0.0f;
                    }
                    else
                    {
                        t3 *= t3;

                        hash = perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]];

                        switch (hash & 0xF)
                        {
                            case 0x0: gradient = x3 + y3; break;
                            case 0x1: gradient = -x3 + y3; break;
                            case 0x2: gradient = x3 - y3; break;
                            case 0x3: gradient = -x3 - y3; break;
                            case 0x4: gradient = x3 + x3; break;
                            case 0x5: gradient = -x3 + x3; break;
                            case 0x6: gradient = x3 - x3; break;
                            case 0x7: gradient = -x3 - x3; break;
                            case 0x8: gradient = y3 + x3; break;
                            case 0x9: gradient = -y3 + x3; break;
                            case 0xA: gradient = y3 - x3; break;
                            case 0xB: gradient = -y3 - x3; break;
                            case 0xC: gradient = y3 + z3; break;
                            case 0xD: gradient = -y3 + x3; break;
                            case 0xE: gradient = y3 - x3; break;
                            case 0xF: gradient = -y3 - z3; break;

                            default: gradient = 0.0f; break;
                        }

                        n3 = t3 * t3 * gradient;
                    }

                    // Add contributions from each corner to get the final noise value.
                    // The result is scaled to stay just inside [-1, 1].

                    return 32.0f * (n0 + n1 + n2 + n3);
                }

                // ...

                public static float octavePerlin(float x, float y, float z, float frequency, int octaves, float lacunarity, float persistence)
                {
                    // 0 and 1 will do nothing.

                    if (octaves < 2)
                    {
                        return perlin(x * frequency, y * frequency, z * frequency);
                    }
                    else
                    {
                        float total = 0.0f;
                        float amplitude = 1.0f;

                        float max = 0.0f;

                        for (int i = 0; i < octaves; i++)
                        {
                            total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

                            max += amplitude;

                            frequency *= lacunarity;
                            amplitude *= persistence;
                        }

                        return total / max;
                    }
                }

                // ...

                public static float octaveSimplex(float x, float y, float z, float frequency, int octaves, float lacunarity, float persistence)
                {
                    // 0 and 1 will do nothing.

                    if (octaves < 2)
                    {
                        return simplex(x * frequency, y * frequency, z * frequency);
                    }
                    else
                    {
                        float total = 0.0f;
                        float amplitude = 1.0f;

                        float max = 0.0f;

                        for (int i = 0; i < octaves; i++)
                        {
                            total += simplex(x * frequency, y * frequency, z * frequency) * amplitude;

                            max += amplitude;

                            frequency *= lacunarity;
                            amplitude *= persistence;
                        }

                        return total / max;
                    }
                }
                
                // Based on Stefan Gustavson's C/C++ implementation.
                // Easier to understand, but not optimized (function calls aren't rolled out).

                public static float perlinUnoptimized(float x, float y, float z)
                {
                    int ix0, iy0, iz0;
                    int ix1, iy1, iz1;

                    float fx0, fy0, fz0;
                    float fx1, fy1, fz1;

                    // Integer part (floor).

                    ix0 = floor(x);
                    iy0 = floor(y);
                    iz0 = floor(z);

                    // Fractional part (v - floor).

                    fx0 = x - ix0;
                    fy0 = y - iy0;
                    fz0 = z - iz0;

                    // Fractional part minus one.    

                    fx1 = fx0 - 1.0f;
                    fy1 = fy0 - 1.0f;
                    fz1 = fz0 - 1.0f;

                    // Wrap to 0...255.

                    ix1 = (ix0 + 1) & 255;
                    iy1 = (iy0 + 1) & 255;
                    iz1 = (iz0 + 1) & 255;

                    ix0 &= 255;
                    iy0 &= 255;
                    iz0 &= 255;

                    // Smooth / fade.

                    float s, t, r;

                    r = fade(fz0);
                    t = fade(fy0);
                    s = fade(fx0);

                    // Gradients.

                    float nxy0, nxy1;
                    float nx0, nx1;
                    float n0, n1;

                    nxy0 = grad(perm[ix0 + perm[iy0 + perm[iz0]]], fx0, fy0, fz0);
                    nxy1 = grad(perm[ix0 + perm[iy0 + perm[iz1]]], fx0, fy0, fz1);
                    nx0 = lerp(nxy0, nxy1, r);

                    nxy0 = grad(perm[ix0 + perm[iy1 + perm[iz0]]], fx0, fy1, fz0);
                    nxy1 = grad(perm[ix0 + perm[iy1 + perm[iz1]]], fx0, fy1, fz1);
                    nx1 = lerp(nxy0, nxy1, r);

                    n0 = lerp(nx0, nx1, t);

                    nxy0 = grad(perm[ix1 + perm[iy0 + perm[iz0]]], fx1, fy0, fz0);
                    nxy1 = grad(perm[ix1 + perm[iy0 + perm[iz1]]], fx1, fy0, fz1);
                    nx0 = lerp(nxy0, nxy1, r);

                    nxy0 = grad(perm[ix1 + perm[iy1 + perm[iz0]]], fx1, fy1, fz0);
                    nxy1 = grad(perm[ix1 + perm[iy1 + perm[iz1]]], fx1, fy1, fz1);
                    nx1 = lerp(nxy0, nxy1, r);

                    n1 = lerp(nx0, nx1, t);

                    return 0.936f * (lerp(n0, n1, s));
                }

                // ...

                public static float simplexUnoptimized(float x, float y, float z)
                {
                    float n0, n1, n2, n3; // Noise contributions from the four corners

                    // Skew the input space to determine which simplex cell we're in.
                    float s = (x + y + z) * F3; // Very nice and simple skew factor for 3D.

                    float xs = x + s;
                    float ys = y + s;
                    float zs = z + s;

                    int i = floor(xs);
                    int j = floor(ys);
                    int k = floor(zs);

                    float t = (i + j + k) * G3;

                    float X0 = i - t; // Unskew the cell origin back to (x, y, z) space
                    float Y0 = j - t;
                    float Z0 = k - t;

                    float x0 = x - X0; // The x, y, z distances from the cell origin.
                    float y0 = y - Y0;
                    float z0 = z - Z0;

                    // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
                    // Determine which simplex we are in.

                    int i1, j1, k1; // Offsets for second corner of simplex in (i, j, k) coords.
                    int i2, j2, k2; // Offsets for third corner of simplex in (i, j, k) coords.

                    /* This code would benefit from a backport from the GLSL version! */

                    if (x0 >= y0)
                    {
                        if (y0 >= z0)       // X Y Z order
                        {
                            i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                        }
                        else if (x0 >= z0)  // X Z Y order
                        {
                            i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;
                        }
                        else                // Z X Y order
                        {
                            i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;
                        }
                    }
                    else
                    {
                        // x0 < y0.

                        if (y0 < z0)        // Z Y X order.
                        {
                            i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;
                        }
                        else if (x0 < z0)   // Y Z X order.
                        {
                            i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;
                        }
                        else                // Y X Z order.
                        {
                            i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                        }
                    }

                    // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
                    // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
                    // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
                    // c = 1/6.

                    float x1 = x0 - i1 + G3;            // Offsets for second corner in (x, y, z) coords.
                    float y1 = y0 - j1 + G3;
                    float z1 = z0 - k1 + G3;

                    float x2 = x0 - i2 + 2.0f * G3;     // Offsets for third corner in (x, y, z) coords.
                    float y2 = y0 - j2 + 2.0f * G3;
                    float z2 = z0 - k2 + 2.0f * G3;

                    float x3 = x0 - 1.0f + 3.0f * G3;   // Offsets for last corner in (x, y, z) coords.
                    float y3 = y0 - 1.0f + 3.0f * G3;
                    float z3 = z0 - 1.0f + 3.0f * G3;

                    // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds.

                    int ii = i & 0xff;
                    int jj = j & 0xff;
                    int kk = k & 0xff;

                    // Calculate the contribution from the four corners.

                    float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;

                    if (t0 < 0.0f)
                    {
                        n0 = 0.0f;
                    }
                    else
                    {
                        t0 *= t0;
                        n0 = t0 * t0 * grad(perm[ii + perm[jj + perm[kk]]], x0, y0, z0);
                    }

                    float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;

                    if (t1 < 0.0f)
                    {
                        n1 = 0.0f;
                    }
                    else
                    {
                        t1 *= t1;
                        n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]], x1, y1, z1);
                    }

                    float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;

                    if (t2 < 0.0f)
                    {
                        n2 = 0.0f;
                    }
                    else
                    {
                        t2 *= t2;
                        n2 = t2 * t2 * grad(perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]], x2, y2, z2);
                    }

                    float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;

                    if (t3 < 0.0f)
                    {
                        n3 = 0.0f;
                    }
                    else
                    {
                        t3 *= t3;
                        n3 = t3 * t3 * grad(perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]], x3, y3, z3);
                    }

                    // Add contributions from each corner to get the final noise value.
                    // The result is scaled to stay just inside [-1, 1].

                    return 32.0f * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
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
