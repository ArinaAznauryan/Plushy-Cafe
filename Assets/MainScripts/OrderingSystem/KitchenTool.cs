using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitchenToolState {PRIMARY, CHANGED}

[System.Serializable]
public class KitchenTool : InventoryItem
{
    public string name;
    public KitchenToolState state = KitchenToolState.PRIMARY;

    public KitchenTool(string name) {
        this.name = name;
    }

    public void SetState(KitchenToolState newState) {
        state = newState;
    }

    public bool IsFree() {
        return state is KitchenToolState.PRIMARY; 
    }
}