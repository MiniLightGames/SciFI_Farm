using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTrigger : MonoBehaviour
{
    public Action <SeedBullet> OnSeed;

    void OnTriggerEnter(Collider other)
    {
        SeedBullet bullet = other.GetComponent<SeedBullet>();
        if (bullet)
        {
            OnSeed?.Invoke(bullet);
        }
    }
}
