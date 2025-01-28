// using System.Collections;
// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;
// using PixelCrushers.DialogueSystem;
// using UnityEngine.Events;
// using PixelCrushers;
// using System.Linq;
// using UnityEngine.Timeline;
// using UnityEngine.Playables;

// public class NetworkedInteractable : NetworkBehaviour {
//     [SerializeField] protected TimelineAction timeline;
//     protected GrabbableItem grabbable;
//     protected Usable usable;
//     protected Collider collider;
//     protected Animator animator;
//     protected bool lockedIn = false;
//     protected Holder holder;

//     protected void SetToPickable() {
//         RemoveAllEventsE();
//         usable.AddEventE(GameEventsManager.instance.inventory.slot.SetItem, gameObject, "E to take");
//     }

//     protected void InitTmeline(string[] timelineNames) {
//         timeline = new TimelineAction(GetComponent<PlayableDirector>(), timelineNames);
//     }

//     protected void InitLocalData(Holder holder) {
//         this.holder = holder;
//     }

//     public Holder GetHolder() {
//         return holder;
//     }

//     protected void Awake() {
//         grabbable = GetComponent<GrabbableItem>();
//         usable = GetComponent<Usable>();
//         collider = GetComponent<Collider>();
//         animator = GetComponent<Animator>();
//         holder = GetComponent<Holder>();
//     }

//     protected void Start() {
//         Debug.Log("On start: " + gameObject.name);
//         InitAutoDefaultInteractions();
//     }

//     public void SetEventE(UnityAction listener, string message = "") {usable.AddEventE(listener, gameObject, message);}
//     public void SetEventF(UnityAction listener, string message = "") {usable.AddEventF(listener, gameObject, message);}
//     public void SetEventR(UnityAction listener, string message = "") {usable.AddEventR(listener, gameObject, message);}

//     public void RemoveAllEventsE() {
//         usable.ResetEventE();
//     }
//     public void RemoveAllEventsF() {
//         usable.ResetEventF();
//     }
//     public void RemoveAllEventsR() {
//         usable.ResetEventR();
//     }

//     void PutBack() {
//         Debug.Log("Trying to put back:" + gameObject.name);
//         KitchenTool tool = GameEventsManager.instance.inventory.slot.item as KitchenTool;
//         if (tool != null && tool.IsFree()) {
//             Debug.Log("Not null and is free: " + gameObject.name);
//             GameEventsManager.instance.inventory.slot.RemoveItem();
//         }
//     }

//     public void EnableInteractions() {
//         usable.enabled = true;
//     }

//     public void DisableInteractions() {
//         usable.enabled = false;
//     }
//     bool IsSpawner() {
//         return grabbable != null && grabbable.eternal;
//     }

//     void InitAutoDefaultInteractions() {
//         if (IsSpawner()) {
//             Debug.Log("Adding putting back event: " + gameObject.name);
//             SetEventF(PutBack, "F to put back");
//         }
//     }

//     public void ChangeState(int state) {    //0 - primary, 1 - changed 
//         KitchenTool tool = grabbable.item as KitchenTool;
//         KitchenToolState result = state is 0 ? KitchenToolState.PRIMARY : KitchenToolState.CHANGED;
//         if (tool != null) tool.SetState(result);
//     }

//     protected bool IsPlaced(Holder holder) {
//         if (grabbable?.IsUnder() != null) {
//             Holder holderType = grabbable.IsUnder();
//             return holderType.GetType() == holder.GetType();
//         }
//         return false;
//     }

//     public void ResetAnimator() {
//         animator.applyRootMotion = false;
//     }

// }