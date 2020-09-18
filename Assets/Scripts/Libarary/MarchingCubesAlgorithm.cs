using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarchingCubes
{
    public class MarchingCubesAlgorithm
    {
        public float CubeSize = 1;
        public Vector3Int CubesCount = Vector3Int.one;
        public float IsoLevel = 1;
        public Func<Vector3, float> ScalarFieldFunction;
        public Vector3 Position = Vector3.zero;
        public bool IsIntropolateCubesDistance = false;
        public Vector3 HalfBound { get => CubeSize / 2 * CubesCount.ToVector3(); }
        public Vector3 Bound { get => CubeSize * CubesCount.ToVector3(); }

        private Vector3[] cubesVertexRaw = new Vector3[8]
        {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 0, 1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 1, 1)
        };
        private Vector3[] cubesVertexScaled = new Vector3[8];
        public IList<Vector3> CubesVertexLocal
        {
            get => cubesVertexScaled;
        }
        private List<Vector3> meshVertices = new List<Vector3>();
        public IList<Vector3> FindedVertices
        {
            get => meshVertices;
        }

        public MarchingCubesAlgorithm()
        {
            ScaleCubeVertices();
        }
        public MarchingCubesAlgorithm(float cubeSize, Vector3Int cubesCount, Func<Vector3, float> scalarFieldFunction)
        {
            CubeSize = cubeSize;
            CubesCount = cubesCount;
            ScalarFieldFunction = scalarFieldFunction;
            ScaleCubeVertices();
        }

        public IEnumerable<Vector3> GridCenters
        {
            get
            {
                var halfSize = CubeSize / 2;
                for (float y = -HalfBound.y; y < HalfBound.y; y += CubeSize)
                {
                    for (float x = -HalfBound.x; x < HalfBound.x; x += CubeSize)
                    {
                        for (float z = -HalfBound.z; z < HalfBound.z; z += CubeSize)
                        {
                            yield return Position + new Vector3(x + halfSize, y + halfSize, z + halfSize);
                        }
                    }
                }
            }
        }
        public IEnumerable<Vector2> PlaneCenters
        {
            get
            {
                var halfSize = CubeSize / 2;
                var planePosition = new Vector2(Position.x, Position.z);
                for (float x = -HalfBound.x; x < HalfBound.x; x += CubeSize)
                {
                    for (float z = -HalfBound.z; z < HalfBound.z; z += CubeSize)
                    {
                        yield return planePosition + new Vector2(x + halfSize, z + halfSize);
                    }
                }
            }
        }
        private void ScaleCubeVertices()
        {
            for (int i = 0; i < cubesVertexScaled.Length; i++)
            {
                cubesVertexScaled[i] = ScalePointOffset(cubesVertexRaw[i]);
            }
        }
        private Vector3 ScalePointOffset(Vector3 offset)
        {
            return offset * CubeSize - Vector3.one * CubeSize / 2;
        }

        public void Generate()
        {
            if (ScalarFieldFunction == null)
            {
                throw new Exception("Scalar Field Function is not setted");
            }
            meshVertices.Clear();
            var gridCenters = GridCenters;
            foreach (var cubeCenter in gridCenters)
            {
                CubeChecking(cubeCenter);
            }
        }
        private void CubeChecking(Vector3 cubeCenter)
        {
            int cubeIndex = 0;
            int i = 0;
            foreach (var localVertex in CubesVertexLocal)
            {
                var current = cubeCenter + localVertex;
                var scalar = ScalarFieldFunction(current);
                if (scalar < IsoLevel)
                    cubeIndex |= (1 << i);
                i++;
            }
            if (MarchingCubesTable.Edges[cubeIndex] != 0)
                AddTriangles(cubeIndex, cubeCenter);
        }
        private Vector3 InterpolateVerts(Vector3 v1, Vector3 v2)
        {
            float s1 = ScalarFieldFunction(v1);
            float s2 = ScalarFieldFunction(v2);
            float t = 0.5f;
            if (IsIntropolateCubesDistance)
            {
                t = (IsoLevel - s1) / (s2 - s1);
            }
            return v1 + t * (v2 - v1);
        }

        private void AddTriangles(int cubeIndex, Vector3 cubeCenter)
        {
            var triangulation = MarchingCubesTable.Triangulation;
            var cornerIndexAFromEdge = MarchingCubesTable.CornerIndexAFromEdge;
            var cornerIndexBFromEdge = MarchingCubesTable.CornerIndexBFromEdge;
            for (int i = 0; triangulation[cubeIndex][i] != -1; i += 3)
            {
                // Get indices of corner points A and B for each of the three edges
                // of the cube that need to be joined to form the triangle.
                int a0 = cornerIndexAFromEdge[triangulation[cubeIndex][i]];
                int b0 = cornerIndexBFromEdge[triangulation[cubeIndex][i]];

                int a1 = cornerIndexAFromEdge[triangulation[cubeIndex][i + 1]];
                int b1 = cornerIndexBFromEdge[triangulation[cubeIndex][i + 1]];

                int a2 = cornerIndexAFromEdge[triangulation[cubeIndex][i + 2]];
                int b2 = cornerIndexBFromEdge[triangulation[cubeIndex][i + 2]];

                var indexs = new[] { (a0, b0), (a1, b1), (a2, b2) };
                foreach (var index in indexs)
                {
                    var p1 = cubeCenter + cubesVertexScaled[index.Item1];
                    var p2 = cubeCenter + cubesVertexScaled[index.Item2];
                    meshVertices.Add(InterpolateVerts(p1, p2));
                }
            }
        }
    }
}