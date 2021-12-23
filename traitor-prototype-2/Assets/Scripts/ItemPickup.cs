using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {
    public Item item;
    public int Amount;
    public GameObject Indicator;

    private void Start() {
    }

    private void Update() {
        if (Indicator != null) {
            Indicator.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (this.enabled) {
            if (other.gameObject.layer == 6) { // player's layer is 6
                if (Indicator != null)
                    Indicator.SetActive(true);
                //
                PlayerController player = other.GetComponent<PlayerController>();
                // a cool popup of some sorts telling the user to press F to pickup
                if (Input.GetKey(KeyCode.F)) {
                    // add item to inventory
                    player.Inventory.AddItem(item, Amount);
                    player.PickupNotification.ItemPickedUp(item);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (this.enabled) {
            if (other.gameObject.layer == 6) { // player's layer is 6
                if (Indicator != null)
                    Indicator.SetActive(false);
                //
            }
        }
    }
}
