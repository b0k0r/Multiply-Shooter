using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static Launcher instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of launcher manager found!");
            Destroy(this);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] private InputField roomNameInput;
    [SerializeField] private Text roomNameText;
    [SerializeField] private Text errorText;
    [SerializeField] private Transform roomlistContent;
    [SerializeField] private Transform userlistContent;
    [SerializeField] private Transform redTeamlistContent;
    [SerializeField] private Transform blueTeamlistContent;

    [SerializeField] private GameObject roomlistObject;
    [SerializeField] private GameObject userlistObject;
    [SerializeField] private GameObject startBattleButton;

    PhotonView pView;

    private void Start()
    {
        pView = GetComponent<PhotonView>();
        Debug.Log("Connecting to Master...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("Main");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("000");
        Debug.Log("Joined Lobby");
        Debug.Log("Welcome, " + PhotonNetwork.NickName);
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("Room");

        Player[] players = PhotonNetwork.PlayerList;
        
        if (PhotonNetwork.IsMasterClient)
            JoinTeam(1);
        else
            JoinTeam(2);


        startBattleButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startBattleButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "An error occurred while creating room: " + message;
        MenuManager.instance.OpenMenu("Error");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("Main");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform t in roomlistContent)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            GameObject go = Instantiate(roomlistObject, roomlistContent);
            go.GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject go = Instantiate(userlistObject, userlistContent);
        go.GetComponent<UserListItem>().Setup(newPlayer);
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            Debug.Log("Room name cannot be empty!");
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
        MenuManager.instance.OpenMenu("Loading");
    }

    public void JoinRoom(RoomInfo _roomInfo)
    {
        PhotonNetwork.JoinRoom(_roomInfo.Name);
        MenuManager.instance.OpenMenu("Loading");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }

    public void StartBattle()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void JoinTeam(int _team)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = _team;
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Team", _team);

            PhotonNetwork.SetPlayerCustomProperties(hashtable);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        JoiningTeam();
    }

    public void JoiningTeam()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform t in redTeamlistContent)
        {
            if (t.gameObject.GetComponent<UserListItem>())
                Destroy(t.gameObject);
        }

        foreach (Transform t in blueTeamlistContent)
        {
            if (t.gameObject.GetComponent<UserListItem>())
                Destroy(t.gameObject);
        }

        foreach (Player p in players)
        {
            if (p.CustomProperties.ContainsKey("Team"))
            {
                if ((int)p.CustomProperties["Team"] == 1)
                {
                    GameObject go = Instantiate(userlistObject, redTeamlistContent);
                    go.GetComponent<UserListItem>().Setup(p);
                }
                else if ((int)p.CustomProperties["Team"] == 2)
                {
                    GameObject go = Instantiate(userlistObject, blueTeamlistContent);
                    go.GetComponent<UserListItem>().Setup(p);
                }
                else if ((int)p.CustomProperties["Team"] == 0)
                {
                    GameObject go = Instantiate(userlistObject, userlistContent);
                    go.GetComponent<UserListItem>().Setup(p);
                }
            }
        }
    }
}
