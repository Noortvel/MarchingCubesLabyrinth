using UnityEngine;

#if UNITY_EDITOR
namespace MarchingCubes
{
    [RequireComponent(typeof(MarchingCubesChunk))]
    public class MarchingCubesChunkGizmoz : MonoBehaviour
    {
        [SerializeField]
        private float sphereRadius = 0.1f;
        private MarchingCubesChunk marchingCubes;

        [SerializeField, Header("Debug drawers")]
        private bool isFindedVertexDraw = false;
        [SerializeField]
        private bool isCubeCentersDraw = false;
        [SerializeField]
        private bool isNoiseHeightsDraw = false;

        private void OnValidate()
        {
            marchingCubes = GetComponent<MarchingCubesChunk>();
        }
        private void OnDrawGizmos()
        {
            DrawBound();
            if (Application.isPlaying)
            {
                if (isFindedVertexDraw)
                    DrawGeneratedPoints();
                if (isNoiseHeightsDraw)
                    DrawNoiseHeights();
                if (isCubeCentersDraw)
                    DrawCenters();
            }
        }
        private void DrawBound()
        {
            var size = marchingCubes.cubesCount.ToVector3() * marchingCubes.cubeSize;
            Gizmos.DrawWireCube(transform.position, size);
        }
        private void DrawNoiseHeights()
        {
            Gizmos.color = Color.yellow;

            var planeCenters = marchingCubes.MarchingCubesAlgorithm.PlaneCenters;
            foreach (var center in planeCenters)
            {
                var scalar = marchingCubes.noise.GetHeight(center.x, center.y);
                scalar *= marchingCubes.NoiseHeightScale;
                var position = new Vector3(center.x, scalar, center.y);
                Gizmos.DrawWireSphere(position, sphereRadius);
            }
        }
        private void DrawCenters()
        {
            Gizmos.color = Color.yellow;
            foreach (var x in marchingCubes.MarchingCubesAlgorithm.GridCenters)
            {
                Gizmos.DrawWireSphere(x, sphereRadius);
            }
        }
        private void DrawGeneratedPoints()
        {
            Gizmos.color = Color.red;
            foreach (var x in marchingCubes.MarchingCubesAlgorithm.FindedVertices)
            {
                Gizmos.DrawWireSphere(x, sphereRadius);
            }
        }
    }
}
#endif