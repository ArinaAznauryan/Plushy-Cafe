using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : PanData
{
    GrabbableItem oil;
    string finalDish;

    void Start()
    {
        base.Start();
        
        Initialize(20000);
    }

    public void StartFrying() {
        timer.StartTimer();
        GetComponent<Animator>().Play("fry");
    }

    void AddOil() {
        ArmOil();
        GetComponent<Animator>().Play("pourOil");

        oil.GetComponent<Animator>().enabled = true;
        oil.GetComponent<Animator>().Play("pour");

        TryAddIngredient(oil);
    }

    void PourInCup(GrabbableItem plate) {
        ArmPlate(plate);
        EmptyPanIngredients();
        GetComponent<Animator>().Play("pourIntoPlate");

        plate.GetComponent<Animator>().enabled = true;
        
        plate.GetComponent<Animator>().Play("pourIn");

        //GameEventsManager.instance.Tools.PlaySound(GetComponent<AudioSource>(), "pouring");
    }

    public void ArmOil() {
        oil.DropNoGravity();
    }

    public void DisarmOil() {
        GameEventsManager.instance.inventory.slot.SetItem(oil);
    }

    public void ArmPlate(GrabbableItem plate) {
        plate.DropNoGravity();
    }

    public override void OnMix() {
        StartFrying();
    }

    public override void OnFinished() {
        GetComponent<Animator>().Play("done");
    }

    public override void OnTake() {
        Slot slot = GameEventsManager.instance.inventory.slot;
        if (slot.curGrabItem.GetComponent<Plate>()) PourInCup(slot.curGrabItem);
        Debug.Log("Taking the fried food!");
    }

    public void FinalizeDish() {
        Plate plate = GameEventsManager.instance.inventory.slot.curGrabItem.GetComponent<Plate>();
        finalDish = DetermineDishName();
        Debug.Log("pan ingerdeits:" + finalDish);
        plate.GetComponent<Plate>().FinishDish(finalDish, ingredients);
    }

    public override void OnPutIngredient() {
        Debug.Log("Putting new ingredients!");

        var slot = GameEventsManager.instance.inventory.slot;

        if (slot.curGrabItem.item is Dish dish && GameEventsManager.instance.Tools.IfCorrespondingFood(GetComponent<GrabbableItem>().item, dish.food)) {
            
            if (isEmpty() && dish.food.name is "oil") {
                SetOil(slot.curGrabItem);
                TryAddIngredient(slot.curGrabItem);
                //AddOil();
                //FinalIdleLevel();
            }

            else {
                if (!(dish.food.name is "oil")) {
                    TryAddIngredient(slot.curGrabItem);
                }
            }
            
        }

        else AnnulOil();
    }

    void SetOil(GrabbableItem oil) {this.oil = oil;}
    void AnnulOil() {oil = null;}
}