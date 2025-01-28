// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Netcode;

// public class NetworkObjectSpawnerManual : MonoBehaviour
// {
//     public bool spawn = false;
//     GameObject potentialParent = null;
//     NetworkObject networkObj;


//     void Awake() {
//         networkObj = GetComponent<NetworkObject>();
//         //if (!networkObj.IsSpawned) RequestSpawnServerRpc();
//     }

//     void Start() {
        
//     }

//     void Update() {

//         //Debug.Log("Are you even fucking running?");
//         if (!networkObj.IsSpawned && spawn) {
//             Debug.Log("IS NOT SPAWNED IN UPDATE");
//             TrySpawn();
//         }

//         if (potentialParent != null && transform.parent is null) {
            
//             TryReparent();
//         }
//     }

//     void TrySpawn() {
//     if (!networkObj.IsSpawned && spawn) {
//         // Debug log to check what's going on
//         Debug.Log("Attempting to spawn object: " + gameObject.name);

//         // if (transform.parent != null) { 
//         //     potentialParent = transform.parent.gameObject;

//         //     // Check if parent is spawned
//         //     if (potentialParent.GetComponent<NetworkObject>() != null && !potentialParent.GetComponent<NetworkObject>().IsSpawned) {
//         //         Debug.LogError("Parent network object not spawned yet!: " + potentialParent.name);
//         //         return;  // Don't proceed if parent is not spawned
//         //     }
//         // }

//         try {
//             //GameEventsManager.instance.Tools.InstantiateWithNetwork(gameObject, Vector3.zero);
//             GlobalManager.Instance.SpawnGlobalObject(gameObject);
//             Debug.Log("Object spawned successfully: " + gameObject.name);
//             spawn = false;
//         } catch (System.Exception ex) {
//             Debug.LogError("Error during spawn: " + ex.Message);
//         }

//         //if (IsOwner) spawn = false;  // Ensures it only spawns once
//     }
// }


//     void TryReparent() {
//         if (potentialParent.GetComponent<NetworkObject>() != null) 
//             networkObj.transform.parent = potentialParent.transform;
//     }
// }
