using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;
using PixelCrushers;
using System.Linq;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineAction {
    [SerializeField] public PlayableDirector director;
    [SerializeField] public string[] names;
    [SerializeField] public List<Timeline> timelines = new List<Timeline>();

    public TimelineAction(PlayableDirector director, string[] names) {
        this.director = director;
        this.names = names;
        timelines = new List<Timeline>(names.Length);
        timelines = FindTimelineAssets();
    }
    
    public void Play(string name) {
        if (timelines.Count < 1) Debug.LogError("Timelines is empty!");
        if (name == null) Debug.LogError("Name is null!");
        TimelineAsset asset = timelines.Find(x => x.name.Equals(name)).asset;
        if (asset) {
            SetTimeline(asset);
            director.Play();
        }
    }

    void SetTimeline(TimelineAsset tarAsset) {
        director.playableAsset = tarAsset;
        // rebuild for runtime playing
        director.RebuildGraph();
        director.time = 0.0;
    }

    List<Timeline> FindTimelineAssets() {
        List<TimelineAsset> _timelineAssets = Resources.FindObjectsOfTypeAll<TimelineAsset>().ToList();
        Debug.Log("Timeline: " + _timelineAssets + _timelineAssets.Count);

        // foreach (TimelineAsset asset in _timelineAssets) {
        //     Debug.Log("Searching through assets: " + asset);
        //     foreach (TrackAsset track in asset.GetOutputTracks()) {
        //         Debug.Log("Searching through tracks: " + track);
        //         if (director.GetGenericBinding(track) != null) Debug.Log("File is in the director!: " + track);
        //     }
        // }

        List<TimelineAsset> configuredTimelines = new List<TimelineAsset>();

        foreach (var timelineAsset in _timelineAssets)
        {
            bool hasBinding = false;
            Debug.Log("Asset: " + timelineAsset);

            // Iterate through each track in the timelineAsset
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                Debug.Log("Track: " + track);
                // Check if the director has a binding for this track
                if (director.GetGenericBinding(track) != null)
                {
                    Debug.Log("Is in the director!: " + track);
                    hasBinding = true;
                    break;  // No need to check further tracks for this timeline asset
                }
            }

            // If we found at least one track with a binding, add it to the list
            if (hasBinding)
            {
                Debug.Log("Has binding");
                configuredTimelines.Add(timelineAsset);
            }
        }


        // List<TimelineAsset> configuredTimelines = _timelineAssets
        //     .Where(timelineAsset => timelineAsset.GetOutputTracks()
        //         .Any(track => director.GetGenericBinding(track) != null)).ToList();
        //     //.ToList();
        // timelines = new List<Timeline>(configuredTimelines.Count);
        
        Debug.Log("Names length: " + names.Length);
        Debug.Log("ConfiguredTimelines length: " + configuredTimelines.Count);

        for (int i = 0; i < configuredTimelines.Count; i++) {
            timelines.Add(new Timeline(configuredTimelines[i], names[i]));
        }
        
        Debug.Log("Length: " + timelines.Count);
        return timelines;
    }
}

[System.Serializable] public class Timeline {
    [SerializeField] public TimelineAsset asset;
    [SerializeField] public string name;

    public Timeline(TimelineAsset asset, string name) {
        this.asset = asset;
        this.name = name;
    }
}

public class Interactable : MonoBehaviour {
    [SerializeField] protected TimelineAction timeline;
    protected GrabbableItem grabbable;
    protected Usable usable;
    protected Collider collider;
    protected Animator animator;
    protected bool lockedIn = false;
    protected Holder holder;

    protected void SetToPickable() {
        RemoveAllEventsE();
        usable.AddEventE(GameEventsManager.instance.inventory.slot.SetItem, gameObject, "E to take");
    }

    protected void InitTmeline(string[] timelineNames) {
        timeline = new TimelineAction(GetComponent<PlayableDirector>(), timelineNames);
    }

    protected void InitLocalData(Holder holder) {
        this.holder = holder;
    }

    public Holder GetHolder() {
        return holder;
    }

    protected void Awake() {
        grabbable = GetComponent<GrabbableItem>();
        usable = GetComponent<Usable>();
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        holder = GetComponent<Holder>();
    }

    protected void Start() {
        Debug.Log("On start: " + gameObject.name);
        InitAutoDefaultInteractions();
    }

    public void SetEventE(UnityAction listener, string message = "") {usable.AddEventE(listener, gameObject, message);}
    public void SetEventF(UnityAction listener, string message = "") {usable.AddEventF(listener, gameObject, message);}
    public void SetEventR(UnityAction listener, string message = "") {usable.AddEventR(listener, gameObject, message);}

    public void RemoveAllEventsE() {
        usable.ResetEventE();
    }
    public void RemoveAllEventsF() {
        usable.ResetEventF();
    }
    public void RemoveAllEventsR() {
        usable.ResetEventR();
    }

    void PutBack() {
        Debug.Log("Trying to put back:" + gameObject.name);
        KitchenTool tool = GameEventsManager.instance.inventory.slot.item as KitchenTool;
        if (tool != null && tool.IsFree()) {
            Debug.Log("Not null and is free: " + gameObject.name);
            GameEventsManager.instance.inventory.slot.RemoveItem();
        }
    }

    public void EnableInteractions() {
        usable.enabled = true;
    }

    public void DisableInteractions() {
        usable.enabled = false;
    }
    bool IsSpawner() {
        return grabbable != null && grabbable.eternal;
    }

    void InitAutoDefaultInteractions() {
        if (IsSpawner()) {
            Debug.Log("Adding putting back event: " + gameObject.name);
            SetEventF(PutBack, "F to put back");
        }
    }

    public void ChangeState(int state) {    //0 - primary, 1 - changed 
        KitchenTool tool = grabbable.item as KitchenTool;
        KitchenToolState result = state is 0 ? KitchenToolState.PRIMARY : KitchenToolState.CHANGED;
        if (tool != null) tool.SetState(result);
    }

    protected bool IsPlaced(Holder holder) {
        if (grabbable?.IsUnder() != null) {
            Holder holderType = grabbable.IsUnder();
            return holderType.GetType() == holder.GetType();
        }
        return false;
    }

    public void ResetAnimator() {
        animator.applyRootMotion = false;
    }

}