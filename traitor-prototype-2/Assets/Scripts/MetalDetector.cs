using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetector : MonoBehaviour {
    public Transform Guard;
    public Material Dim;
    public Material Bright;
    public bool Alert = false;

    private Transform lightBar;
    private float timer = 0f;
    private bool lightOn = false;

    private void Start() {
        // default values
        lightBar = transform.Find("Light");
        Alert = false;
        timer = 0f;
        lightOn = false;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 6) {
            // player's layer is 6
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.Inventory.HasMetalItem()) {
                // if the player has a guard disguise and the guard watching the detector isn't alert and not dead, the player can pass through
                if (player.Disguise.Current == Disguises.Guard) {
                    if (Guard != null) { // make sure there is a guard assigned to the detector
                        if (Guard.GetComponent<AIBehaviour>().AIState == AIStates.Relaxed) {
                            // relaxed; player passes through, guard disables alarm but becomes alert
                            Guard.GetComponent<AIBehaviour>().AIState = AIStates.Alert;
                        } else if (Guard.GetComponent<AIBehaviour>().AIState == AIStates.Alert) {
                            // guard already suspicious, player caught by guard
                            Alert = true;
                            Guard.GetComponent<AIBehaviour>().AIState = AIStates.Detected;
                            GameObject.Find("GameHandler").GetComponent<Game>().Fail("you walked through a metal detector next to a guard that was already suspicious of you", Guard.transform);
                        } else {
                            // guard is dead, player caught by alarm system tripped
                            Alert = true;
                            GameObject.Find("GameHandler").GetComponent<Game>().Fail("there was no guard to turn off the metal detector after you walked through");
                        }
                    }
                    else {
                        // no guard assigned to metal detector, alarm system tripped
                        Alert = true;
                        GameObject.Find("GameHandler").GetComponent<Game>().Fail("there was no guard to turn off the metal detector after you walked through");
                    }
                } else {
                    // alarm system tripped, no disguise
                    Alert = true;
                    GameObject.Find("GameHandler").GetComponent<Game>().Fail("you walked through a metal detector with a weapon, try finding a disguise?");
                }
            }
        }
    }

    private void Update() {
        // when alert, flash light and sound alarm
        if (Alert) {
            timer += Time.deltaTime;
            if (timer > 0.5f) {
                if (lightOn) {
                    lightOn = false;
                    lightBar.GetComponent<MeshRenderer>().material = Dim;
                    foreach(Transform light in lightBar)
                        light.GetComponent<Light>().enabled = false;
                } else {
                    lightOn = true;
                    lightBar.GetComponent<MeshRenderer>().material = Bright;
                    foreach(Transform light in lightBar)
                        light.GetComponent<Light>().enabled = true;
                }
                timer = 0f;
            }
        }
    }
}
