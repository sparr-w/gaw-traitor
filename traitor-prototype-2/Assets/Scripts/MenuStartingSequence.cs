using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuStartingSequence : MonoBehaviour {
    public float[] Intervals;
    public Image Cover;

    private MenusHandler menusHandler;
    private List<GameObject> parts;
    private int currentPart;
    private float timer;

    private void Start() {
        menusHandler = transform.parent.GetComponent<MenusHandler>(); // static hierarchy
        // gather parts of starting sequence
        parts = new List<GameObject>();
        foreach (Transform child in transform)
            parts.Add(child.gameObject);
        parts.RemoveAt(parts.Count - 1); // remove cover from list
        currentPart = 0;

        timer = 0;
    }

    private void Update() {
        if (menusHandler.Current == Menus.Loading) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                // skip loading screens
                if (currentPart + 1 < parts.Count) {
                    currentPart++;
                    timer = 0;
                }
            }
            // only carry out sequence if the game is currently loading
            timer += Time.deltaTime;
            // switch part
            if (timer > Intervals[currentPart]) {
                if (currentPart == 4) { // part 5
                    foreach (Transform child in parts[currentPart].transform)
                        child.gameObject.SetActive(true);
                    // wait for input to proceed
                    if (Input.anyKey) {
                        currentPart++;
                        timer = 0;
                    }
                } else {
                    currentPart++;
                    timer = 0;
                }
            }
            // start the game at the end of the sequence
            if (currentPart + 1 > parts.Count)
                SceneManager.LoadScene("Demo");
            else {
                // display correct part and hide others
                for (int i = 0; i < parts.Count; i++)
                    if (parts[i].activeSelf) parts[i].SetActive(false);
                parts[currentPart].SetActive(true);
            }
            //
            Cover.color = new Color(Cover.color.r, Cover.color.g, Cover.color.b, 1 - timer);
        }
    }
}
