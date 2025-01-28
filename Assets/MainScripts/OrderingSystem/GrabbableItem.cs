using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableItem : MonoBehaviour
{
    public GameObject rootPrefab;

    [SerializeField] public bool eternal = false;

    [SerializeReference] public InventoryItem item;

    void Awake() {
        SetToPickable();
        if (item is Dish dish) dish.allergies = GameEventsManager.instance.Tools.GetFoodAllergies(dish.food);
    }

    public void SetToPickable() {
        Debug.Log("Setting to pickable?: " + gameObject.name);
        transform.GetComponent<Usable>().AddEventE(GameEventsManager.instance.inventory.slot.SetItem, gameObject, "E to take");
    }

    public void SetToUnpickable() {
        Debug.Log("Setting to unpickable?: " + gameObject.name);
        transform.GetComponent<Usable>().RemoveEventE(GameEventsManager.instance.inventory.slot.SetItem);
    }

    public void PlaySound(string name) {
        MyPlayer player = GameEventsManager.instance.player;
        GameEventsManager.instance.Tools.PlaySound(player.playerManager.audio, name);
    }

    public void Disarm(GameObject inventory, bool tray = false) {
        transform.parent = inventory.transform;
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        transform.localScale = tray ? new Vector3(1, 1, 1) : new Vector3(1.5f, 1.5f, 1.5f);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        EnableCollider(false, true);
        
        gameObject.GetComponent<Usable>().enabled = tray ? true : false;

        PlaySound("pick");
    }

    public void Disarm(GameObject inventory, Holder holder) {
        transform.parent = inventory.transform;
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        EnableCollider(false, true);
        
        gameObject.GetComponent<Usable>().enabled = false;

        PlaySound("pick");
    }


    void EnableCollider(bool enable, bool tray = false) {
        if (!enable) {
            gameObject.GetComponent<Collider>().enabled = tray ? true : false;
        }
        else gameObject.GetComponent<Collider>().enabled = true;
    }

    void DisableCollider(bool disableChildren) {
        gameObject.GetComponent<Collider>().enabled = false;

        if (disableChildren) {
            foreach (Transform child in transform) {
                child.GetComponent<Collider>().enabled = false;
            }
        }
    }


    public void Arm(bool gravity = true) {
        transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<Usable>().enabled = true;
        
        EnableCollider(true);
        gameObject.GetComponent<Rigidbody>().isKinematic = !gravity;

        PlaySound("pick");
    }

    public void Arm(Holder holder) {
        transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.GetComponent<Usable>().enabled = true;
        
        EnableCollider(true);

        PlaySound("pick");
    }

    public Holder IsUnder() {
        return transform.parent?.GetComponent<Holder>();
    }

    public void Drop() {
        Arm();
        transform.parent = null;
    }

    public void DropDepending() {
        Arm();
    }

    public void DropNoGravity() {
        Arm(false);
        transform.parent = null;
    }
}

[System.Serializable]
public class InventoryItem
{
    //public string name;
}
