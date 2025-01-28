using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
}
