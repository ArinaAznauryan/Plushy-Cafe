using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PixelCrushers.DialogueSystem;

public class TrayTrigger : Interactable
{
    public GameObject traySlotsReference;
    GameObject savedTraySlotsReference;

    public List<GameObject> traySpotsObjs;
    public bool once = false;
    //public Order order;
    Tray tray;
    GrabbableItem inventoryItem;

    public bool AreSpotsAllFree() {
        foreach (GameObject spot in traySpotsObjs) {
            if (spot.transform.childCount > 0) return false;
        }
        return true;
    }

    void Awake() {
        base.Awake();

        savedTraySlotsReference = traySlotsReference;
        
    }

    // void InitializeTraySlots() {
    //     if (GetComponent<NetworkObject>().IsSpawned) {

    //         if (!IsOwner) return;

    //         foreach (Transform child in transform) {
    //             if (child.GetComponent<TraySlot>()) traySpotsObjs.Add(child.gameObject);
    //         }
    //     }

    //     // savedTraySlotsReference.transform.parent = parent;
    //     // savedTraySlotsReference.transform.localPosition = new Vector3(0, 0, 0);

    //     // foreach (Transform child in savedTraySlotsReference.transform) {
    //     //     GlobalManager.Instance.SpawnGlobalObject(child.gameObject);
    //     //     child.parent = transform;
    //     //     traySpotsObjs.Add(child.gameObject);
    //     // }
        
    //     // Destroy(savedTraySlotsReference);
    // }

    void Start()
    {
        //InitializeTraySlots();
        InitializeEvents();
        tray = new Tray();
        InitLocalData(new TrayHolder());
    }

    void InitializeEvents() {
        Usable usable = transform.GetComponent<Usable>();
        GetComponent<GrabbableItem>().SetToPickable();

        if (GetComponent<GrabbableItem>().eternal) {
            usable.overrideUseMessages.fMessage = "F to put back";
            usable.overrideUseMessages.eMessage = "E to take";
            usable.overrideUseMessages.rMessage = "";
        
            SetEventF(PutTrayBack);
        }
        else {
            usable.overrideUseMessages.fMessage = "F to put a dish";
            usable.overrideUseMessages.eMessage = "E to take";
            usable.overrideUseMessages.rMessage = "";

            SetEventF(PutDish);
           // SetEventE(PutDish);
        }
    }

    void PutTrayBack() {
        TrayTrigger tarTray = GameEventsManager.instance.inventory.slot.curGrabItem.gameObject.GetComponent<TrayTrigger>();
        if (tarTray && tarTray.AreSpotsAllFree()) GameEventsManager.instance.inventory.slot.RemoveItem();
        Destroy(tarTray.gameObject);
    }

    void Update() {
        inventoryItem = GameEventsManager.instance.inventory.slot.curGrabItem;

        UpdateSpots();
       
        //if (inventoryItem is null) gameObject.GetComponent<Usable>().enabled = false;
        //else gameObject.GetComponent<Usable>().enabled = true;
    }

    // void LateUpdate() {
    //     Debug.Log("In late update");
    //     UpdateSpots();
    // }

    bool SlotsFree() {
        foreach (GameObject obj in traySpotsObjs) {
            if (obj.transform.childCount > 0) {
                return false;
            }
        }
        return true;
    }

    public void PutDish() {
        if (inventoryItem?.item is Dish dish) { 
            /*tray.AddDish(dish);*/ 
            SetSpot();
            UpdateTray(); 
        }
    }

    void UpdateSpots() { 
        List<TraySlot> traySlots = traySpotsObjs.Select(go => go.GetComponent<TraySlot>())
            .Where(traySlot => traySlot != null)
            .ToList();

         tray.UpdateOrder(traySlots);
    }

    void UpdateTray() {
        List<TraySlot> traySlots = traySpotsObjs.Select(go => go.GetComponent<TraySlot>())
            .Where(traySlot => traySlot != null)
            .ToList();

        tray.UpdateOrder(traySlots);
    }

    void SetSpot() {
        foreach (GameObject obj in traySpotsObjs) {
            
            if (obj.transform.childCount < 1) {
                GrabbableItem curItem = inventoryItem;
                curItem.Disarm(obj, true);
                curItem.enabled = true;
                //obj.GetComponent<TraySlot>().SetItem(curItem.item);
                break;
            }
        }
        GameEventsManager.instance.inventory.slot.curGrabItem = null;
    }

}

[System.Serializable]
public class Tray : InventoryItem {
    public Order order;
    public List<TraySlot> traySlots;
    const int traySpotAmount = 5;
    public List<Dish> dishes;
    List<Allergy> allergies;

    public Tray() {
        traySlots = new List<TraySlot>();
        allergies = new List<Allergy>();
    }

    // public void AddDish(Dish dish) {
    //     dishes.Add(dish);
    //     traySpots.Add(new TraySpot(traySpots.Count-1, TraySpotStatus.BUSY, dish));
    //     UpdateOrder();
    // }

    public void RemoveDish(int idx) {
        traySlots.RemoveAt(idx);
        dishes.RemoveAt(idx);
    }

    // public bool IsFull() {
    //     if (traySpots.Count == traySpotAmount) return true;
    //     return false;
    // }

    void UpdateDishes(List<TraySlot> slots) {
        dishes = new List<Dish>();
        foreach (TraySlot slot in slots) {
            Debug.Log("In slot loop");

            if (slot.item != null)  {
                if ((slot.item as Dish) != null)  {
                        if ((slot.item as Dish).food != null) {
                        Debug.Log("Slot.item is not null:  " + slot.item);
                        dishes.Add(slot.item as Dish);
                    }
                }
            }

            else Debug.LogError("Slot.item is null!");
        }

        foreach (Dish dish in dishes) {
            if (dish != null) {
                List<Allergy> tarAllergies = GameEventsManager.instance.Tools.GetFoodAllergies(dish.food);
                foreach (Allergy allergy in dish.allergies) {
                    allergies.Add(allergy);
                }
            }
        }
    }

    

    public void UpdateOrder(List<TraySlot> slots) {
        traySlots = slots;
        UpdateDishes(slots);

        /*if (IsFull()) */order = new Order(dishes, allergies);
    }

    public Order ReturnOrder() {
        return order;
    }

}

public class TraySpot {
    int arrangement;
    TraySpotStatus status;
    Dish dish;

    public TraySpot(int arrangement, TraySpotStatus status, Dish dish) {
        this.arrangement = arrangement;
        this.status = status;
        this.dish = dish;
    }
}

public enum TraySpotStatus {EMPTY, BUSY}