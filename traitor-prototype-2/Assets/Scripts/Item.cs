using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes {
    Weapon,
    Disguise
}

public class Item : MonoBehaviour {
    [Header("Definition")]
    public string Name;
    public ItemTypes ItemType;
    public Disguises Disguise;
    [Header("Components and Behaviour")]
    public GameObject Object; // used for pickups and environment
    public Vector3 HandOffset;
    public Sprite Icon;
    public bool isMetal;
    public bool Equippable;
    public GameObject HandObject; // used for items that are equippable, the gun for example // these objects contain behaviour scripts
    public bool Stackable;
    [Header("Variables")]
    public int Count;
}
