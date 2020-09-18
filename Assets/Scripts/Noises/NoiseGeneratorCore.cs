using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NoiseGenerator", menuName = "ScriptableObjects/NoiseData", order = 1)]
public class NoiseGeneratorCore : ScriptableObject
{
    public enum NoiseGeneratorType
    {
        Celluar,
        Perlin,
        Fractal
    }
    public NoiseGeneratorData noiseData;
    public NoiseGeneratorType generatorType = NoiseGeneratorType.Celluar;
    private FastNoise fastNoise = new FastNoise();
    private void OnEnable()
    {
        UpdateFastNoise();   
    }
    public float GetHeight(float x, float y)
    {
        x += noiseData.Offset.x;
        y += noiseData.Offset.y;
        x *= noiseData.Scale.x;
        y *= noiseData.Scale.y;
        float val = fastNoise.GetNoise(x, y);
        //Offset and normalize
        val += 0 - noiseData.Bounds.Min;
        val /= noiseData.Bounds.Length;
        if (noiseData.IsInvert)
            val = -(val - 1);
        if(noiseData.Discreting > Mathf.Epsilon)
        {
            int count = Mathf.RoundToInt(val / noiseData.Discreting);
            val = count * noiseData.Discreting;
        }
        return val;
    }
    public void UpdateFastNoise()
    {
        fastNoise.SetFrequency(noiseData.Frequency);
        fastNoise.SetSeed(noiseData.Seed);
        FastNoise.NoiseType noiseType = FastNoise.NoiseType.Cellular;
        switch (generatorType)
        {
            case NoiseGeneratorType.Celluar:
                noiseType = FastNoise.NoiseType.Cellular;
                break;
            case NoiseGeneratorType.Fractal:
                noiseType = FastNoise.NoiseType.SimplexFractal;
                break;
            case NoiseGeneratorType.Perlin:
                noiseType = FastNoise.NoiseType.Perlin;
                break;
        }
        fastNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance);
        fastNoise.SetNoiseType(noiseType);
    }
}
