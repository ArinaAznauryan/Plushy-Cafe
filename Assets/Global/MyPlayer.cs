using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
public class MyPlayer : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject playerManagers;

    public FirstPersonController movementController;
    
    void Awake() {
        playerManager = GetComponent<PlayerManager>();
        playerManagers = transform.Find("PlayerManagers").gameObject;
        movementController = GetComponent<FirstPersonController>();
    }
    
}
