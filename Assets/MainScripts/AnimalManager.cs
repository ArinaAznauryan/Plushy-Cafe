using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

public class AnimalManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimal();
        //check if the scene is playground, and call InitializeAnimal() once...I DONT KNOW HOW TO DO IT, THINK MONKEY
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateAnimal() {
        GameEventsManager.instance.Tools.GenerateRandomAnimal();
        
    }

    void InitializeAnimal()
    {
        GenerateAnimal();
        DialogueManager.instance.conversationStarted += OnConversationStarted;
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }

    public void OnConversationStarted(Transform actor) {
        if (actor is null) Debug.LogError("Actor is null");
        Debug.Log("Actor: " + actor.name);
        actor.GetComponent<AnimalOrderTrigger>().FinishAwaitingServant();
    }

    public void OnConversationEnded(Transform actor) {Debug.Log("CONVERSATION ENDED");}

}
