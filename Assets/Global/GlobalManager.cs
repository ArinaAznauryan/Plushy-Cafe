// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// public class GlobalManager : MonoBehaviour
// {
//     public static GlobalManager Instance;
//     public Dictionary<ulong, MyPlayer>  players = new Dictionary<ulong, MyPlayer>();
//     bool once = true;

//     public GameObject networkObjectPrefab; // Prefab of the object you want to spawn

//     void Awake()
//     {

//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(GlobalManager.Instance.gameObject);
//         }
//         Debug.Log("GETTING SPAWNED?");
//        // if (!GetComponent<NetworkObject>().IsSpawned) 
//             //GlobalManager.Instance.gameObject.GetComponent<NetworkObject>().Spawn();
//     }

//     void Start() {
//        // if (!GetComponent<NetworkObject>().IsSpawned)
//             //GlobalManager.Instance.gameObject.GetComponent<NetworkObject>().Spawn();
//         if (NetworkManager.Singleton != null)
//         {
//             // Subscribe to the OnClientConnectedCallback event
//             NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
//         }
//         Debug.Log("NetworkManager is null at start!");
//     }

//     void OnEnable() { 
        
//         if (once) {
//             once = false;
//             if (NetworkManager.Singleton != null)
//             {
//                 // Subscribe to the OnClientConnectedCallback event
//                 //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
//                 Debug.Log("Local singleton is not null!");
//                 Debug.Log("Local client id: " + NetworkManager.Singleton.LocalClientId);
//                 CallToClientConnectedServerRPC(NetworkManager.Singleton.LocalClientId);
//             }
//             Debug.Log("NetworkManager is null in update!");
//         }
//     }

//     [ServerRpc(RequireOwnership = false)]
//     public void CallToClientConnectedServerRPC(ulong id)
//     {
//         //Gameobject playerObj = NetworkManager.Singleton.ConnectedClients[(int)id].PlayerObject
//         //players.Keys = NetworkManager.Singleton.ConnectedClients.Keys.ToList();
//         //players.Values = NetworkManager.Singleton.ConnectedClients.Values.ToList().Select(go => go.PlayerObject.transform.GetComponent<Player>()).ToList();

//         foreach(KeyValuePair<ulong, NetworkClient> element in NetworkManager.Singleton.ConnectedClients) {
//             Debug.Log("Adding a new client");
//             players.Add(element.Key, element.Value.PlayerObject.transform.GetComponent<MyPlayer>());
//         }
//     }

//     private void OnClientConnectedCallback(ulong clientId)
//     {
//         Debug.Log($"Client connected: {clientId}");

//         Debug.Log("Adding a new client");
//         players.Add(clientId, NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform.GetComponent<MyPlayer>());
//     }

//     private void OnClientDisconnectCallback(ulong clientId)
//     {
//         Debug.Log($"Client disconnected: {clientId}");

//         Debug.Log("Removing a client");
//         players.Remove(clientId);
//     }

//     public void SpawnGlobalObject(GameObject networkPrefab) {
//         if (IsServer) {
//             networkObjectPrefab = networkPrefab;
//             Transform parent = networkObjectPrefab.transform.parent;
//             Vector3 targetPosition = networkObjectPrefab.transform.position;

//             RequestObjectSpawnServerRpc(targetPosition);
//             networkObjectPrefab.transform.parent = parent;

//             if (networkObjectPrefab.transform.childCount < 1) return;

//             foreach (Transform child in networkObjectPrefab.transform) {
//                 if (child.GetComponent<NetworkObject>()) 
//                 {
//                     Debug.Log("Happened");
//                     SpawnGlobalObject(child.gameObject);
//                 }
//             }
//         }
//     }

//     [ServerRpc]
//     void RequestObjectSpawnServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
//     {
//         // Only the server can handle the actual spawning
//         if (IsServer)
//         {
//             // Spawn the object so it's synchronized across all clients
//             NetworkObject targetNetwork = networkObjectPrefab.GetComponent<NetworkObject>();
//             if (!targetNetwork.IsSpawned) targetNetwork.Spawn();

//             networkObjectPrefab.transform.position = position;
//         }
//     }
// }
