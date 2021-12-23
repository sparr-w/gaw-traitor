using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotbarSlot : MonoBehaviour, IDropHandler {
    public Image icon;
    public Text label;
    public Text number;
    public int InvSlot = -1;

    private Item item;

    public Item Item {
        get {return item;}
    }

    public void AddItem(Item newItem, int slot) {
        item = newItem;
        InvSlot = slot;

        label.text = item.Name;

        // icon.sprite = item.icon;
        // icon.enabled = true;
    }

    public void ClearSlot() {
        item = null;
        InvSlot = -1;

        label.text = "Empty";

        // icon.sprite = null;
        // icon.enabled = false;
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null) {
            Item i = eventData.pointerDrag.GetComponent<InventorySlot>().Item;
            int s = eventData.pointerDrag.GetComponent<InventorySlot>().Index;
            //
            if (i != null)
                AddItem(i, s);
            else
                ClearSlot();
        }
    }
}
