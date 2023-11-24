using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public int spawnPointIndex;
        public LevelObjectToSpawn LevelObjectToSpawn;
#endif
    }
}
