using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;

public enum orderTakeStatus {UNASSIGNED, STARTED, INPROCESS, FINISHED, DISABLED}
public enum orderStatus {UNASSIGNED, STARTED, INPROCESS, FINISHED, FAILED, DISABLED}
public enum foodStatus {EMPTY, RIGHT, WRONG}

[System.Serializable]
public class AnimalOrderTrigger: MonoBehaviour 
{
    public OrderTaker orderTaker;
    [SerializeField] AnimalOrder order; //BE EQUAL TO ITS SCRIPTABLE FILE;
    public float speed, amplitude;

    void Start() {
        orderTaker = new OrderTaker();
        order = GameEventsManager.instance.Tools.GenerateRandomAnimalOrder();
    }

    public void StartOrder() {
        orderTaker.StartOrder();
    }

    public void TakeOrder() {
        order.orderTime.StartTimer();
        orderTaker.TakeOrder();
    }

    public void OnQuestUnassigned() {
        order.OnQuestUnassigned();
    }

    public void OnQuestActive() {
        order.OnQuestActive();

        Debug.Log("Quest is active!");
        GameEventsManager.instance.TablesManager.GoToClosest(this);
    }

    public void OnQuestSuccess() {
        order.OnQuestSuccess();
    }

    public void AwaitServant() {
        Debug.Log("Waiting the servant!");

        GameEventsManager.instance.Tools.EnableQuestionMark(transform);

        GetComponent<Usable>().enabled = true;
        GetComponent<BarkOnIdle>().enabled = true;
        GetComponent<Animator>().SetTrigger("sit");
    }

    public void FinishAwaitingServant() {
        Debug.Log("Done waiting the servant!");
        
        GameEventsManager.instance.Tools.DisableQuestionMark(transform);
    }

    void Update() {
        order.Update();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            order.OnTriggerEnter(other.gameObject);
        }
    }

    public void WalkToCounter() {
        GameObject counter = GameEventsManager.instance.TablesManager.counter;
        WalkTo(counter.transform);
    }

    public void WalkTo(Transform destination) {
        GameEventsManager.instance.Tools.GoTo(transform, destination);
    }

}

public class OrderTaker {
    public orderTakeStatus status;

    public void StartOrder() {
        status = orderTakeStatus.STARTED;
        Debug.Log("Order started!: " + status);
    }

    public void TakeOrder() {
        status = orderTakeStatus.FINISHED;
        Debug.Log("Order taken!: " + status);
    }

    public void FinishOrder() {
        if (status is orderTakeStatus.FINISHED) {
            status = orderTakeStatus.DISABLED;
        }
    }
}