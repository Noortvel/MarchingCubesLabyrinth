using UnityEngine;
using UnityEngine.AI;

namespace MarchingCubes
{
    public class GameMaster : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint = null;
        [SerializeField]
        private NavMeshAgent character = null;
        [SerializeField]
        private MarchingCubesChunk chunk = null;
        public void Start()
        {
            Restart();
        }
        public void Restart()
        {
            float xoffset = Random.Range(-1000, 1000);
            float yoffset = Random.Range(-1000, 1000);
            chunk.noise.noiseData.Offset = new Vector2(xoffset, yoffset);
            chunk.Generate();
            character.Warp(spawnPoint.position);
        }
    }
}