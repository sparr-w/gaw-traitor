using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public PlayerController Player;
    public Transform ItemsParent;

    private InventorySlot[] slots;

    private void Awake() {
        slots = ItemsParent.GetComponentsInChildren<InventorySlot>();
    }
    
    public void RefreshInventory() {
        for (int i = 0; i < slots.Length; i++) {
            if (Player.Inventory.items[i] != null)
                slots[i].AddItem(Player.Inventory.items[i], i);
            else
                slots[i].ClearSlot();
        }
    }

    public void DropItem(InventorySlot slot) {
        // remove from inventory // clear slot and hide options
        Player.Inventory.RemoveAt(slot.Index);
        slot.ClearSlot();
        slot.HideOptions();
        // create object at feet so user can pick item up again
    }

    public void UseItem(InventorySlot slot) {
        // use item from inventory and then hide inventory
        switch(slot.Item.ItemType) {
            case ItemTypes.Disguise:
                // equip disguise and then get rid of the disguise, one time use
                Player.Disguise.EquipDisguise(slot.Item.Disguise);
                //
                Player.Inventory.RemoveAt(slot.Index);
                slot.ClearSlot();
                slot.HideOptions();
                break;
        }
    }
}
