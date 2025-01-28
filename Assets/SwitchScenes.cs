using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public Transform spawn, player;
    void Update() {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            if (SceneManager.GetActiveScene().name is "Kitchen1") SceneManager.LoadScene("real");
            else SceneManager.LoadScene("Kitchen1");
        }
        
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Wtf?");
            player.position = spawn.position;
        }
    }
}
