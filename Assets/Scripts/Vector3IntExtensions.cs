using UnityEngine;

namespace MarchingCubes
{
    public static class Vector3IntExtensions
    {
        public static Vector3 ToVector3(this Vector3Int vector3Int)
        {
            return new Vector3(vector3Int.x, vector3Int.y, vector3Int.z);
        }
    }
}
