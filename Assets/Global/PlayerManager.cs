using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    bool once = true;
    public AudioSource audio;
    public UnityEvent onMainScene;
  

    void InitPlayerComponents() {
        audio = GetComponent<AudioSource>();
    }


    
    //[ServerRpc(RequireOwnership = false)]
    //void InitalizeSubNetworksServerRpc()
    //{
    //    if (!IsServer) {
    //        gameObject.GetComponent<GlobalManager>().enabled = false;
    //    }

    //    GameObject subParent = Resources.Load("Player/PlayerCameraRoot") as GameObject;
    //    GameObject subChild = Resources.Load("Player/Inventory") as GameObject;

    //    subParent = Instantiate(subParent, new Vector3(0, 0, 0), Quaternion.identity);
    //   // GlobalManager.Instance.SpawnGlobalObject(subParent);
    //    subParent.GetComponent<NetworkObject>().Spawn();
    //    // subParent.GetComponent<NetworkObject>().TrySetParent(transform);
    //    subParent.transform.parent = transform;
    //    subParent.transform.localPosition = new Vector3(.2f, 5.5f, 2.48f);

    //    subChild = Instantiate(subChild, new Vector3(0, 0, 0), Quaternion.identity);
    //   // GlobalManager.Instance.SpawnGlobalObject(subChild);
    //    subChild.GetComponent<NetworkObject>().Spawn();
    //    //subChild.GetComponent<NetworkObject>().TrySetParent(subParent.transform);
    //    subChild.transform.parent = transform;
    //    //subChild.transform.position = subParent.transform.position;
    //    subChild.transform.localPosition = new Vector3(-1.63f, 4.18f, 4f);

    //    //subChild.GetComponent<RelativeTransform>().Initialize(subParent.transform);
    //}

    void Start() {

        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
        InputDevice[] devices = { Keyboard.current, Mouse.current };
        playerInput.SwitchCurrentControlScheme("KeyboardMouse", devices);

        transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
    }
}
