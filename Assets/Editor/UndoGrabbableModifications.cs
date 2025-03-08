using UnityEditor;
using UnityEngine;

public class UndoGrabbableModifications : EditorWindow
{
    [MenuItem("Tools/Undo Grabbable Changes")]
    public static void UndoChanges()
    {
        // Find all objects with GrabbableItem in the scene
        GrabbableItem[] grabbables = FindObjectsOfType<GrabbableItem>();

        foreach (GrabbableItem grabbable in grabbables)
        {
            Transform defaultChild = grabbable.transform.Find("default");

            if (defaultChild != null)
            {
                // Remove ProxyTrigger if it exists
                ProxyTrigger proxy = defaultChild.GetComponent<ProxyTrigger>();
                if (proxy != null)
                {
                    Undo.DestroyObjectImmediate(proxy);
                }

                // Remove MeshCollider if it exists
                MeshCollider collider = defaultChild.GetComponent<MeshCollider>();
                if (collider != null)
                {
                    Undo.DestroyObjectImmediate(collider);
                }

                // Apply changes to the prefab
                PrefabUtility.RecordPrefabInstancePropertyModifications(grabbable.gameObject);
                PrefabUtility.ApplyPrefabInstance(grabbable.gameObject, InteractionMode.UserAction);
                
            }
        }

        Debug.Log("Undo script finished! All ProxyTrigger components and MeshColliders have been removed.");
    }
}
