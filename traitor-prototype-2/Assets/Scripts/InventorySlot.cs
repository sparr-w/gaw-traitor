using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Image icon;
    public Text label;
    public GameObject options;
    public int Index;

    private Item item;
    private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 defaultPosition;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;
        canvas = transform.parent.parent.parent.GetComponent<Canvas>(); // the ui is static
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public Item Item {
        get {return item;}
    }

    public void AddItem(Item newItem, int index) {
        item = newItem;
        this.Index = index;

        label.text = item.Name;

        //icon.sprite = item.icon;
        //icon.enabled = true;
    }

    public void ClearSlot() {
        item = null;

        label.text = "Empty";
        
        //icon.sprite = null;
        //icon.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        //Debug.Log("OnPointerDown");
        // right mouse button for options: use item, drop item
        if (item != null) {
            if (eventData.button == PointerEventData.InputButton.Right)
                options.SetActive(true);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // only drag with left mouse button
        if (item != null) {
            // only drag items that are equippable, only want items on the hotbar that can go in hand
            if (item.Equippable) {
                if (eventData.button == PointerEventData.InputButton.Left && !options.activeSelf)
                    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // only drag with left mouse button
        if (item != null) {
            if (item.Equippable) {
                if (eventData.button == PointerEventData.InputButton.Left && !options.activeSelf) {
                    //Debug.Log("OnBeginDrag");
                    canvasGroup.alpha = .6f;
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        // only drag with left mouse button
        if (item != null) {
            if (item.Equippable) {
                if (eventData.button == PointerEventData.InputButton.Left && !options.activeSelf) {
                    //Debug.Log("OnEndDrag");
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                    rectTransform.anchoredPosition = defaultPosition;
                }
            }
        }
    }

    public void HideOptions() {
        options.SetActive(false);
    }
}
