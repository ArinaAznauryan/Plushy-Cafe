using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PixelCrushers.DialogueSystem;
using LiquidVolumeFX;

public class BlenderData : Interactable {
    GameObject ingredientParser;
    public LiquidVolume liquid;
    public Color32 color, prevColor;
    public Timer timer;
    List<string> ingredients;

    public void Initialize(Color32 color, int mixDuration) {
        ingredients = new List<string>();
        liquid.liquidColor2 = color;

        this.color = color;
        prevColor = new Color32(0, 0, 0, 255);

        SetColor(color);
        prevColor = new Color32(0, 0, 0, 255);

        timer = new Timer(mixDuration);
    }

    public string DetermineDishName() {
        string result = string.Join(" ", ingredients.Distinct());
        Food food = new Food(result, 1);
        Reset();
        return food.name;
    }

    void Reset() {
        Initialize(new Color32(255, 255, 255, 255), 20000);
    }

    public void DisableInteractions() {
        usable.enabled = false;
        // usable.RemoveEventE(OnPutIngredient);
        // usable.RemoveEventR(OnTake);
        // transform.Find("blenderButton").GetComponent<Usable>().RemoveEventF(OnMix);
    }
    public void EnableInteractions() {
        usable.enabled = true;
        // usable.AddEventE(OnPutIngredient, "E to put ingredient");
        // usable.AddEventR(OnTake, "R to pour out");
        // transform.Find("blenderButton").GetComponent<Usable>().AddEventF(OnMix, "F to mix");
    }

    public void InitInteractions() {
        Debug.Log("Setting event E");
        RemoveAllEventsE();
        SetEventE(OnPutIngredient, "E to put ingredient");
    }

    public void InitInteractions1() {
        Debug.Log("Now can mix blener");
        transform.Find("blenderButton").GetComponent<Usable>().AddEventF(OnMix, transform.Find("blenderButton").gameObject, "F to mix");
    }

    public void InitInteractions2() {
        SetEventR(OnTake, "R to pour out");
    }

    void Awake() {
        base.Awake();

        //InitTimeline(new string[]{"addIngredientMilkIn", "addIngedient", "pourMilk", "mix"/*, "pourOut*/});
        
        ingredientParser = transform.Find("ingredientParser").gameObject;

        liquid = transform.Find("blenderLiquid").GetComponent<LiquidVolume>();
        liquid.liquidColor2 = color;

    }

    public void Start() {
        base.Start();
        
        InitInteractions();
    }

    void Update() {
        if (timer.IsTimePassed()) {
            timer.ResetTimer();
            OnFinished();
        }
    }

    // public void ParseToCup(Mug cup) {
    //     cup.DefineIngredients()
    // }

    public Color32 DefineColor(Food food) {
        if (food.name != "milk") {
            ingredients.Add(food.name);
            Debug.Log("Blue fuck u:" + food.name);
        }

        switch (food.name) {
            case "banana":
                return new Color32(254, 255, 186, 255);
            case "strawberry":
                return new Color32(255, 93, 135, 255);
            case "chocolate":
                return new Color32(108, 91, 85, 255);
            case "milk": 
                return new Color32(240, 240, 240, 255);
            case "orange":
                return new Color32(231, 144, 82, 255);
            case "lemon":
                return new Color32(255, 235, 80, 255);
            default: return new Color32(0, 0, 0, 255);
        }
    }

    public void SetColor(Color32 newColor) {
        if (prevColor.r != newColor.r && prevColor.g != newColor.g && prevColor.b != newColor.b) {
            color = new Color32
            (
                // (byte)((color.r * newColor.r)/255), 
                // (byte)((color.g * newColor.g)/255), 
                // (byte)((color.b * newColor.b)/255), 255
                (byte)(255 - Mathf.Sqrt(((255-color.r)*(255-color.r) + (255-newColor.r)*(255-newColor.r))/2)),
                (byte)(255 - Mathf.Sqrt(((255-color.g)*(255-color.g) + (255-newColor.g)*(255-newColor.g))/2)),
                (byte)(255 - Mathf.Sqrt(((255-color.b)*(255-color.b) + (255-newColor.b)*(255-newColor.b))/2)), 255
            );

            prevColor = newColor;
        }
    }

    public void FinalIdleLevel() {
        liquid.level = .8f;
    }

    public void EmptyBlender() {
        liquid.level = 0f;
    }

    public void DropIntoBlender() {
        var ingredient = GameEventsManager.instance.inventory.slot.curGrabItem;
        GameEventsManager.instance.inventory.slot.DropItem();

        ingredient.transform.parent = transform.Find("ingredients");
    }

    public void AddIngredient(GrabbableItem ingredient) {
    
        ingredient.Disarm(ingredientParser);

        if (isEmpty()) GetComponent<Animator>().Play("addIngredient");//timeline.Play("addIngredient");
        else GetComponent<Animator>().Play("addIngredientMilkIn");//timeline.Play("addIngredientMilkIn");
    }

    public void EmptyBlenderIngredients() {
        foreach(Transform child in transform.Find("ingredients")) Destroy(child.gameObject);
    }

    public bool isEmpty() {
        if (liquid.level is 0f) return true;
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

}
