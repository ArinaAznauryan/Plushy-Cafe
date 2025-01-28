using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TablesManager : MonoBehaviour
{
    public List<TableTrigger> tables;
    public GameObject closestTable, counter;


    public TableTrigger FindClosestFreeTable(GameObject guest) {
        List<TableTrigger> freeTables = GetFreeTables();

        int closestIdx = GameEventsManager.instance.Tools.GetClosestObjectIndex<TableTrigger>(ref freeTables, guest);

        return freeTables[closestIdx];
    }

    public bool FreeTablePresent() {
        return GetFreeTables().Count > 0 ? true : false;
    }

    List<TableTrigger> GetFreeTables() {
        return tables.Where(x => x.GetStatus() is tableStatus.FREE).ToList();
    }

    public void GoToClosest(AnimalOrderTrigger guest) {
        closestTable = FindClosestFreeTable(guest.gameObject).gameObject;

        TableTrigger closest = FindClosestFreeTable(guest.gameObject);

        //GameEventsManager.instance.Tools.GoTo(guest.transform, closest.GetSeat().transform);
        guest.WalkTo(closest.GetSeat().transform);
    }  
}
