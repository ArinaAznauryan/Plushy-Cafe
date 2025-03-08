using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitchenToolState {PRIMARY, CHANGED}



[System.Serializable]
public class KitchenTool : InventoryItem
{
    public string name;
    public KitchenToolState state = KitchenToolState.PRIMARY;
    [SerializeField] public List<Recipe> recipes;

    public KitchenTool(string name) {
        this.name = name;
    }

    public void InitRecipes() {
        Debug.Log("Name: " + name);
        recipes = GameEventsManager.instance.Tools.FindValidRecipes(this);
    }

    public void SetState(KitchenToolState newState) {
        state = newState;
    }

    public bool IsFree() {
        return state is KitchenToolState.PRIMARY; 
    }

}