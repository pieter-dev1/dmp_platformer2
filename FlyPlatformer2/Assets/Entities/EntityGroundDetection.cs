using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroundDetection : MonoBehaviour
{
    [SerializeField]
    private EntityComponents comps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.GROUND || other.tag == Tags.WALL)
        {
            comps.entityStats.grounded = true;
            comps.entityJump.jumped = false;
            comps.entityStats.blocks.Remove(Blocks.MOVE);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Tags.GROUND || other.tag == Tags.WALL)
            comps.entityStats.grounded = false;
    }
}
