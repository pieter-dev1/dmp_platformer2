using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] zonesToDisable = new GameObject[0];

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER))
        {
            Challenge.startPoint = transform.position;
            var meter = other.GetComponent<EntityComponents>().entityStats.meter;
            meter.FillMeter(meter.maxMeter);
            foreach (var zone in zonesToDisable)
                zone.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
