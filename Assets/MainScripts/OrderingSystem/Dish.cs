using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dish : InventoryItem
{
    public Food food;
    public List<Allergy> allergies;

    public Dish(Food food, List<Allergy> allergies) {
        this.food = food;
        this.allergies = allergies;
    }
}