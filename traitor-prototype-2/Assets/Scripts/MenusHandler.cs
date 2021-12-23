using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Menus {
    Main,
    Loading,
    Controls
}

public class MenusHandler : MonoBehaviour {
    public Menus Current;

    public GameObject MainMenu;
    public GameObject StartingSequence;
    public GameObject Controls;

    private void Start() {
    }

    private void SwitchMenu(Menus selected) {
        MainMenu.SetActive(false);
        StartingSequence.SetActive(false);
        Controls.SetActive(false);

        if (selected == Menus.Main) MainMenu.SetActive(true);
        else if (selected == Menus.Loading) StartingSequence.SetActive(true);
        else if (selected == Menus.Controls) Controls.SetActive(true);

        Current = selected;
    }

    public void StartGame() {
        SwitchMenu(Menus.Loading);
    }

    public void LoadControls() {
        SwitchMenu(Menus.Controls);
    }

    public void Quit() {
        Application.Quit();
    }

    private void Update() {
        switch (Current) {
            case Menus.Controls:
                if (Input.GetKey(KeyCode.Escape))
                    SwitchMenu(Menus.Main);
                break;
        }
    }
}
