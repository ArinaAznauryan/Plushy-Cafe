using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeTransform : MonoBehaviour
{
    public Transform target;

    public void Initialize(Transform target) {
        this.target = target;
    }

    void FixedUpdate() {
        if (!target) return;
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
    }

    public void Reset() {
        this.target = null;
    }
}
