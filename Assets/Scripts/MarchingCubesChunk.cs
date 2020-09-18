using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

namespace MarchingCubes
{
    public class MarchingCubesChunk : MonoBehaviour
    {
        public float cubeSize = 1;
        public Vector3Int cubesCount = Vector3Int.zero;
        public float isoLevel = 1;
        public bool isIntropolateCubesDistance = true;
        public float NoiseHeightScale = 1f;
        public float NoiseBorder = 0f;
        public NoiseGeneratorCore noise = null;
        
        public Vector3 Bound { get => cubeSize * cubesCount.ToVector3(); }

        public MarchingCubesAlgorithm MarchingCubesAlgorithm
        {
            private set;
            get;
        }
        private NavMeshSurface navMeshSurface;
        private MeshCollider meshCollider;
        private MeshFilter meshFilter;
        private Mesh mesh;

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            MarchingCubesAlgorithm = new MarchingCubesAlgorithm(cubeSize, cubesCount, ScalarField);
            MarchingCubesAlgorithm.IsIntropolateCubesDistance = isIntropolateCubesDistance;
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            navMeshSurface = GetComponent<NavMeshSurface>();
            mesh = new Mesh();
            mesh.name = "GeneratedByMarchingCubes";
        }
        public float ScalarField(Vector3 position)
        {
            var norm = new Vector2(position.x / (Bound.x * 0.5f), position.z / (Bound.z * 0.5f));
            float val = 0;
            if (Mathf.Abs(norm.x) < NoiseBorder && Mathf.Abs(norm.y) < NoiseBorder)
            {
                val = noise.GetHeight(position.x, position.z) * NoiseHeightScale;
            }
            return position.y - val;
        }
        private void ApplyMesh()
        {
            mesh.Clear();
            var verts = new Vector3[MarchingCubesAlgorithm.FindedVertices.Count];
            MarchingCubesAlgorithm.FindedVertices.CopyTo(verts, 0);
            mesh.vertices = verts;
            int[] triangles = new int[MarchingCubesAlgorithm.FindedVertices.Count];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            navMeshSurface.BuildNavMesh();
        }
        public void Generate()
        {
            MarchingCubesAlgorithm.IsIntropolateCubesDistance = isIntropolateCubesDistance;
            MarchingCubesAlgorithm.Generate();
            ApplyMesh();
        }
        [ExecuteInEditMode]
        public void GenerateFromEditor()
        {
            Initialize();
            Generate();
        }
    }
}