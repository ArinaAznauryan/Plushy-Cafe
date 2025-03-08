using UnityEngine;
using UnityEditor;

public class ProxyTriggerSetup : EditorWindow
{
    [MenuItem("Tools/Setup Proxy Triggers")]
    public static void ShowWindow()
    {
        GetWindow<ProxyTriggerSetup>("Setup Proxy Triggers");
    }

    private void OnGUI()
    {
        GUILayout.Label("Proxy Trigger Setup", EditorStyles.boldLabel);

        if (GUILayout.Button("Find and Setup Grabbable Items in Prefabs"))
        {
            SetupProxyTriggers();
        }
    }

    private static void SetupProxyTriggers()
    {
        // Find all GrabbableItem objects in the scene
        GrabbableItem[] grabbableItems = FindObjectsOfType<GrabbableItem>();

        int modifiedCount = 0;

        foreach (var item in grabbableItems)
        {
            // Get the prefab root (if the object is part of a prefab)
            GameObject prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(item.gameObject);
            if (prefabRoot == null) continue; // Skip if it's not a prefab

            // Load the original prefab asset
            GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
            if (prefabAsset == null) continue;

            // Open the prefab for editing
            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
            if (prefabInstance == null) continue;

            // Find the "default" child
            Transform child = prefabInstance.transform.Find("default");

            if (child != null && child.parent == prefabInstance.transform) // Ensure it's a direct child
            {
                // Check if the child has a MeshRenderer
                if (child.GetComponent<MeshRenderer>())
                {
                    // Add ProxyTrigger script if it's not already there
                    if (!child.GetComponent<ProxyTrigger>())
                    {
                        child.gameObject.AddComponent<ProxyTrigger>();
                    }

                    // Add MeshCollider if it's not already there
                    if (!child.GetComponent<MeshCollider>())
                    {
                        child.gameObject.AddComponent<MeshCollider>();
                    }

                    modifiedCount++;

                    // Apply changes back to the prefab
                    PrefabUtility.SaveAsPrefabAsset(prefabInstance, AssetDatabase.GetAssetPath(prefabAsset));
                }
            }

            // Destroy the temporary prefab instance
            DestroyImmediate(prefabInstance);
        }

        Debug.Log($"Finished setup. Modified {modifiedCount} prefab(s).");
    }
}