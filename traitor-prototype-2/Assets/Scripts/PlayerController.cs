using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates {
    Standing,
    Crouching
}

public class PlayerController : MonoBehaviour {
    public PlayerStates State = PlayerStates.Standing;
    [Header("Components")]
    public Transform Camera;
    public Transform Body;
    public Transform Hand;
    public PickupUI PickupNotification;
    [Header("Variables")]
    public float Speed = 10.0f;
    public float LookSensitivity = 3.0f;

    private Rigidbody rb;
    private BoxCollider bcoll;
    private Inventory inventory;
    private Transform legL, legR, armR;

    private Vector3 movement;
    private float lookX, lookY;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        bcoll = GetComponent<BoxCollider>();
        inventory = GetComponent<Inventory>();
        //
        legL = Body.Find("Left Leg");
        legR = Body.Find("Right Leg");
        armR = Body.Find("Right Arm");
        //
        State = PlayerStates.Standing;
    }

    #region Setters and Getters
    public Inventory Inventory {
        set {inventory = value;} get {return inventory;}
    }

    public Disguise Disguise {
        get {return GetComponent<Disguise>();}
    }
    #endregion

    private void Move(Vector3 direction) {
        direction = rb.rotation * direction;
        rb.MovePosition(transform.position + (direction * Speed * Time.deltaTime));
    }

    private void Look(Vector2 direction) {
        transform.Rotate(0f, direction.x * LookSensitivity, 0f);
        Camera.localEulerAngles = new Vector3(-direction.y, Camera.localEulerAngles.y, Camera.localEulerAngles.z);
    }

    private void SwitchStance(PlayerStates state) {
        switch (state) {
            case PlayerStates.Standing:
                // adjust collider appropriately
                bcoll.size = new Vector3(bcoll.size.x, 4f, bcoll.size.z);
                bcoll.center = new Vector3(bcoll.center.x, -0.5f, bcoll.center.z);
                // move player up
                transform.position += new Vector3(0f, 1.5f, 0f);
                // alter limb location
                legL.localPosition = new Vector3(legL.localPosition.x, -1.5f, 0f);
                legR.localPosition = new Vector3(legR.localPosition.x, -1.5f, 0f);
                break;
            case PlayerStates.Crouching:
                bcoll.size = new Vector3(bcoll.size.x, 2.5f, bcoll.size.z);
                bcoll.center = new Vector3(bcoll.center.x, 0.25f, bcoll.center.z);
                //
                transform.position -= new Vector3(0f, 1.5f, 0f);
                //
                legL.localPosition = new Vector3(legL.localPosition.x, 0f, 1f);
                legR.localPosition = new Vector3(legR.localPosition.x, 0f, 1f);
                break;
        }
        // switch state
        State = state;
    }

    private void Equip(int slot) {
        // safe to equip // get the item from that slot and equip it
        Item itemToEquip = inventory.Hotbar.GetItemInSlot(slot);
        //
        armR.gameObject.SetActive(false);
        Hand.gameObject.SetActive(true);
        //
        GameObject itemEquipped = Instantiate(itemToEquip.HandObject, Hand);
        itemEquipped.transform.localPosition = itemToEquip.HandOffset;
        //
        inventory.Hotbar.Equipped = slot;
    }

    private void Dequip() {
        // delete all objects in hand besides the arm // arm is always index 0
        for (int i = 1; i < Hand.childCount; i++)
            Destroy(Hand.GetChild(i).gameObject);
        //
        armR.gameObject.SetActive(true);
        Hand.gameObject.SetActive(false);
        //
        inventory.Hotbar.Equipped = 0;
    }

    private void Update() {
        if (GameObject.Find("GameHandler").GetComponent<Game>().GameState == GameStates.Active) { // only accept input if the game is active
            // wasd and arrow key movement
            movement = new Vector3(0f, 0f, 0f);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                movement.z = 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                movement.z = -1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movement.x = -.6f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movement.x = .6f;
            // look with mouse, turn left and right // don't move camera if the player is in inventory
            if (!Inventory.UI.gameObject.activeSelf) {
                lookY += Input.GetAxis("Mouse Y");
                lookY = Mathf.Clamp(lookY, -40f, 40f);
                lookX = Input.GetAxis("Mouse X");
            } else
                lookX = 0;
            // switch states: standing, crouching
            if (Input.GetKeyDown(KeyCode.C)) {
                switch (State) {
                    case PlayerStates.Standing:
                        SwitchStance(PlayerStates.Crouching);
                        break;
                    case PlayerStates.Crouching:
                        SwitchStance(PlayerStates.Standing);
                        break;
                }
            }
            //
            // toggle inventory UI
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
                Inventory.ToggleUI();
            //
            // equip items from hotbar
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (inventory.Hotbar.GetItemInSlot(1) != null) { // not an empty slot
                    if (inventory.Hotbar.Equipped == 1 || !inventory.Hotbar.GetItemInSlot(1).Equippable)  // item already equipped // item not equippable // empty hands
                        Dequip();
                    else if (inventory.Hotbar.GetItemInSlot(1).Equippable) // otherwise if the item is equippable, equip it
                        Equip(1);
                } else // empty slot, empty hands
                    Dequip();
            }
            // 2
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                if (inventory.Hotbar.GetItemInSlot(2) != null) { // not an empty slot
                    if (inventory.Hotbar.Equipped == 2 || !inventory.Hotbar.GetItemInSlot(2).Equippable)  // item already equipped // item not equippable // empty hands
                        Dequip();
                    else if (inventory.Hotbar.GetItemInSlot(2).Equippable) // otherwise if the item is equippable, equip it
                        Equip(2);
                } else // empty slot, empty hands
                    Dequip();
            }
            // 3
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (inventory.Hotbar.GetItemInSlot(3) != null) { // not an empty slot
                    if (inventory.Hotbar.Equipped == 3 || !inventory.Hotbar.GetItemInSlot(3).Equippable)  // item already equipped // item not equippable // empty hands
                        Dequip();
                    else if (inventory.Hotbar.GetItemInSlot(3).Equippable) // otherwise if the item is equippable, equip it
                        Equip(3);
                } else // empty slot, empty hands
                    Dequip();
            }
            // remove disguise
            if (GetComponent<Disguise>().Current != Disguises.None) {
                if (Input.GetKeyDown(KeyCode.Backspace)) {
                    GetComponent<Disguise>().DequipDisguise();
                }
            }
        }
    }
    
    private void FixedUpdate() {
        // move half the speed if inventory is open
        if (Inventory.UI.gameObject.activeSelf)
            Move(movement * 0.5f);
        else  
            Move(movement);
        //
        Look(new Vector2(lookX, lookY));
    }
}
