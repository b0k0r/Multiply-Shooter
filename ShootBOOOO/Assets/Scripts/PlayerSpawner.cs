using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    #region Singleton
    public static PlayerSpawner instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of PlayerSpawner found!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] private Transform redSpawnPointsParent;
    [SerializeField] private Transform blueSpawnPointsParent;
    [SerializeField] private Transform[] redSpawnPoints;
    [SerializeField] private Transform[] blueSpawnPoints;
    
    private void Start()
    {
        redSpawnPoints = redSpawnPointsParent.GetComponentsInChildren<Transform>();
        blueSpawnPoints = blueSpawnPointsParent.GetComponentsInChildren<Transform>();
    }

    public Vector3 RandomRedPosition()
    {
        int index = Random.Range(0, redSpawnPoints.Length);

        return redSpawnPoints[index].position;
    }

    public Vector3 RandomBluePosition()
    {
        int index = Random.Range(0, blueSpawnPoints.Length);

        return blueSpawnPoints[index].position;
    }
}
