using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PixelCrushers.DialogueSystem;
using System.Threading.Tasks;

[System.Serializable]
public class Order {
    public foodStatus status = foodStatus.EMPTY;
    public List<Dish> dishes;
    public List<Allergy> allergies;
    public int wantedDishAmount;

    public Order(List<Dish> dishes, List<Allergy> allergies) {
        wantedDishAmount = dishes.Count;
        this.dishes = dishes;
        this.allergies = allergies;
    }
}


[System.Serializable]
public class AnimalOrder : QuestManager {
    public orderStatus status;
    public Timer orderTime;
    public Order wantedOrder;
    public AwaitServant awaitServant;
    public string questState;
    public string taskID = "BluePigTask";

    public AnimalOrder(string status, int orderDuration, Order wantedOrder, string taskID) {
        this.questState = status;
        orderTime = new Timer(orderDuration); 
        this.wantedOrder = wantedOrder;
        this.taskID = taskID;
        
        awaitServant = new AwaitServant(AwaitMood.HAPPY);
    }

    public void Update() {
       questState = QuestLog.CurrentQuestState(taskID);

       awaitServant.Update();

        // if (orderTime.IsTimePassed()) {
        //     switch (questState) {
        //         case "active": 
        //             questState = "failure";
        //             SetQuestState("failure");
        //             break;
        //         case "success":
        //             questState = "failure";
        //             SetQuestState("failure");
        //             break;
        //         default: break;
        //     }
        // }

        //if (questState is "active") super.OnQuestActiveUpdate();
    }

    public override void OnQuestUnassigned() {
        Debug.Log("Quest is unassigned!");
    }

    // public override void OnQuestActiveUpdate() {

    // }

    public override void OnQuestActive() {
        Debug.Log("Quest is active!");
        if (orderTime.IsTimePassed()) {
            questState = "failure";
            SetQuestState(taskID, "failure");
        }
    }

    public override void OnQuestSuccess() {
        Debug.Log("Quest is success!");
        if (orderTime.IsTimePassed()) {
            questState = "failure";
            SetQuestState(taskID, "failure");
        }
    }

    public void OnTriggerEnter(GameObject other) {
        if (questState is "unassigned" || questState is "active" || questState is "success") {
            var playerItem = other.gameObject.GetComponent<Inventory>()?.slot.item;

            if (playerItem != null && playerItem is Tray) {
                if (FoodIsEqual(((Tray)playerItem).order)) {
                    Debug.Log("Correct food made!: " );
                    SetQuestState(taskID, "success");
                }
                else {
                    SetQuestState(taskID, "active");
                    Debug.Log("Incorrect food made!: "/* + ((Dish)playerItem).food.name*/);
                }
            }
        }
    }
 
    public bool FoodIsEqual(Order curOrder) {
        Order bufOrder = wantedOrder;

        int factor = wantedOrder.wantedDishAmount;

        if (curOrder.dishes.Count == bufOrder.dishes.Count) {
            foreach (Dish dish in curOrder.dishes) {
                if (bufOrder.dishes.Any(x => x.food.name.Equals(dish.food.name))) {
                    factor++;
                    bufOrder.dishes.Remove(dish);
                }
            }

            if (factor == wantedOrder.wantedDishAmount) {
                Debug.Log("You have the right food!");
                return true;
            }  

            Debug.LogError("You have the wrong food..."); 
            return false; 
        }

        Debug.Log("You have the wrong food...(or you dont have it at all!)");
        return false;
    }
}

[System.Serializable]
public class Food {
    public int number;
    public string name;

    public Food(string name, int number) {
        this.name = name;
        this.number = number;
    }
}

[System.Serializable]
public class Allergy {
    public string name;

    public Allergy(string name) {
        this.name = name;
    }
}

public class QuestManager {
    public virtual void OnQuestUnassigned() {

    }

    public virtual void OnQuestActive() {
        
    }

    public virtual void OnQuestSuccess() {
        
    }

    public virtual void OnQuestActiveUpdate() {
        
    }

    public void SetQuestState(string taskID, string tarState) {
        QuestLog.SetQuestState(taskID, tarState);
    }

    // void OnQuestStateChange(string questTitle) {
    //     Debug.Log("Quest changed!: " + questTitle);
    //     switch (questTitle) {
    //         case "unassigned": 
    //             OnQuestUnassigned();
    //             break;
    //         case "active":
    //             OnQuestActive();
    //             break;
    //         case "succes":
    //             OnQuestSuccess();
    //             break;
    //         default: break;
    //     }

    // }
}

public enum AwaitMood {HAPPY, NEUTRAL, ANGRY}

public class AwaitServant {

    AwaitMood mood;
    MoodTimer moodTimer;

    int[] moodSeconds = {20, 40, 60};

    public AwaitServant(AwaitMood mood) 
    {
        this.mood = mood;
        if (moodTimer is null) moodTimer = new MoodTimer(moodSeconds);
    }

    public void Update() {
        moodTimer.UpdateMood();
    }

    AwaitMood CheckMood() {
        return moodTimer.CheckMood();
    }
}

public class MoodTimer : Timer 
{
    public float resultTime = 0;
    int[] times;
    AwaitMood mood;

    public MoodTimer(int[] times) : base(0)
    {
        this.times = times;
    }

    public void UpdateMood() {
        resultTime += Time.deltaTime;

        if (resultTime < times[0]) mood = AwaitMood.HAPPY;
        else if (resultTime > times[0] && resultTime < times[1]) mood = AwaitMood.NEUTRAL;
        else mood = AwaitMood.ANGRY;
    }

    public AwaitMood CheckMood() {
        return mood;
    }
}

public class Timer
{
    private int time; // in milliseconds
    private Task delayTask;
    private bool isTimePassed;

    public Timer(int seconds)
    {
        this.time = seconds;
        this.isTimePassed = false;
    }

    public void StartTimer()
    {
        delayTask = PassTimeAsync();
        Debug.Log("Timer started!");
    }

    public void ResetTimer() {
        this.isTimePassed = false;
    }

    public bool IsTimePassed()
    {
        return isTimePassed;
    }

    async Task PassTimeAsync()
    {
        await Task.Delay(time);
        isTimePassed = true;
    }
}