using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static RoomManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of room manager found!");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }
    #endregion


    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
    {
        if (_scene.buildIndex == 1) 
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}