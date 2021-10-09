using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoutZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.PLAYER)
        {
            other.transform.position = Challenge.startPoint;
            var meter = other.GetComponent<EntityComponents>().entityStats.meter;
            meter.FillMeter(meter.maxMeter);
        }
    }
}
