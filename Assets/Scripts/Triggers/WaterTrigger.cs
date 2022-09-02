using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public Action OnWater;

    void OnTriggerEnter(Collider other)
    {
        WaterBall ball = other.GetComponent<WaterBall>();
        if (ball)
        {
            OnWater?.Invoke();
        }
    }
}
