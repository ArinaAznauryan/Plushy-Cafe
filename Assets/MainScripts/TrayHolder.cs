using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class TrayHolder : Holder
{
    public byte status = 0; //0 - empty; 1 - full
    

    void Start() {
        type = holder;
        Debug.Log("Adding the place object event?: " + usable.gameObject.name); //CHECK THIS RIGHT NOW, THE TRAY HOLDER'S START() DOESN'T RUN OR THE USABLE "R" EVENT RESETS SOMEWHERE ELSE
        SetEventR(PlaceObject, "R to put a tray");
    }
}
