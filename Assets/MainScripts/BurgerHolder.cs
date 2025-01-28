using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BurgerHolder : IStackableHolder
{
    public List<GameObject> stackedVersions;
    int placedPieces, i = 0;
    GameObject visual;
    GameObject finalDish;
    

    void Awake() {
        base.Awake();
        
        target = gameObject;
        stackedVersions = new List<GameObject>(Resources.LoadAll<GameObject>("Dishes/FoodsFromScratch/burger/BurgerVersions"));
        
        
        finalDish = (stackedVersions.Where(x => x.name.Equals("finishedDish")).ToList())[0];

        stackedVersions = stackedVersions.Where(x => !x.name.Equals(finalDish.name)).ToList();

        visual = transform.GetChild(0).gameObject;

        //Debug.Log("SIZE: " + visual.GetComponent<Renderer>().bounds.size);
    }


    public override void OnFocused() 
    {
       Debug.Log("Its focused!");
    }

    public override void OnStacking() 
    {
        var curItem = GameEventsManager.instance.inventory.slot.curGrabItem;
        if (curItem != null) Stack(curItem);
    }

    public void Stack(GrabbableItem piece) {
        Debug.Log("Its working!");
        if (piece.item is Dish dish) {
            string foodName = dish.food.name;
            switch (placedPieces) {
                case 0: 
                    if (foodName is "bottomBun") StackBurger(0);
                    break;
                case 1: 
                    if (foodName is "patty") StackBurger(1);
                    break;
                case 2: 
                    if (foodName is "cheeseSlice") StackBurger(2);
                    break;
                case 3: 
                    if (foodName is "slicedLettuce") StackBurger(3);
                    break;
                case 4: 
                    if (foodName is "slicedTomato") StackBurger(4);
                    break;
                case 5:
                    if (foodName is "topBun") StackBurger(5);
                    break;
                default: break;
            }
        }

    }

    void StackBurger(int order) {
        GameEventsManager.instance.inventory.slot.RemoveItem();
        usable.PlaySound("pick");
        visual.GetComponent<MeshFilter>().sharedMesh = stackedVersions[order].GetComponent<MeshFilter>().sharedMesh;
        placedPieces++;

        if (placedPieces == stackedVersions.Count) {
            InitInteractions1();
            FinishDish();
        }
    }

    void FinishDish() {
        Destroy(transform.GetChild(0).gameObject);
        var finalObj = Instantiate(finalDish, new Vector3(0, 0, 0), Quaternion.identity);
        finalObj.transform.parent = transform;
        finalObj.transform.localPosition = new Vector3(0, 0, 0);
        visual = transform.GetChild(0).gameObject;
    }

}