using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameStates {
    Active,
    Paused,
    Victory,
    Failed
}

public class Game : MonoBehaviour {
    public PlayerController Player;
    public GameStates GameState;
    public GameObject UI;

    private List<GameObject> lastUI;
    private CursorLockMode lastLock;
    private bool lastVisible;

    private void Start() {
        CursorLocked(true);
        GameState = GameStates.Active;
    }

    public void CursorLocked(bool state) {
        if (state) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Victory() {
        foreach(Transform child in UI.transform)
            child.gameObject.SetActive(false);
        UI.transform.Find("Victory").gameObject.SetActive(true);
        // unlock mouse if currently locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //
        Time.timeScale = 0;
        GameState = GameStates.Victory;
    }

    private void PauseGame() {
        lastUI = new List<GameObject>();
        foreach (Transform child in UI.transform) {
            if (child.gameObject.activeSelf) {
                lastUI.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
        // store last mouse state
        lastLock = Cursor.lockState;
        lastVisible = Cursor.visible;
        // unlock mouse if currently locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //
        UI.transform.Find("Paused").gameObject.SetActive(true);
        //
        Time.timeScale = 0;
        GameState = GameStates.Paused;
    }

    public void ResumeGame() {
        UI.transform.Find("Paused").gameObject.SetActive(false);
        //
        foreach (GameObject ui in lastUI)
            ui.SetActive(true);
        // restore last mouse state
        Cursor.lockState = lastLock;
        Cursor.visible = lastVisible;
        //
        Time.timeScale = 1;
        GameState = GameStates.Active;
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main-Menus");
    }

    public void RestartScene() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Fail(string condition, Transform caughtBy = null) {
        // show appropriate UI
        foreach(Transform child in UI.transform)
            child.gameObject.SetActive(false);
        UI.transform.Find("GameOver").gameObject.SetActive(true);
        UI.transform.Find("GameOver").Find("Reason").GetComponentInChildren<Text>().text = condition;
        // unlock mouse if currently locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //
        if (caughtBy != null) {
            Player.transform.LookAt(caughtBy);
        }
        Time.timeScale = 0;
        GameState = GameStates.Failed;
        Debug.Log("Game Over: " + condition);
    }

    private void Update() {
        switch (GameState) {
            case GameStates.Active:
                if (Input.GetKeyDown(KeyCode.Escape))
                    PauseGame();
                break;
            case GameStates.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                    ResumeGame();
                break;
            case GameStates.Victory:
                break;
            case GameStates.Failed:
                break;
        }
    }
}
