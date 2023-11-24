using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

namespace LevelEditor
{
    public class NewLevel : MonoBehaviour
    {
#if UNITY_EDITOR
        public int levelNumber;

        [Foldout("Fixed Variables")]  public LevelEditor levelEditor;
        [Foldout("Fixed Variables")] public SaveLevel saveLevel;

        [Button]
        public void CreateNewLevel()
        {
            levelEditor.NewLevel();

            string path = saveLevel.levelDataPath;
            string name = saveLevel.levelDataName + levelNumber.ToString();

            if (AssetDatabase.FindAssets(name, new[] { path }).Length != 0)
            {
                Debug.Log("Level with number " + levelNumber.ToString() + " already exist. it will be overwritten with your new changes.");
            }
        }
#endif
    }
}