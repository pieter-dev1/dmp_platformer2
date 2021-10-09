using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaFill : MonoBehaviour
{
    [SerializeField]
    private float amount;
    [SerializeField]
    private bool staysAlive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(Tags.PLAYER)) {
            other.GetComponent<EntityStats>().meter.FillMeter(amount);
            if (!staysAlive)
                Destroy(gameObject);
        }

    }
}
