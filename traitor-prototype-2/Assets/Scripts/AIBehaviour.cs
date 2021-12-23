using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AITypes {
    Citizen,
    Guard,
    Target
}

public enum AIStates {
    Relaxed,
    Alert,
    Detected,
    Dead
}

public class AIBehaviour : MonoBehaviour {
    public AITypes AIType;
    public AIStates AIState;
    public float Health = 100f;

    private Rigidbody rb;
    private BoxCollider coll;
    private Transform body;

    private SphereCollider sphereCollider; // collider for player detection and looting dead AI
    private ItemPickup loot;
    private Transform indicators;
    private bool canSeePlayer;

    private void Start() {
        AIState = AIStates.Relaxed;
        //
        rb = GetComponent<Rigidbody>();
        coll = transform.Find("Collider").GetComponent<BoxCollider>();
        body = transform.Find("Body");
        //
        indicators = transform.Find("Indicators");
        canSeePlayer = false;
        //
        sphereCollider = GetComponent<SphereCollider>();

        if (GetComponent<ItemPickup>() != null) { // check whether the body has loot before accessing it
            loot = GetComponent<ItemPickup>();
            loot.enabled = false;
        }
    }

    public float TakeDamage(float amount) {
        // take damage and return remaining health
        if (amount >= Health) { 
            Health = 0f;
            Die();
        } 
        else {
            if (AIType == AITypes.Guard) {
                AIState = AIStates.Detected;
                GameObject.Find("GameHandler").GetComponent<Game>().Fail("you shot a guard but he didn't die and called for help, try shooting them in the head", transform);
            }
            return Health -= amount;
        }
        return Health;
    }

    private void Die() {
        SwitchState(AIStates.Dead);
        sphereCollider.radius = 6.7f;
        //
        if (loot != null)
            loot.enabled = true;
    }

    private void SwitchState(AIStates newState) {
        switch (newState) {
            case AIStates.Alert:
                AIState = AIStates.Alert;
                break;
            case AIStates.Relaxed:
                AIState = AIStates.Relaxed;
                break;
            case AIStates.Dead:
                body.localEulerAngles = new Vector3(90f, 0f, 0f);
                transform.rotation = GameObject.FindWithTag("Player").transform.rotation;
                coll.enabled = false;
                transform.position -= Vector3.up * 2.22f;
                //
                if (AIType == AITypes.Target)
                    GameObject.Find("GameHandler").GetComponent<Game>().Victory();
                else
                    AIState = AIStates.Dead;
                break;
        }
    }

    private void Update() {
        if (AIState != AIStates.Dead) {
            if (indicators != null) {
                indicators.transform.LookAt(GameObject.FindWithTag("Player").transform);
                // some AI may not have indicators, ensure that they do
                if (canSeePlayer) {
                    // hide all other indicators first
                    foreach (Transform child in indicators)
                        child.gameObject.SetActive(false);
                    // show appropriate indicators
                    switch (AIState) {
                        case AIStates.Alert:
                            indicators.Find("Alert").gameObject.SetActive(true);
                            break;
                        case AIStates.Detected:
                            indicators.Find("Detected").gameObject.SetActive(true);
                            break;
                    }
                } else {
                    // hide all indicators if the AI cannot see the player
                    foreach (Transform child in indicators)
                        child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        // detect whether the playing is acting suspicious
        if (other.gameObject.layer == 6) {
            // player's layer is 6
            PlayerController player = other.GetComponent<PlayerController>();
            if (AIState != AIStates.Dead) {
                if (AIType == AITypes.Guard) { // only applies for guards rn with this statement
                    RaycastHit hit;
                    LayerMask layers = ~(LayerMask.GetMask(LayerMask.LayerToName(7), LayerMask.LayerToName(8), LayerMask.LayerToName(9), LayerMask.LayerToName(10)));
                    if (Physics.Linecast(transform.position, player.transform.position, out hit, layers, QueryTriggerInteraction.Ignore)) {
                        if (hit.collider.gameObject.layer == 6) { // nothing obstructs the line of sight to player
                            Color debugraycolor = Color.red;
                            if (Vector3.Dot((player.transform.position - transform.position), transform.forward) > 0) {
                                canSeePlayer = true;
                                debugraycolor = Color.green; // player infront of AI
                                if (player.Inventory.Hotbar.GetItemInSlot(player.Inventory.Hotbar.Equipped) != null) {
                                    if (player.Inventory.Hotbar.GetItemInSlot(player.Inventory.Hotbar.Equipped).ItemType == ItemTypes.Weapon) {
                                        // AI sees player with a weapon // their reaction
                                        if (player.Disguise.Current == Disguises.Guard) {
                                            if (AIState == AIStates.Alert) {
                                                // AI is already suspicious of player, pulling a weapon will alert them
                                                AIState = AIStates.Detected;
                                                GameObject.Find("GameHandler").GetComponent<Game>().Fail("you were caught walking around with your weapon by a suspicious guard", transform);
                                            }
                                        }
                                        else {
                                            // if the player is not disguised, pulling a weapon will alert them
                                            AIState = AIStates.Detected;
                                            GameObject.Find("GameHandler").GetComponent<Game>().Fail("you were caught with a weapon out, try finding a guard disguise", transform);
                                        }
                                            
                                    }
                                }
                            } else
                                canSeePlayer = false;
                            Debug.DrawLine(transform.position, player.transform.position, debugraycolor);
                        }
                    } else
                        canSeePlayer = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 6) 
            canSeePlayer = false;
    }
}
