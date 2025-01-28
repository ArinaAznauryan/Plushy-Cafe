using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

// public class Player 
// {
//     public GameObject player;
//     public PlayerManager manager;

//     public Player(GameObject player, PlayerManager manager) {
//         this.player = player;
//         this.manager = manager;
//     }
// }

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance {get; private set; } 

    public MyTools Tools;
    //public PlayerManager player;
    public Inventory inventory;
    public ManualUpdatables Updatables;
    public FirstPersonController personController;
    public TablesManager TablesManager;
    public MyPlayer player;

    bool once = true;

    void Awake() {
        Initialize();
        
    }

    public void Initialize() {
        //if (player.IsOwner) {
            player = transform.parent.GetComponent<MyPlayer>();//new Player(transform.parent.gameObject, transform.parent.GetComponent<PlayerManager>());
            instance = this;
            Debug.Log("IM THE OWNER");
            inventory = player.transform.GetComponent<Inventory>();

            try {inventory.Initialize(player.GetComponentsInChildren<Slot>()[0]);}
			catch (System.IndexOutOfRangeException e) {}

            personController = player.transform.GetComponent<FirstPersonController>();
        //}
    }

    public void EnableMovement() {personController.EnableMovement();}
    public void DisableMovement() {personController.DisableMovement();}

    public void LimitCameraRotation() {personController.LimitCameraRotation();}
    public void RestoreCameraRotation() {personController.RestoreCameraRotation();}

}
