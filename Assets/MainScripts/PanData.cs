using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PixelCrushers.DialogueSystem;
//using LiquidVolumeFX;

public class PanData : Interactable {
    GameObject ingredientParser, result;
    public Timer timer;
    //public LiquidVolume liquid;
    //List<Ga
    protected List<string> ingredients;
    bool interactionsEnabled = true;

    public void Initialize(int fryDuration) {
        ingredients = new List<string>();
        timer = new Timer(fryDuration);
    }

    public string DetermineDishName() {
        string resultName = string.Join(" ", ingredients.Distinct());
        Food food = new Food(resultName, 1);
        Reset();
        return food.name;
    }

    void Reset() {
        Initialize(20000);
    }


    void Awake() {
        base.Awake();
        EnableInteractions();
        //InitInteractions();
        ingredientParser = transform.Find("ingredientParser").gameObject;
    }

    void Update() {
        if (timer.IsTimePassed()) {
            timer.ResetTimer();
            OnFinished();
            InitInteractions2();
        }

        if (interactionsEnabled && IsPlaced(new StoveHolder()) && !lockedIn) {
            lockedIn = true;
            LockIn();
        }
    }

    public void InitInteractions() {
        RemoveAllEventsE();
        SetEventE(OnPutIngredient, "E to put ingredient");
    }

    public void InitInteractions1() {
        SetEventF(OnMix,  "F to fry");
    }

    public void InitInteractions2() {
        usable.enabled = true;
        SetEventR(OnTake, "R to pour out");
    }

    protected void Start() {
        base.Start();

        InitLocalData(new StoveHolder());
    }

    protected void OnPlacedEnter() {
        if (IsPlaced(new StoveHolder())) LockIn();
    }

    protected void LockIn() {
        Debug.Log("Placed on stove");
        RemoveAllEventsE();
        SetEventE(OnPutIngredient, "E to put ingredient");
    }


    // public void ParseToCup(Mug cup) {
    //     cup.DefineIngredients()
    // }

    public Dish DefineDish(List<string> ingredients) {
        return GameEventsManager.instance.Tools.DefineDish(ingredients);
    }

    public void TryAddIngredient(GrabbableItem ingredient) {
        Dish curIngredient = ingredient.item as Dish;

        if (ingredients.Contains(curIngredient.food.name)) {
            Debug.LogError("Ingredient already added into the pan!");
            return;
        }

        ingredients.Add(curIngredient.food.name);
        
        AddIngredient(ingredient);
    }

    public void FinalIdleLevel() {
        //liquid.level = .1f;
    }

    public void EmptyPan() {
        //liquid.level = 0f;
    }

    public void DropIntoPan() {
        var ingredient = GameEventsManager.instance.inventory.slot.curGrabItem;
        GameEventsManager.instance.inventory.slot.DropItem();

        ingredient.transform.parent = transform.Find("ingredients");
    }

    public void AddIngredient(GrabbableItem ingredient) {
    
        ingredient.Disarm(ingredientParser);
        animator.Play("addIngredient");

        /*if (isEmpty()) GetComponent<Animator>().Play("addIngredient");
        else GetComponent<Animator>().Play("addIngredientOilIn");*/
    }

    public void EmptyPanIngredients() {
        foreach(Transform child in transform.Find("ingredients")) Destroy(child.gameObject);
    }

    public bool isEmpty() {
       // if (liquid.level is 0f) return true;
        return false;
    }

    public virtual void OnFinished() {
        
    }

    public virtual void OnMix() {

    }

    public virtual void OnTake() {

    }

    public virtual void OnPutIngredient() {

    }

    public virtual void OnOven() {

    }

}
