using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "NoiseData", menuName = "ScriptableObjects/NoiseData", order = 1)]
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

    //public float Frequency = 0.01f;
    //public Vector2 Scale = Vector2.one;
    //public Bound Bounds = new Bound(-1, 1);
    ///// <summary>
    ///// 0 - Not discreting,..., X discreted func,..., 1 - discreted {0 or 1} noise
    ///// </summary>
    //[Range(0, 1)]
    //public float Discreting = 0;
    ///// <summary>
    ///// Is invert [0,1] to [1,0]
    ///// </summary>
    //public bool isInvert = false;
    //public NoiseGeneratorData()
    //{
    //    Frequency = 0.01f;
    //    Scale = Vector2.one;
    //    Bounds = new Bound(-1, 1);
    //    Discreting = 0;
    //    isInvert = false;
    //}
}
