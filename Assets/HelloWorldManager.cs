
// using System.Collections;
// using UnityEngine;
// using System;
// using System.Collections.Generic;
// using UnityEngine.SceneManagement;

// //public class HelloWorldManager : MonoBehaviour
// //{
// //    private NetworkManager m_NetworkManager;

// //    void Awake()
// //    {
// //        m_NetworkManager = GetComponent<NetworkManager>();
// //    }

// //    void OnGUI()
// //    {
// //        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
// //        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
// //        {
// //            StartButtons();
// //        }
// //        else
// //        {
// //            StatusLabels();

// //            SubmitNewPosition();
// //        }

// //        GUILayout.EndArea();
// //    }

// //    void StartButtons()
// //    {
// //        if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
// //        if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
// //        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
// //    }

// //    void StatusLabels()
// //    {
// //        var mode = m_NetworkManager.IsHost ?
// //            "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

// //        GUILayout.Label("Transport: " +
// //            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
// //        GUILayout.Label("Mode: " + mode);
// //    }

// //    void SubmitNewPosition()
// //    {
// //        if (GUILayout.Button(m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
// //        {
// //            if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
// //            {
// //            foreach (ulong uid in m_NetworkManager.ConnectedClientsIds)
// //                Debug.Log("dunno when this is happening");
// //                   // m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
// //            }
// //            else
// //            {
// //                var playerObject = m_NetworkManager.SpawnManager.GetLocalPlayerObject();
// //            //var player = playerObject.GetComponent<HelloWorldPlayer>();
// //            //player.Move();
// //            Debug.Log("either dont know");
// //            }
// //        }
// //    }
// //}

// public class HelloWorldManager : NetworkBehaviour
// {
//     private NetworkManager m_NetworkManager;

//     void LoadScene() {
//         Debug.Log("It gets calles");
//         //if (IsOwner)
//         //{
//                 Debug.Log("Is a server");
//                 var status = NetworkManager.SceneManager.LoadScene("Playground", LoadSceneMode.Single);
//                 if (status != SceneEventProgressStatus.Started)
//                 {
//                     Debug.LogWarning($"Failed to load \"PlayGround\" " +
//                             $"with a {nameof(SceneEventProgressStatus)}: {status}");
//                 }
//         //}
//     }

//     public void StartScene(ulong name)
//     {
//         LoadScene();
//     }

//     public void Client()
//     {
//         //FindObjectOfType<ClientStatus>().status = "client";
//         //PlayerPrefs.SetString("status", "client");
//         //StartCoroutine(ConnectClient());
//         NetworkManager.Singleton.StartClient();
//         //NetworkManager.Singleton.OnClientConnectedCallback += StartScene;
//         //Destroy(this);
        
//     }

//     public void Server()
//     {
//         //FindObjectOfType<ClientStatus>().status = "server";
//         //PlayerPrefs.SetString("status", "server");
//         NetworkManager.Singleton.StartServer();
//         LoadScene();
//       //  StartCoroutine(LoadSceneAsync("Playground"));
//         //Destroy(this);
//     }

//     // void StartScene(ulong name)
//     // {
//     //    // StartCoroutine(LoadSceneAsync("Playground"));
//     // }

//     private IEnumerator ConnectClient()
//     {
//         // Start the client connection
//         NetworkManager.Singleton.StartClient();

//         // Wait until the client is connected
//         while (!NetworkManager.Singleton.IsClient || !NetworkManager.Singleton.IsConnectedClient)
//         {
//             yield return null; // Wait for the next frame
//         }

//         // Now that the client is connected, proceed
//         Debug.Log("Client successfully connected! Now synchronizing the scene...");
        
//         // At this point, the scene should automatically sync if the host is already in the "Playground" scene
//     }

//     // IEnumerator LoadSceneAsync()
//     // {
//     //     //SceneManager.LoadSceneAsync(levName);
//     //     //AsyncOperation operation = SceneManager.LoadSceneAsync(levName);
//     //     if (!GameObject.Find("Canvas").transform.Find("LoadScreen"))
//     //     {
//     //         Debug.LogError("There's no load screen in the scene, or its name is not \"LoadScreen\"");
//     //     }
//     //     else GameObject.Find("Canvas").transform.Find("LoadScreen").gameObject.SetActive(true);
//     //     while (!NetworkManager.SceneManager.OnSynchronizeComplete && !FindObjectOfType<PlayerManager>().IsOwner)
//     //     {
//     //         // Debug.Log("its not done yet, bruh");
//     //         yield return null;
//     //     }
//     // }
// }
