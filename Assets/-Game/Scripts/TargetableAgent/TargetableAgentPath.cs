using System;
using UnityEngine;

public class TargetableAgentPath : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Transform targetTransform;

    [NonSerialized] public Vector3 spawnPoint;
    [NonSerialized] public Vector3 targetPoint;

    private void Awake()
    {
        spawnPoint = spawnTransform.position;
        targetPoint = targetTransform.position;
    }
}
