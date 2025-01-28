using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MaterialPropertyBlockDemo))]
public class MyHighlighter : MonoBehaviour
{
    GameObject tarObj;

    void Initialize() {
        if (gameObject.GetComponent<MeshRenderer>()) {
            tarObj = gameObject;
            tarObj.AddComponent<MaterialPropertyBlockDemo>();
            return;
        }
        foreach (Transform child in transform) {
            if (child.GetComponent<MeshRenderer>() && child.name is "default") {
                tarObj = child.gameObject;
                tarObj.AddComponent<MaterialPropertyBlockDemo>();
                return;
            }
        }
    }

    void Start() {
        Initialize();

        //tarObj.GetComponent<MaterialPropertyBlockDemo>().ResetMaterialProperties();
    }

    public void Enable() {
        tarObj.GetComponent<MaterialPropertyBlockDemo>().Enable();
    }

    public void Disable() {
        tarObj.GetComponent<MaterialPropertyBlockDemo>().Disable();
    }
}
