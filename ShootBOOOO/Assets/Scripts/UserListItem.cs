using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UserListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text usernameText;
    public Player player;

    public void Setup(Player _player)
    {
        player = _player;
        usernameText.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer) 
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(this.gameObject);
    }
}
