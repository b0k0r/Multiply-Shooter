using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of menu manager found!");
            Destroy(this);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] private Menu[] menuArray;

    public void OpenMenu(string _menuName)
    {
        for (int i = 0; i < menuArray.Length; i++) 
        {
            if (menuArray[i].menuName == _menuName)
            {
                menuArray[i].Open();
            }
            else if (menuArray[i].isOpen)
            {
                CloseMenu(menuArray[i]);
            }
        }

    }

    public void OpenMenu(Menu _menu)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            if (menuArray[i].isOpen)
            {
                CloseMenu(menuArray[i]);
            }
        }

        _menu.Open();
    }

    public void CloseMenu(Menu _menu)
    {
        _menu.Close();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
