using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Disguises {
    None,
    Guard
}

public class Disguise : MonoBehaviour {
    public Disguises Current = Disguises.None;
    public Text UI;

    public void EquipDisguise(Disguises newDisguise) {
        Current = newDisguise;
        UI.text = "[←]  Disguised as " + newDisguise.ToString();
    }

    public void DequipDisguise() {
        Current = Disguises.None;
        UI.text = "No disguise";
    }
}
