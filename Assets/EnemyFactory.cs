using System;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [Serializable]

    public class EnemyPrefabEntry
    {
        public EnemyType type;
        public GameObject prefab;
    }
    
    [SerializeField] private EnemyPrefabEntry[] prefabEntries;
    
    public GameObject CreateEnemy(EnemyType  type, Vector3 pos, Quaternion rot)
    {
        GameObject prefab = GetPrefab(type);
        return Instantiate(prefab, pos, rot);
    }

    public GameObject GetPrefab(EnemyType type)
    {
        for (int i = 0; i < prefabEntries.Length; i++)
        {
            if (prefabEntries[i].type == type)
            {
                return prefabEntries[i].prefab;
            }
        }
        return null;
    }
}
