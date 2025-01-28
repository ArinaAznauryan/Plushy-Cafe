// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System;
using System.Reflection;
using Zenject;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{



    public delegate void UsableDelegate(Usable usable);

    [Serializable]
    public class OverrideUseMessages {
        public string eMessage, fMessage, rMessage;
    }
    /// <summary>
    /// This component indicates that the game object is usable. This component works in
    /// conjunction with the Selector component. If you leave overrideName blank but there
    /// is an OverrideActorName component on the same object, this component will use
    /// the name specified in OverrideActorName.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class Usable : MonoBehaviour
    {

        int GetNonPersistentEventCount(UnityEvent tarEvent)
        {
            if (tarEvent == null)
                return 0;

            // Access the "m_Calls" field using reflection
            FieldInfo callsField = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
            var invokableList = callsField?.GetValue(tarEvent);

            // Access the "m_RuntimeCalls" list in the invokable list
            FieldInfo runtimeCallsField = invokableList?.GetType().GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = runtimeCallsField?.GetValue(invokableList) as System.Collections.IList;

            // Return the count of runtime calls
            return runtimeCalls?.Count ?? 0;
        }

        string GetNonPersistentMethodName(UnityEvent unityEvent, int i)
        {
            var field = typeof(UnityEvent).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field is null) return null;
            var calls = field.GetValue(unityEvent);

            var persistentCalls = calls.GetType().GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = persistentCalls.GetValue(calls) as System.Collections.IList;

            if (runtimeCalls != null)
            {
                if (runtimeCalls.Count >= i)
                    return (string)runtimeCalls[i].GetType().GetField("m_MethodName", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(runtimeCalls[i]);
                else Debug.LogError("The index is bigger than the amount of methods in the unity event!");
                return null;
            }
            Debug.LogError("The non-persistent methods are null!");
            return null;
        }

        //[Inject] MyTools tools;

            /// <summary>
            /// (Optional) Overrides the name shown by the Selector.
            /// </summary>
        public string overrideName;

        public bool highlight = true;
        public UnityEvent eBuf, fBuf, rBuf;

        public UnityEvent manualE, manualF, manualR;

        /// <summary>
        /// (Optional) Overrides the use message shown by the Selector.
        /// </summary>
        [SerializeField] public OverrideUseMessages overrideUseMessages;

        /// <summary>
        /// The max distance at which the object can be used.
        /// </summary>
        public float maxUseDistance = 5f;

        List<GameObject> eventSendersE = new List<GameObject>();
        List<GameObject> eventSendersF = new List<GameObject>();
        List<GameObject> eventSendersR = new List<GameObject>();

        [Serializable]
        public class Event {
            public UsableEvent eventE = new UsableEvent(new UnityEvent(), "default E message");
            public UsableEvent eventF = new UsableEvent(new UnityEvent(), "default F message");
            public UsableEvent eventR = new UsableEvent(new UnityEvent(), "default R message");
        }

        public class UsableEvent
        {
            public UnityEvent Event;
            public string Message;

            public UsableEvent(UnityEvent Event, string Message)
            {
                this.Event = Event;
                this.Message = Message;
            }
        }

        [Serializable]
        public class UsableEvents
        {
            /// <summary>
            /// Invoked when a Selector or ProximitySelector selects this Usable.
            /// </summary>
            public UnityEvent onSelect = new UnityEvent();

            /// <summary>
            /// Invoked when a Selector or ProximitySelector deselects this Usable.
            /// </summary>
            public UnityEvent onDeselect = new UnityEvent();

            /// <summary>
            /// Invoked when a Selector or ProximitySelector uses this Usable.
            /// </summary>
            public Event onUse = new Event();
        }

        public UsableEvents events;

        public event UsableDelegate disabled = delegate { };

        protected virtual void OnDisable()
        {
            disabled(this);
        }

        public virtual void Start()
        {
            if (highlight)
            {
                if (GetComponent<MyHighlighter>() is null) gameObject.AddComponent<MyHighlighter>();
            }

            //if (manualE != null) events.onUse.eventE.Event = manualE;
            //if (manualF != null) events.onUse.eventF.Event = manualF;
            //if (manualR != null) events.onUse.eventR.Event = manualR;
        }
        
        public bool ListenerAlreadyRegistered(UnityEvent tarEvent, UnityAction action, GameObject sender)
        {
            List<GameObject> senders = new List<GameObject>();

            if (tarEvent == events.onUse.eventE.Event) senders = eventSendersE;
            else if (tarEvent == events.onUse.eventF.Event) senders = eventSendersF;
            else if (tarEvent == events.onUse.eventR.Event) senders = eventSendersR;
            else Debug.LogError("The target event is not 'E', 'F' nor 'R' interactable");


            for (int i = 0; i < GetNonPersistentEventCount(tarEvent); i++)
            {
                if (GetNonPersistentMethodName(tarEvent, i) == action.Method.Name) 
                {
                    Debug.Log("E event name: " + GetNonPersistentMethodName(tarEvent, i) + "\n method name: " + action.Method.Name + sender.name);
                    if (senders.Contains(sender))
                    {
                        Debug.Log("It's already registered: " + action.Method.Name);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the name of the override, including parsing if it contains a [lua]
        /// or [var] tag.
        /// </summary>
        /// <returns>The override name.</returns>
        public virtual string GetName()
        {
            if (string.IsNullOrEmpty(overrideName))
            {
                return DialogueActor.GetActorName(transform);
            }
            else if (overrideName.Contains("[lua") || overrideName.Contains("[var"))
            {
                return DialogueManager.GetLocalizedText(FormattedText.Parse(overrideName, DialogueManager.masterDatabase.emphasisSettings).text);
            }
            else
            {
                return DialogueManager.GetLocalizedText(overrideName);
            }
        }

        void InvokeEvents(Event current) {
            UpdateMessages(current);
            current.eventE.Event.Invoke();
            current.eventF.Event.Invoke();
            current.eventR.Event.Invoke();
        }

        public void UpdateMessage(string eventType, string message)
        {
            switch (eventType)
            {
                case "e":
                    Debug.Log("MESSAGING THE E:" + gameObject.name);
                    overrideUseMessages.eMessage = message;
                    break;
                case "f":
                    overrideUseMessages.fMessage = message;
                    break;
                case "r":
                    overrideUseMessages.rMessage = message;
                    break;
                default: break;
            }
        }

        public void ResetMessageE() { overrideUseMessages.eMessage = "";  }
        public void ResetMessageF() { overrideUseMessages.fMessage = "";  }
        public void ResetMessageR() { overrideUseMessages.rMessage = ""; }

        public void UpdateMessages(Event current)
        {
            overrideUseMessages.eMessage = current.eventE.Message;
            overrideUseMessages.fMessage = current.eventF.Message;
            overrideUseMessages.rMessage = current.eventR.Message;
        }

        public void PlaySound(string name)
        {
            MyPlayer player = GameEventsManager.instance.player;
            GameEventsManager.instance.Tools.PlaySound(player.playerManager.audio, name);
        }

        public void AddEventE(UnityAction action, GameObject sender, string message = "") {
            if (!ListenerAlreadyRegistered(events.onUse.eventE.Event, action, sender))
            {
                eBuf = events.onUse.eventE.Event;
                Debug.Log("Adding event E: " + sender.gameObject + action.Method.Name);
                eventSendersE.Add(sender);
                //UnityEditor.Events.UnityEventTools.AddPersistentListener(events.onUse.eventE.Event, action); if (message != "") UpdateMessage("e", message);
                events.onUse.eventE.Event.AddListener(action); if (message != "") UpdateMessage("e", message);
            }
        }
        public void AddEventF(UnityAction action, GameObject sender, string message = "") {
            if (!ListenerAlreadyRegistered(events.onUse.eventF.Event, action, sender))
            {
                fBuf = events.onUse.eventF.Event;
                Debug.Log("Adding event F: " + sender.gameObject + action.Method.Name);
                eventSendersF.Add(sender);
                //UnityEditor.Events.UnityEventTools.AddPersistentListener(events.onUse.eventF.Event, action); if (message != "") UpdateMessage("f", message);
                events.onUse.eventF.Event.AddListener(action); if (message != "") UpdateMessage("f", message);
            }
        }
        public void AddEventR(UnityAction action, GameObject sender, string message = "") {
            if (!ListenerAlreadyRegistered(events.onUse.eventR.Event, action, sender))
            {
                rBuf = events.onUse.eventR.Event;
                Debug.Log("Adding event R: " + sender.gameObject + action.Method.Name);
                eventSendersR.Add(sender);
                //UnityEditor.Events.UnityEventTools.AddPersistentListener(events.onUse.eventR.Event, action); if (message != "") UpdateMessage("r", message);
                events.onUse.eventR.Event.AddListener(action); if (message != "") UpdateMessage("r", message);
            }
        }

        public void RemoveEventE(UnityAction action) {events.onUse.eventE.Event.RemoveListener(action);}
        public void RemoveEventF(UnityAction action) {events.onUse.eventF.Event.RemoveListener(action);}
        public void RemoveEventR(UnityAction action) {events.onUse.eventR.Event.RemoveListener(action);}

        public void ResetEventE() { events.onUse.eventE.Event.RemoveAllListeners(); events.onUse.eventE.Event = new UnityEvent(); ResetMessageE(); }
        public void ResetEventF() { events.onUse.eventF.Event.RemoveAllListeners(); events.onUse.eventF.Event = new UnityEvent(); ResetMessageF(); }
        public void ResetEventR() { events.onUse.eventR.Event.RemoveAllListeners(); events.onUse.eventR.Event = new UnityEvent(); ResetMessageR(); }

        public virtual void OnSelectUsable()
        {
            if (events != null && events.onSelect != null) {
                gameObject.GetComponent<MyHighlighter>()?.Enable();
                events.onSelect.Invoke();
            }
        }

        public virtual void OnDeselectUsable()
        {
            if (events != null && events.onDeselect != null) {
                gameObject.GetComponent<MyHighlighter>()?.Disable();
                events.onDeselect.Invoke();
            }
        }

        public virtual void OnUseUsable(KeyCode useKey)
        {
            if (events != null && events.onUse != null) RunEvent(useKey);
        }

        void RunEvent(KeyCode useKey) {
            switch (useKey) {
                case KeyCode.E:
                    events.onUse.eventE.Event.Invoke();
                    break;
                case KeyCode.F:
                    events.onUse.eventF.Event.Invoke();
                    break;
                case KeyCode.R:
                    events.onUse.eventR.Event.Invoke();
                    break;
                default: break;
            }
        }

    }

     
}
