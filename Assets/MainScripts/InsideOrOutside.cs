using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideOrOutside : MonoBehaviour
{
    bool inside = true;
    public bool once = true;

    public bool IsInside() {return inside;}

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            inside = true;
            once = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player")) {
            inside = false;
            once = true;
        }
    }
}
