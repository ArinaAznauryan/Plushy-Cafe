using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

[System.Serializable]
public class Slot : MonoBehaviour 
{
    [SerializeField] public InventoryItem item, potentialItem;
    GameObject itemObj;

    public GrabbableItem curGrabItem;

    public void RemoveItem() {
        item = null;
        potentialItem = null;
        Destroy(transform.GetChild(0)?.gameObject);

        AnnulItem();
    }
    
    public void DropItem(bool depending = false) {
        curGrabItem.enabled = true;
        if (!depending) curGrabItem.Drop();
        else curGrabItem.DropDepending();
        curGrabItem = null;
    }

    public void AnnulItem() {
        curGrabItem = null;
    }

    void Update() {
        if (InputDeviceManager.IsKeyDown(KeyCode.B) && !(curGrabItem.item is Tray)) DropItem();
    }

    public void SetItem() {
        GameObject ObjectOnFocus = GameEventsManager.instance.Tools.ObjectOnFocus();
        bool delPrev = false;
        GrabbableItem prevItem = null;

        if (transform.childCount > 0) {
            prevItem = transform.GetChild(0).gameObject.GetComponent<GrabbableItem>();
            delPrev = true;
        }

        if (ObjectOnFocus?.GetComponent<GrabbableItem>() != null && !(prevItem?.item is Tray)) {
            
            curGrabItem = ObjectOnFocus.GetComponent<GrabbableItem>();

            this.item = curGrabItem.item;
            GenerateSlotItem(prevItem, delPrev);
        }
    }

    public void SetItem(GrabbableItem finalItem) {
        bool delPrev = false;
        GrabbableItem prevItem = null;

        if (transform.childCount > 0) {
            prevItem = transform.GetChild(0).gameObject.GetComponent<GrabbableItem>();
            delPrev = true;
        }

        if (finalItem.GetComponent<GrabbableItem>() != null && !(prevItem?.item is Tray)) {
            
            curGrabItem = finalItem.GetComponent<GrabbableItem>();

            this.item = curGrabItem.item;
            GenerateSlotItem(prevItem, delPrev);
        }
    }

    void GenerateSlotItem(GrabbableItem prevItem, bool deletePrev) {
        
        if (deletePrev) {
            prevItem.enabled = true;
            prevItem.Drop();
        }

        if (curGrabItem.eternal) {
            curGrabItem = Instantiate(curGrabItem.gameObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GrabbableItem>();
            curGrabItem.eternal = false;
        }

        if (curGrabItem.item is Tray) curGrabItem.Disarm(gameObject, new Holder());
        else curGrabItem.Disarm(gameObject);

        curGrabItem.enabled = false;
    }
}
