using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : Interactable
{
    public Slot inventory;
    protected Holder type;


    void Update() {
        inventory = GameEventsManager.instance.inventory.slot;
    }

    public void PlaceObject() {
        if (inventory != null) {
            GrabbableItem grabbable = inventory.curGrabItem;
            
            if (BelongsTo(grabbable)) {
                grabbable.Disarm(gameObject, new Holder());
                grabbable.Arm(new Holder());
                inventory.AnnulItem();
                return;
            }
        }
    }

    bool BelongsTo(GrabbableItem grabbable) {
        /* if the curGrabItem.item (which can be a kitchentool, dish, and a tray) belongs to the holder type (stove - pan and pot) 
        this is not the best idea, think better, biatch
        either make all the grabbables (dishes, blender, coffee machine) inventoryItems,
        or make a function that decides, whether a curGrab.item belongs to a holder
        */
        if (grabbable.item is KitchenTool || grabbable.item is Tray) {
            Debug.Log("Is a kitchen tool or a tray");
            if (grabbable.GetComponent<Interactable>() is Interactable interactable) {
                return interactable.GetHolder()?.GetType() == type.GetType();
            }

            return false;
        }

        return false;
    }

    protected void InitType(Holder type) {
        this.type = type;
    }   

}
