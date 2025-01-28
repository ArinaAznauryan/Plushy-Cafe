
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//public class HelloWorldManager : MonoBehaviour
//{
//    private NetworkManager m_NetworkManager;

//    void Awake()
//    {
//        m_NetworkManager = GetComponent<NetworkManager>();
//    }

//    void OnGUI()
//    {
//        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
//        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
//        {
//            StartButtons();
//        }
//        else
//        {
//            StatusLabels();

//            SubmitNewPosition();
//        }

//        GUILayout.EndArea();
//    }

//    void StartButtons()
//    {
//        if (GUILayout.Button("Host")) m_NetworkManager.StartHost();
//        if (GUILayout.Button("Client")) m_NetworkManager.StartClient();
//        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
//    }

//    void StatusLabels()
//    {
//        var mode = m_NetworkManager.IsHost ?
//            "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

//        GUILayout.Label("Transport: " +
//            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
//        GUILayout.Label("Mode: " + mode);
//    }

//    void SubmitNewPosition()
//    {
//        if (GUILayout.Button(m_NetworkManager.IsServer ? "Move" : "Request Position Change"))
//        {
//            if (m_NetworkManager.IsServer && !m_NetworkManager.IsClient)
//            {
//            foreach (ulong uid in m_NetworkManager.ConnectedClientsIds)
//                Debug.Log("dunno when this is happening");
//                   // m_NetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
//            }
//            else
//            {
//                var playerObject = m_NetworkManager.SpawnManager.GetLocalPlayerObject();
//            //var player = playerObject.GetComponent<HelloWorldPlayer>();
//            //player.Move();
//            Debug.Log("either dont know");
//            }
//        }
//    }
//}

public class HelloWorldManager : MonoBehaviour
{


    public void LoadScene() {
        SceneManager.LoadSceneAsync("PlayGround");
    }

    //IEnumerator LoadSceneAsync()
    //{
    //    SceneManager.LoadSceneAsync(levName);
    //    AsyncOperation operation = SceneManager.LoadSceneAsync("PlayGround");
    //    if (!GameObject.Find("Canvas").transform.Find("LoadScreen"))
    //    {
    //        Debug.LogError("There's no load screen in the scene, or its name is not \"LoadScreen\"");
    //    }
    //    else GameObject.Find("Canvas").transform.Find("LoadScreen").gameObject.SetActive(true);
    //    while (!operation)
    //    {
    //         Debug.Log("its not done yet, bruh");
    //        yield return null;
    //    }
    //}
}
