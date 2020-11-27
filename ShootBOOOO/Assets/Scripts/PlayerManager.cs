using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pView;
    PlayerController playerController;
    int team = 0;
    
    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pView.IsMine)
        {
            team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            CreateController();
        }
    }

    private void Update()
    {
        if (!pView.IsMine)
            return;
               
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerController.enabled = !GameMenuManager.instance.isMenu;
        }
    }

    private void CreateController()
    {
        GameObject go;
        if (team == 1)
        {
            go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefs", "PlayerController"),
                                                    PlayerSpawner.instance.RandomRedPosition(), Quaternion.identity);
            playerController = go.GetComponent<PlayerController>();
        }
        else if (team == 2)
        {
            go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefs", "PlayerController"),
                                                    PlayerSpawner.instance.RandomBluePosition(), Quaternion.identity);
            playerController = go.GetComponent<PlayerController>();
        }
    
    }
}
