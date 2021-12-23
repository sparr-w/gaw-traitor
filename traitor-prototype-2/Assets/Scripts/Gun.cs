using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public LayerMask layersCanHit;
    public float Damage = 10f;

    private RaycastHit hit;
    private Ray ray;

    private void Update() {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000f, Color.cyan);
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layersCanHit, QueryTriggerInteraction.Ignore)) {
                if (hit.collider.gameObject.layer == 10) {
                    AILimb target = hit.collider.GetComponent<AILimb>();
                    //Debug.Log(target.Host.gameObject.name + "'s " + target.gameObject.name + " got sclarted");
                    // don't attempt to damage dead AI
                    if (target.Host.Health > 0) {
                        target.TakeDamage(Damage);
                        Debug.Log(target.Host.gameObject.name + " health is now " + target.Host.Health);
                    }
                }
            }
        }
    }
}
