using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public Item[] items = new Item[20];
    public InventoryUI UI;
    public Hotbar Hotbar;

    public bool AddItem(Item newItem, int amount) {
        // check whether the item already exists and increase count
        if (newItem.Stackable) {
            for (int i = 0; i < items.Length; i++) {
                if (items[i] != null) {
                    if (items[i] == newItem) { // item exists, increase count
                        Debug.Log("Increased '" + newItem.Name + "' count by " + amount + ".");
                        items[i].Count += amount;
                        return true;
                    }
                }
            }
        }
        // item doesn't exist, find the nearest empty slot
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) { // slot free, add item
                Debug.Log("Added '" + newItem.Name + "' to inventory.");
                items[i] = newItem;
                return true;
            }
        }
        // item doesn't already exist, no free slots
        return false;
    }

    /* removes every instance of the item
    public bool Remove(Item item, int amount = -1) {
        // remove specific amount of an item, amount default to -1, if no amount passed through, remove all
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == item) {
                // if amount is still -1 or if amount is larger or equal to count, remove all instances
                if (amount == -1 || amount >= items[i].Count)
                    items[i] = null;
                else // remove specified amount from count
                    items[i].Count -= amount;
                return true;
            }
        }
        // didn't find item, can't remove it
        return false;
    }
    */

    public bool RemoveAt(int slot, int amount = -1) {
        if (items[slot] != null) {
            if (amount == -1 || amount >= items[slot].Count) {
                items[slot] = null;
                Hotbar.ClearedInventorySlot(slot);
            }
            else
                items[slot].Count -= amount;
            return true;
        }
        // slot empty, can't remove it
        return false;
    }

    public int Count {
        // return number of items in inventory
        get {
            int counter = 0;
            for (int i = 0; i < items.Length; i++) {
                if (items[i] != null)
                    counter++;
            }
            return counter;
        }
    }

    public bool HasMetalItem() {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null) {
                if (items[i].isMetal)
                    return true;
            }
        }
        // couldn't find a metal item
        return false;
    }

    public Item ItemByName(string name) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null) {
                if (items[i].Name == name)
                    return items[i];
            }
        }
        // item doesn't exist in inventory
        return null;
    }

    public void ToggleUI() {
        // sets the UI active state to the opposite that it currently is, if true, set false and vice versa
        UI.gameObject.SetActive(!UI.gameObject.activeSelf);
        // if now active, refresh
        if (UI.gameObject.activeSelf) {
            GameObject.Find("GameHandler").GetComponent<Game>().CursorLocked(false);
            UI.RefreshInventory();
        } else
            GameObject.Find("GameHandler").GetComponent<Game>().CursorLocked(true);
    }
}
