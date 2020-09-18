using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Bound
{
    public Bound(float min, float max)
    {
        Min = min;
        Max = max;
    }
    public float Length { get => (Max - Min); }
    public float Min;
    public float Max;
}
