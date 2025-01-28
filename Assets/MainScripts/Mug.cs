using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LiquidVolumeFX;

public class Mug : MonoBehaviour
{
    GrabbableItem mug;
    Animator animator;
    public string finalDishIngredient;
    GameObject finalDish;
    GameObject visual;
    public LiquidVolume liquid;

    void Start()
    {
        mug = GetComponent<GrabbableItem>();
        animator = GetComponent<Animator>();
        visual = gameObject;
        liquid = transform.Find("liquid").GetComponent<LiquidVolume>();
    }

    void DefineIngredients(string dishName, Color32 color) {
        finalDishIngredient = dishName;
        liquid.liquidColor2 = color;
    }

    public void FinalIdleLevel() {
        liquid.level = 1f;
    }

    public void FinishDish(string dishName, Color32 color) {
        DefineIngredients(dishName, color);

        //finalDish = Resources.Load<GameObject>("Dishes/FoodsFromScratch/cocktails/" + finalDishIngredient);
        string path = "Dishes/FoodsFromScratch/cocktails/";
        finalDish = GameEventsManager.instance.Tools.LoadPrefabOnRandOrderPath((dishName.Split(' ')).ToList(), path);

        //Destroy(transform.GetChild(0)?.gameObject);
        var finalObj = Instantiate(finalDish, transform.position, Quaternion.identity);
        //finalObj.transform.parent = transform;
        //finalObj.transform.localPosition = new Vector3(0, 0, 0);
        visual = finalObj;

        Destroy(gameObject);
    }
}
 