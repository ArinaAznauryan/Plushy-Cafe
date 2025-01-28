using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot slot;

    public void Initialize(Slot slot) {
        this.slot = slot;
    }

    void Update() {
        
    }
}