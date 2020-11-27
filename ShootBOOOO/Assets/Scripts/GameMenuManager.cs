using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameMenuManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static GameMenuManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of game menu manager found!");
            Destroy(this);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] private GameObject gameMenuGo;
    [SerializeField] private Crosshair crosshair;

    [SerializeField] private Text healthText;
    [SerializeField] private Image healthBar;

    [SerializeField] private GameObject pullString;
    [SerializeField] private Image pullStringBar;

    [SerializeField] private GameObject reload;
    [SerializeField] private Image reloadBar;
    [SerializeField] private Text reloadText;

    [SerializeField] private Text messageText;

    [SerializeField] private WeaponItemUI[] weaponItems;

    [SerializeField] private Color[] colors;

    [SerializeField] private GameObject bgr;
    [SerializeField] private Text respawnerTimerText;
    [SerializeField] private Text respawnerTimerLabel;

    [SerializeField] private Text blueScoreText;
    [SerializeField] private Text redScoreText;
    [SerializeField] private Text grossMessageText;

    [SerializeField] private Text eventItemText;

    [SerializeField] private GameObject endedBtns;
    public bool isMenu = false;

    WeaponItemUI currentWeaponItem;

    private void Start()
    {
        gameMenuGo.SetActive(false);
        pullString.SetActive(false);
        reload.SetActive(false);
        messageText.gameObject.SetActive(false);
        bgr.SetActive(false);
        ShowEndedButtons(false);


        isMenu = false;
        CursorVisible(isMenu);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowGameMenu();
        }
    }

    public void ShowGameMenu()
    {
        isMenu = !isMenu;
        
        gameMenuGo.SetActive(!gameMenuGo.activeSelf);
        crosshair.enabled = !gameMenuGo.activeSelf;


        CursorVisible(isMenu);
    }

    public void CursorVisible(bool _visible)
    {
        Cursor.visible = _visible;
    }

    public void CurrentHealth(float _health)
    {
        if (_health < 0)
            _health = 0;

        healthText.text = _health.ToString("000");
        healthBar.fillAmount = _health / 100;
    }

    public void CurrentHealth(float _health, float _maxHealth)
    {
        if (_health < 0)
            _health = 0;

        if (_health > 0.2f * _maxHealth)
            healthText.color = colors[0];
        else
            healthText.color = colors[1];

        healthText.text = _health.ToString("000");
        healthBar.fillAmount = _health / 100;
    }
    public void ShowPullBowString(bool _show)
    {
        pullString.SetActive(_show);
    }

    public void CurrentPullBowString(float _charge, float _maxCharge)
    {
        pullStringBar.fillAmount = _charge / _maxCharge;
    }

    public void ShowReload(bool _show)
    {
        reload.SetActive(_show);
    }

    public void CurrentReload(float _cur, float _max)
    {
        reloadText.text = "Reloading";
        reloadBar.fillAmount = _cur / _max;
    }

    public void SelectWeaponItem(int _index)
    {
        for(int i = 0; i < weaponItems.Length; i++)
        {
            if (i == _index)
            {
                weaponItems[i].IsSelect(true);
                currentWeaponItem = weaponItems[i];
            }
            else
            {
                weaponItems[i].IsSelect(false);
                weaponItems[i].AmmoText("");
            }
        }
    }

    public void CurrentAmmo(PlayerItem _playerItem)
    {
        if (currentWeaponItem != null)
        {
            string ammoString = _playerItem.currentAmmo.ToString("00") +
                                "/" + _playerItem.maxAmmo.ToString("000");

            bool isWarning = false;

            if (_playerItem.maxAmmo <= _playerItem.ammoPerMagazine)
                isWarning = true;

            currentWeaponItem.AmmoText(ammoString, isWarning);
        }
    }

    public void ActivateItem(int _index, bool _access)
    {
        for (int i = 0; i < weaponItems.Length; i++)
        {
            if (_index == i)
            {
                weaponItems[i].ActivateItem(_access);
            }
        }
    }

    public void Message(string _message)
    {
        StartCoroutine(ShowMessage(_message));
    }

    private IEnumerator ShowMessage(string _message)
    {
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        messageText.gameObject.SetActive(false);
    }

    public void ShowBackground(bool _show)
    {
        bgr.SetActive(_show);
    }

    public void RespawnerTimer(string _mes, string _time)
    {
        respawnerTimerLabel.text = _mes;
        respawnerTimerText.text = _time;
    }

    public void SetScore(string _scoreRed, string _scoreBlue)
    {
        redScoreText.text = _scoreRed;
        blueScoreText.text = _scoreBlue;
    }

    public void StartingMatchMessage(float _timer, string _message)
    {
        StartCoroutine(StartMatchMessage(_timer, _message));
    }

    IEnumerator StartMatchMessage(float _timer, string _message)
    {
        float r = _timer;
        grossMessageText.text = "";
        GameTablo.matchEnded = true;
        while (r >= 0)
        {
            yield return new WaitForSeconds(1);

            r -= 1.0f;

            if (r >= 1)
                grossMessageText.text = r.ToString("00");
            else
            {
                grossMessageText.text = _message;
                GameTablo.matchEnded = false;
            }

        }

        grossMessageText.text = "";
    }

    public void TypingDisplayMessage(string _message)
    {
        StartCoroutine(TypeDisplayMessage(_message));
    }

    IEnumerator TypeDisplayMessage(string _message)
    {
        grossMessageText.text = "";

        yield return new WaitForSeconds(1.0f);

        foreach (char letter in _message.ToCharArray())
        {
            grossMessageText.text += letter;
            yield return null;
        }
    }

    public void ShowEndedButtons(bool _show)
    {
        endedBtns.SetActive(_show);
    }

    public void EventItemText(string _text)
    {
        eventItemText.text = _text;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
