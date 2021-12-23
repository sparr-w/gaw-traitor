using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour {
    public float DisplayTime = 3f;
    public float timer;

    public void ItemPickedUp(Item item) {
        transform.GetComponentInChildren<Text>().text = "Picked up '" + item.Name + "'";
        timer = 0f;
        //
        gameObject.SetActive(true);
    }

    private void Update() {
        if (timer < DisplayTime)
            timer += Time.deltaTime;
        else
            gameObject.SetActive(false);
    }
}
