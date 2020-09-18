using System;
using UnityEngine;
namespace MarchingCubes
{
    [Serializable]
    public struct NoiseGeneratorData
    {

        public int Seed;
        public float Frequency;
        public Vector2 Scale;
        public Bound Bounds;
        /// <summary>
        /// 0 - Not discreting,..., X discreted func,..., 1 - discreted {0 or 1} noise
        /// </summary>
        [Range(0, 1)]
        public float Discreting;
        /// <summary>
        /// Is invert [0,1] to [1,0]
        /// </summary>
        public bool IsInvert;
        public Vector2 Offset;
    }
}