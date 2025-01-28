using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum tableStatus {FREE, TAKEN, DIRTY}

public class Table : MonoBehaviour {
    List<Transform> seats;
    TrayHolder trayHolder;
    GameObject firstSeat;
    tableStatus status = tableStatus.FREE;

    AnimalGuest animalGuest;

    void Start() {
        seats = transform.Find("seats").GetComponentsInChildren<Transform>().ToList();
        seats.RemoveAt(0);

        trayHolder = transform.Find("trayHolder").GetComponent<TrayHolder>();

        firstSeat = seats[0].gameObject;
    }

    public void SetAnimal(AnimalOrderTrigger guest) {
        animalGuest.animal = guest;
    }

    void SetAnimalGroup(AnimalGuestGroup guests) {

    }

    public GameObject GetSeat() {
        return firstSeat;
    }

    public tableStatus GetStatus() {return status;}
}