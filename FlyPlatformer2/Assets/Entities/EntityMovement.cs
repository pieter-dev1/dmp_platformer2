using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityMovement : MonoBehaviour
{
    private EntityComponents comps;
    [SerializeField]
    private float moveSpeed = 1; 
    [HideInInspector]
    public bool moving = false;
    [HideInInspector]
    public Vector2 direction;
    private readonly float gravity = -80;

    // Start is called before the first frame update
    void Start()
    {
        comps = GetComponent<EntityComponents>();
    }

    private void FixedUpdate()
    {
        if (moving)
            Move(direction);
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, comps.entityStats.groundUp); //rotation
        }
        ApplyGravity();

    }

    public void Move(Vector2 direction)
    {
        if (!comps.entityStats.blocks.Contains(Blocks.MOVE))
        {
            var vector = Vector3.zero;
            var groundUpAxisIndex = comps.entityStats.upAxis.index;
            if (groundUpAxisIndex == 0) //player hit wall from the side
            { 
                vector = Vector3.up * direction.x + Vector3.forward * direction.y;
                if (comps.entityStats.upAxis.positive)
                    vector.y *= -1;
            }
            else if (groundUpAxisIndex == 1) //player hit wall from above/below
                vector = Vector3.right * direction.x + Vector3.forward * direction.y;
            else //player hit wall from the front/back
            { 
                vector = Vector3.right * direction.x + Vector3.up * direction.y;
                if (comps.entityStats.upAxis.positive)
                    vector.y *= -1;
            }

            var movement = (vector) * Time.deltaTime * (moveSpeed * comps.entityStats.moveSpeedRatio);
            transform.rotation = Quaternion.LookRotation(movement, comps.entityStats.groundUp); //rotation

            //apply vertical force (f.e. jumpforce or gravity)
            movement[groundUpAxisIndex] = comps.rigidbody.velocity[groundUpAxisIndex];
            comps.rigidbody.velocity = movement; //moving
        }
    }

    private void ApplyGravity()
    {
        //if (!comps.entityStats.grounded && comps.fauxAttractor != null && comps.fauxAttractor.enabled)
        //{
        //    RaycastHit hit;
        //    var surfaceColl = comps.fauxAttractor.currentSurface.GetComponent<Collider>();
        //    var surfacePoint = Physics.ClosestPoint(transform.position, surfaceColl, surfaceColl.transform.position, surfaceColl.transform.rotation);
        //    var distance = Vector3.Distance(transform.position, surfacePoint);
        //    if (Physics.Raycast(comps.fauxAttractor.cornerDetector.position, surfacePoint - transform.position, out hit))
        //    {
        //        if (hit.normal != comps.entityStats.groundUp)
        //        {
        //            //print($"diff normal: {hit.normal}");
        //            comps.entityStats.groundUp = hit.normal;

        //        }
        //        comps.rigidbody.AddForce(-(surfacePoint).normalized * gravity);
        //    }
        //}
        //else
         comps.rigidbody.AddForce(comps.entityStats.groundUp * gravity);
    }

    public void CancelMovement()
    {
        moving = false;
        direction = Vector2.zero;
        comps.rigidbody.velocity = direction;
    }
}
