using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour {
    public Transform ItemsParent;
    public int Equipped = 0; // 0 means nothing is equipped

    private HotbarSlot[] slots;

    private void Start() {
        slots = ItemsParent.GetComponentsInChildren<HotbarSlot>();
        for (int i = 1; i <= slots.Length; i++)
            slots[i - 1].number.text = "" + i;
    }

    public void ClearedInventorySlot(int slot) {
        foreach (HotbarSlot s in slots) {
            if (s.InvSlot == slot)
                s.ClearSlot();
        }
    }

    public Item GetItemInSlot(int slot) {
        if (slot - 1 < 0) return null;
        return slots[slot - 1].Item;
    }

    /*
    public int slotEquipped;
    private Transform slotsContainer;

    private void Start() {
        // collate all components
        slotsContainer = transform.Find("Slots");
    }

    public void UpdateSlot(Item item, int slot) {
        Transform s = slotsContainer.GetChild(slot - 1);
        //
        s.Find("Name").GetComponent<Text>().text = "" + item.Name;
    }

    public void HighlightSlot(int slot) {
        // disable highlight from all
        for (int i = 0; i < slotsContainer.childCount; i++) {
            Transform j = slotsContainer.GetChild(i).Find("Background");
            j.GetComponent<Outline>().enabled = false;
        }
        // enable highlight on all
        Transform k = slotsContainer.GetChild(slot - 1).Find("Background");
        k.GetComponent<Outline>().enabled = true;
    }
    */
}
