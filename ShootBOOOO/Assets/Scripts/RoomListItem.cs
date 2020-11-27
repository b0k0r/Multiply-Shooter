using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private Text nameText;
    public RoomInfo roomInfo;

    public void Setup(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        nameText.text = "(" +_roomInfo.PlayerCount + 
                        "/" + _roomInfo.MaxPlayers + 
                        ") : " + _roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(roomInfo);
    }
}
