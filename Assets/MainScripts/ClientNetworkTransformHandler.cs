// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class ClientNetworkTransformHandler : MonoBehaviour
// {
//     bool once = true;


//     private void OnEnable()
//     {
//       //  SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     private void OnDisable()
//     {
//       //  SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     void Update() {
//         if (NetworkManager.Singleton.IsClient && GetComponent<NetworkObject>().IsOwner)
//         {
//             if (once)
//             {
//                 transform.Find("PlayerManagers").GetComponent<GameEventsManager>().Initialize();
//                 once = false;
//             }
//             //Debug.Log("ID:" + NetworkManager.Singleton.LocalClientId);
//             //GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
//             OnSceneLoaded();
//         }
//     }

//     void OnSceneLoaded()
//     {
//         // Request ownership change from the host (server)
//         //RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
//     }
    

//     [ServerRpc(RequireOwnership = false)]
//     void RequestOwnershipServerRpc(ulong clientId)
//     {
//         GetComponent<NetworkObject>().ChangeOwnership(clientId);
//     }
// }
