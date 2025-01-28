using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class Blender : BlenderData
{
    GrabbableItem milk;
    public bool fucku = false;
    string finalDish;

    void Start()
    {
        base.Start();

        Initialize(new Color32(255, 255, 255, 255), 20000);
        color = new Color32(255, 255, 255, 255);
    }

    public void Mix(Food food) {
        SetColor(DefineColor(food));
    }

    public void MixIngredient() {
        timer.StartTimer();
        liquid.liquidColor2 = color;
        //timeline.Play("mix");
        Debug.Log("Trying to mix and starting the timer");
        animator.Play("mix");
    }

    void AddMilk() {
        ArmMilk();
        animator.Play("pourMilk");//timeline.Play("pourMilk");

        milk.GetComponent<Animator>().enabled = true;

        milk.GetComponent<Animator>().Play("pour");
        Mix((milk.item as Dish).food);
    }

    void PourInCup(GrabbableItem cup) {
        ArmCup(cup);
        EmptyBlenderIngredients();
        animator.Play("pourIntoCup");//timeline.Play("pourOut");

        cup.GetComponent<Animator>().enabled = true;
        
        cup.GetComponent<Animator>().Play("pourIn");
    }

    public void ArmMilk() {
        milk.DropNoGravity();
    }

    public void DisarmMilk() {
        GameEventsManager.instance.inventory.slot.SetItem(milk);
    }

    public void ArmCup(GrabbableItem cup) {
        cup.DropNoGravity();
    }

    public override void OnMix() {
        liquid.liquidColor2 = color;
        Debug.Log("On mix: ");
        MixIngredient();
    }

    public override void OnFinished() {
        animator.Play("done");
    }

    public override void OnTake() {
        Slot slot = GameEventsManager.instance.inventory.slot;
        if (slot.curGrabItem.GetComponent<Mug>()) PourInCup(slot.curGrabItem);
        Debug.Log("Taking the milkshake!");
    }

    public void FinalizeDish() {
        Mug cup = GameEventsManager.instance.inventory.slot.curGrabItem.GetComponent<Mug>();
        Color32 colorBuf = this.color;
        finalDish = DetermineDishName();
        Debug.Log("cocktail ingerdeits:" + finalDish);
        cup.GetComponent<Mug>().FinishDish(finalDish, colorBuf);
    }

    public override void OnPutIngredient() {
        Debug.Log("Putting new ingredients!");

        var slot = GameEventsManager.instance.inventory.slot;

        if (slot.curGrabItem.item is Dish dish && GameEventsManager.instance.Tools.IfCorrespondingFood(GetComponent<GrabbableItem>().item, dish.food)/* && isEmpty()*/) {
            
            if (isEmpty() && dish.food.name is "milk") {
                SetMilk(slot.curGrabItem);
                Mix(dish.food);
                AddMilk();
                FinalIdleLevel();
            }

            else {
                if (!(dish.food.name is "milk")) {
                    Mix(dish.food);

                    AddIngredient(slot.curGrabItem);
                }
            }
            
        }

        else {
            AnnulMilk();
        }
    }

    void SetMilk(GrabbableItem milk) {
        this.milk = milk;
    }

    void AnnulMilk() {milk = null;}
}