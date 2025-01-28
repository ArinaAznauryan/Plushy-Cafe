using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

public class StoveHolder : Holder
{
    public byte status = 0; //0 - empty; 1 - full
    
    void Start() {
        type = holder;
        SetEventE(PlaceObject, "E to place a pan or a pot");
    }
}