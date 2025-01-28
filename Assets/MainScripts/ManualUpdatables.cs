using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManualUpdatables : MonoBehaviour
{
    public UnityEvent events;
    [SerializeField] public UnityAction action;

    public void AddUpdatable(UnityAction newAction) 
    {
        events.AddListener(newAction);
    }

    public void RemoveUpdatable(UnityAction newAction) {
        events.RemoveListener(newAction);
    }

    void Update()
    {
        events.Invoke();
    }
}
