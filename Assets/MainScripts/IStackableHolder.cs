using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class IStackableHolder : Interactable {
    public GameObject target;

    // public void Initialize(GameObject target) {
    //     this.target = target;
    // }

    void Start() {
        base.Start();
        Debug.Log("In Istackable burger");
        InitInteractions();
    }

    void Update() {

        if (CheckIfFocused()) {
            OnFocused();
        }
        
    }

    public void InitInteractions() {
        Debug.Log("Setting event E burger");
        RemoveAllEventsE();
        SetEventE(OnStacking, "E to put ingredient");
    }

    public void InitInteractions1() {
        SetToPickable();
    }


    public virtual void OnFocused() {}

    public virtual void OnStacking() {}

    

    bool CheckIfFocused() {
        if (GameEventsManager.instance.Tools.ObjectOnFocus()?.name == target?.name) return true;
        return false;
    }

    // void InvokeMethod(string methodName)
    // {
    //     var method = GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
    //     if (method != null)
    //     {
    //         method.Invoke(this, null);
    //     }
    // }
}
