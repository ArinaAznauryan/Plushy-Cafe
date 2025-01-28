using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleRotating : MonoBehaviour
{
    float angle = 0f;
    public float multiplier, speed;

    void Update()
    {
        angle += 0.1f*speed;
        transform.position += new Vector3(0f, Mathf.Sin(angle)*multiplier, 0f);
    }
}
