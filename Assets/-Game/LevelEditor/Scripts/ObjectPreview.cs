using NaughtyAttributes;
using UnityEngine;

namespace LevelEditor
{
    public class ObjectPreview : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("Fixed Variables")] public GameObject pointsData;

        [ShowAssetPreview(128, 128)] public GameObject Enemy1;
        //[ShowAssetPreview(256, 256)] public GameObject Enemy2;
#endif
    }
}
