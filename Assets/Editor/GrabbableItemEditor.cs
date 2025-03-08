using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(GrabbableItem))]
public class GrabbableItemEditor : Editor
{
    private SerializedProperty inventoryItemProperty;

    private void OnEnable()
    {
        inventoryItemProperty = serializedObject.FindProperty("item");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GrabbableItem grabbableItem = (GrabbableItem)target;

        grabbableItem.eternal = EditorGUILayout.Toggle("Eternal", grabbableItem.eternal);

        if (grabbableItem.item == null)
        {
            if (GUILayout.Button("Create Dish"))
            {
                grabbableItem.item = new Dish(new Food("defaultFood", 1), new List<Allergy>());
            }

            if (GUILayout.Button("Create KitchenTool"))
            {
                grabbableItem.item = new KitchenTool("defaultKitchenTool");
            }

            if (GUILayout.Button("Create Tray"))
            {
                grabbableItem.item = new Tray();
            }
        }
        else
        {
            if (grabbableItem.item is Dish dish)
            {
                GUILayout.Label("Dish Properties", EditorStyles.boldLabel);

                dish.food.name = EditorGUILayout.TextField("Dish Name", dish.food.name);

                if (dish.allergies == null) dish.allergies = new List<Allergy>();

                EditorGUILayout.LabelField("Allergies");

                for (int i = 0; i < dish.allergies.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    dish.allergies[i].name = EditorGUILayout.TextField($"Allergy {i + 1}", dish.allergies[i].name);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        dish.allergies.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("Add Allergy")) dish.allergies.Add(new Allergy(string.Empty));

                if (GUILayout.Button("Remove Dish"))
                {
                    grabbableItem.item = null;
                }
            }
            else if (grabbableItem.item is KitchenTool kitchenTool)
            {
                GUILayout.Label("KitchenTool Properties", EditorStyles.boldLabel);

                kitchenTool.name = EditorGUILayout.TextField("Tool Name", kitchenTool.name);
                kitchenTool.state = (KitchenToolState)EditorGUILayout.EnumPopup("State", kitchenTool.state);

                EditorGUI.indentLevel++;

                if (GUILayout.Button("Initialize recipes"))
                {
                    kitchenTool.InitRecipes();
                }
                //kitchenTool.recipes.name = EditorGUILayout.TextField("Recipe name", kitchenTool.recipes.name);
                //holder.kitchenTool.durability = EditorGUILayout.IntField("Durability", holder.kitchenTool.durability);

                EditorGUI.indentLevel--;
                //kitchenTool.recipes = EditorGUILayout.TextField("Recipes", kitchenTool.recipes);

                if (kitchenTool.recipes != null)
                {
                    EditorGUILayout.LabelField("Recipes");

                    for (int i = 0; i < kitchenTool.recipes.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        kitchenTool.recipes[i].name = EditorGUILayout.TextField($"Recipe {i + 1}", kitchenTool.recipes[i].name);


                        EditorGUILayout.LabelField("Ingredients", EditorStyles.boldLabel);

                        for (int j = 0; j < kitchenTool.recipes[i].ingredients.Count; j++)
                        {
                            kitchenTool.recipes[i].ingredients[j] = EditorGUILayout.TextField($"{j + 1}", kitchenTool.recipes[i].ingredients[j]);
                        }

                        if (GUILayout.Button("Add ingredient"))
                        {
                            kitchenTool.recipes[i].ingredients = AddIngredient(kitchenTool.recipes[i].ingredients);
                        }

                        if (GUILayout.Button("Remove ingredient", GUILayout.Width(60)))
                        {
                            kitchenTool.recipes.RemoveAt(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }


                    if (GUILayout.Button("Add recipe")) kitchenTool.recipes.Add(new Recipe(string.Empty, new List<string>()));

                    if (GUILayout.Button("Remove recipes"))
                    {
                        kitchenTool.recipes = null;
                    }
                }


                if (GUILayout.Button("Remove KitchenTool"))
                {
                    grabbableItem.item = null;
                }
            }

            else if (grabbableItem.item is Tray tray) 
            {
                //GUILayout.Label("Tray Properties", EditorStyles.boldLabel);

                if (GUILayout.Button("Remove Tray"))
                {
                    grabbableItem.item = null;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(grabbableItem);
    }

    List<string> AddIngredient(List<string> ingredients)
    {
        var newRecipes = ingredients;
        newRecipes.Add("");
        return newRecipes;
    }
}
