using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Interactable
{

    public string finalDishIngredient;
    GameObject finalDish;
    GameObject visual;

    void Start()
    {
        base.Start();
    }

    void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        visual = gameObject;
    }

    void DefineIngredients(string dishName, List<string> ingredients) {
        finalDishIngredient = dishName;
    }


    public void FinishDish(string dishName, List<string> ingredients) {
        DefineIngredients(dishName, ingredients);

        //finalDish = Resources.Load<GameObject>("Dishes/FoodsFromScratch/cocktails/" + finalDishIngredient);
        string path = "Dishes/FoodsFromScratch/friedFood";
        finalDish = GameEventsManager.instance.Tools.LoadPrefabOnRandOrderPath((dishName.Split(' ')).ToList(), path);

        //Destroy(transform.GetChild(0)?.gameObject);
        var finalObj = Instantiate(finalDish, transform.position, Quaternion.identity);
        //finalObj.transform.parent = transform;
        //finalObj.transform.localPosition = new Vector3(0, 0, 0);
        visual = finalObj;

        Destroy(gameObject);
    }
}
