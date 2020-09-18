using MarchingCubes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingCubesChunk))]
public class MarchingCubesChunkEditor : Editor
{
    private Texture2D texture;
    private MarchingCubesChunk chunk;
    private NoiseGeneratorCore noise;

    private SerializedObject noiseGeneratorObject;
    private SerializedProperty noiseData;

    private void OnEnable()
    {
        texture = new Texture2D(256, 256);
        chunk = target as MarchingCubesChunk;
        noise = chunk.noise;
        if (noise != null)
        {
            chunk.noise.UpdateFastNoise();
            noiseGeneratorObject = new SerializedObject(noise);
            noiseData = noiseGeneratorObject.FindProperty("noiseData");

            RedrawTexture();
        }
    }
    private void RedrawTexture()
    {
        var halfBound = chunk.Bound / 2;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                var xPos = (x / (float)texture.width * 2 - 1) * halfBound.x;
                var yPos = (y / (float)texture.height * 2 - 1) * halfBound.z;
                var position = new Vector3(xPos, 0, yPos);
                var col = -chunk.ScalarField(position);
                texture.SetPixel(x, y, new Color(col, col, col));
            }
        }
        texture.Apply();
    }
    private NoiseGeneratorCore oldNoiseGeneratorCore;
    public override void OnInspectorGUI()
    {
        oldNoiseGeneratorCore = chunk.noise;
        base.DrawDefaultInspector();
        noiseGeneratorObject.Update();
        if (noise != null)
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.LabelField("Noise Settings", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(noiseData);
            noiseGeneratorObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck() || chunk.noise != oldNoiseGeneratorCore)
            {
                noise.UpdateFastNoise();
                RedrawTexture();
            }
            EditorGUILayout.LabelField("Noise Preview");
            GUILayout.Box(texture);
        }
    }
    void OnSceneGUI()
    {
        Handles.BeginGUI();
        if (GUI.Button(new Rect(0,0, 100, 60), "Generate"))
        {
            chunk.GenerateFromEditor();
        }
        Handles.EndGUI();
    }
}