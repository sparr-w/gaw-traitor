using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILimb : MonoBehaviour {
    public float DamageMultiplier = 1f;
    
    private AIBehaviour AI;

    public AIBehaviour Host {
        get {return AI;}
    }

    private void Awake() {
        // parent of limb is body, and parent of body is AI
        AI = transform.parent.parent.GetComponent<AIBehaviour>();
    }

    public float TakeDamage(float amount) {
        return AI.TakeDamage(amount * DamageMultiplier);
    }
}
