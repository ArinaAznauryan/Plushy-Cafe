using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    void Start()
    { 
        // Create a new NavMesh instance
        NavMeshData navMeshData = new NavMeshData();
        Debug.Log("NavMeshData created successfully!");

        // Example NavMesh build settings
        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(0);
        Debug.Log("NavMeshBuildSettings retrieved successfully!");
    }
}
