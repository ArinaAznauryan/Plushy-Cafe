using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraySlot : MonoBehaviour
{
    public InventoryItem item;

    public void SetItem(InventoryItem item) {
        this.item = item;
    }

    public InventoryItem GetItem() {
        return item;
    }

    void Update() {
        if (transform.childCount > 0) item = transform.GetChild(0).GetComponent<GrabbableItem>().item;
    }

    public void ResetItem() {
        this.item = null;
    }
}
