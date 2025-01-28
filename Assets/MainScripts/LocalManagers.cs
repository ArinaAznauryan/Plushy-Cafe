using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManagers : MonoBehaviour
{
    bool once = true;

    void FixedUpdate() {
        if (once) {
            Debug.Log("SETTING TABLES");
            GameEventsManager.instance.TablesManager = GetComponent<TablesManager>();
            GameEventsManager.instance.personController.SetCamera();
            once = false;
        }
    }
}
