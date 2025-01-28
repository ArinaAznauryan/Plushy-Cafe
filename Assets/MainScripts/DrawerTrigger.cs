using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Zenject;

enum drawerStatus {disabled, rightDoorOpen, leftDoorOpen, inProcess, closed}

public class DrawerTrigger : Interactable
{
    Transform looker;
    MyPlayer player;
    [SerializeField] float speed;
    public bool triggerRightLooker, triggerLeftLooker;
    GameObject rightDrawer, leftDrawer;
    GameObject cameraRoot;

    drawerStatus status = drawerStatus.disabled;
    
    void Start()
    {
        player = GameEventsManager.instance.player;

        looker = transform.Find("looker");
        rightDrawer = transform.Find("rightDoor").gameObject;
        leftDrawer = transform.Find("leftDoor").gameObject;

        cameraRoot = GameObject.FindWithTag("CinemachineTarget");
        //player = GameEventsManager.instance.player.player.transform;

        InitializeEvents();

        //player.position = new Vector3(looker.position.x, player.position.y, looker.position.z);
    }

    void InitializeEvents() {

        rightDrawer.GetComponent<Usable>().AddEventE(TriggerRightLooker, gameObject, "E to open");
        rightDrawer.GetComponent<Usable>().AddEventF(UntriggerRightLooker, gameObject, "F to close");

        leftDrawer.GetComponent<Usable>().AddEventE(TriggerLeftLooker, gameObject, "E to open;");
        leftDrawer.GetComponent<Usable>().AddEventF(UntriggerLeftLooker, gameObject, "F to close");
    }

    void Update() {
        if (status is drawerStatus.rightDoorOpen) LookIntoDrawer();
        else if (status is drawerStatus.leftDoorOpen) LookIntoDrawer();
        else if (status is drawerStatus.closed) GetOutOfDrawer();
    }

    void LookIntoDrawer() {
        
        //GameEventsManager.instance.LimitCameraRotation();

        player.transform.position = Vector3.MoveTowards(player.transform.position, looker.position, speed);

        if (Vector3.Distance(cameraRoot.transform.position, looker.position) > .2f) {
            cameraRoot.transform.parent = null;
            cameraRoot.transform.position = Vector3.MoveTowards(
                cameraRoot.transform.position, 
                looker.position, 
                speed
            );
        }

        else {
            cameraRoot.transform.parent = player.transform;
            status = drawerStatus.inProcess;
        }
    }

    // void LookIntoLeftDrawer() {
    //     GameEventsManager.instance.LimitCameraRotation();

    //     if (Vector3.Distance(cameraRoot.transform.position, leftLooker.position) > .2f) 
    //         cameraRoot.transform.position = Vector3.MoveTowards(
    //             cameraRoot.transform.position, 
    //             leftLooker.position, 
    //             speed
    //         );

    //     else status = drawerStatus.inProcess;
    // }

    void GetOutOfDrawer() {
        //primareCameraPos -> Vector3(.27f, 7f, 2.48f);
        if (Vector3.Distance(cameraRoot.transform.localPosition, new Vector3(.27f, 7f, 2.48f)) > .2f) {

            cameraRoot.transform.localPosition = Vector3.MoveTowards(
                cameraRoot.transform.localPosition, 
                new Vector3(.27f, 7f, 2.48f), 
                speed
            );

            // if (status is drawerStatus.rightDoorOpen) GetComponent<Animation>().Play("rightDoorClose");
            // else if (status is drawerStatus.leftDoorOpen) GetComponent<Animation>().Play("leftDoorClose");
        
            GameEventsManager.instance.EnableMovement();
        }

        else status = drawerStatus.disabled;
    }

    public void TriggerRight() {

        GameEventsManager.instance.DisableMovement();
        GetComponent<Animation>().Play("rightDoorOpen");
    }

    public void TriggerLeft() {

        GameEventsManager.instance.DisableMovement();
        GetComponent<Animation>().Play("leftDoorOpen");
    }

    public void TriggerRightLooker() {
        TriggerRight();
        status = drawerStatus.rightDoorOpen;
    }

    public void TriggerLeftLooker() {
        TriggerLeft();
        status = drawerStatus.leftDoorOpen;
    }

    public void UntriggerRightLooker() {
        GetComponent<Animation>().Play("rightDoorClose");
        status = drawerStatus.closed;
    }

    public void UntriggerLeftLooker() {
        GetComponent<Animation>().Play("leftDoorClose");
        status = drawerStatus.closed;
    }
}
