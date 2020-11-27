using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameTablo : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static GameTablo instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of game tablo found!");
            Destroy(this);
            return;
        }
        instance = this;
    }
    #endregion

    public static int blueScore = 0;
    public static int redScore = 0;

    [SerializeField] private float startTimer = 5f;
    [SerializeField] private int totalScore = 10;

    public static readonly byte RestartGameEventCode = 1;
    public static bool matchEnded = false;

    private void Start()
    {
        ResetScore();
        GameMenuManager.instance.CursorVisible(false);
        GameMenuManager.instance.StartingMatchMessage(startTimer, "Battle begin!");
    }

    private void Update()
    {
        SetScore();

        if (!matchEnded)
        {
            if (totalScore <= redScore)
            {
                ShowTotalScore();
                GameMenuManager.instance.TypingDisplayMessage("Red team wins");

            }
            if (totalScore <= blueScore)
            {
                ShowTotalScore();
                GameMenuManager.instance.TypingDisplayMessage("Blue team wins");
            }
        }
    }

    private void SetScore()
    {
        GameMenuManager.instance.SetScore(redScore.ToString(), blueScore.ToString());
    }

    private void ShowTotalScore()
    {
        matchEnded = true;
        GameMenuManager.instance.ShowBackground(true);
        GameMenuManager.instance.RespawnerTimer("", "");
        GameMenuManager.instance.ShowEndedButtons(true);
        GameMenuManager.instance.CursorVisible(true);
    }

    private void ResetScore()
    {
        matchEnded = false;
        redScore = 0;
        blueScore = 0;
    }

    /*
    public void RestartingGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
               StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        redScore = 0;
        blueScore = 0;
        yield return new WaitForSeconds(2);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        ExitGames.Client.Photon.SendOptions sendOptions = new ExitGames.Client.Photon.SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(RestartGameEventCode, null, raiseEventOptions, sendOptions);
    }
    */

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //ReloadLevel();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //ReloadLevel();
    }

    public void ReloadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Reloading Level");
            PhotonNetwork.LoadLevel(1);
        }
    }

}
