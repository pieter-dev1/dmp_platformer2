using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FauxAttractor : MonoBehaviour
{
    private EntityComponents comps;
    public bool onWall = false;
    [SerializeField]
    private Transform cornerDetector;
    [SerializeField]
    private Transform mainGround;
    [SerializeField]
    public Transform currentSurface { get; private set; }
    private RaycastHit newHit;

    private void Start()
    {
        comps = GetComponent<EntityComponents>();
        comps.rigidbody.useGravity = false;
        enabled = false;
        currentSurface = mainGround;
    }

    private void Update()
    {
        var pos = transform.position;
        var upAxisIndex = comps.entityStats.upAxis.index;
        var verticalRotationAxis = upAxisIndex == 0 ? 2 : upAxisIndex - 1;
        var raycastStart = pos;
        raycastStart[upAxisIndex] -= 0.5f;
        Debug.DrawRay(raycastStart, Quaternion.Euler(transform.right * 30) * (transform.forward * 3), Color.red);
        if (Physics.Raycast(raycastStart, Quaternion.Euler(transform.right * 30) * transform.forward, out newHit, 1f) && newHit.transform.gameObject != currentSurface.gameObject)
        {
            if (newHit.transform.tag.Equals(Tags.GROUND))
            {
                onWall = false;
                currentSurface = newHit.transform;
                comps.entityStats.groundUp = newHit.normal;
                var upAxis = comps.entityStats.upAxis.index;
                comps.entityStats.horAxis = MoveAxis.AXES.First(x => x != upAxis);
                comps.entityStats.verAxis = MoveAxis.AXES.Last(x => x != upAxis);
            }
            else if (newHit.transform.tag.Equals(Tags.WALL))
            {
                onWall = true;
                currentSurface = newHit.transform;
                comps.entityStats.groundUp = newHit.normal;
                var upAxis = comps.entityStats.upAxis.index;
                comps.entityStats.horAxis = MoveAxis.AXES.First(x => x != upAxis);
                comps.entityStats.verAxis = MoveAxis.AXES.Last(x => x != upAxis);
            }
        }
        //else
        //{
        //    raycastStart = cornerDetector.position;
        //    var forwardAxis = MoveAxis.AXES.First(x => transform.forward[x] != 0);
        //    Debug.DrawRay(raycastStart, Quaternion.Euler(transform.right * 50) * -transform.up * 1f, Color.magenta);    // show floor check ray
        //    if (Physics.Raycast(raycastStart, Quaternion.Euler(transform.right * 50) * -transform.up, out newHit, 1f) && newHit.transform.gameObject == currentSurface.gameObject
        //        && newHit.normal != comps.entityStats.groundUp)
        //    {
        //        Debug.Log("HitNormal " + newHit.transform.gameObject);
        //        currentSurface = newHit.transform;
        //        comps.entityStats.groundUp = newHit.normal;
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag.Equals(Tags.GROUND) || collision.collider.tag.Equals(Tags.WALL))
        {
            var rot = new Vector3(0, 0, 0);
            if (comps.entityStats.upAxis.index != MoveAxis.VERTICAL)
                rot[comps.entityStats.horAxis] = -90;
            transform.rotation = Quaternion.LookRotation(rot, comps.entityStats.groundUp); //rotation
        }
    }

    public void CancelCustomGravity(bool disableAttractor = true)
    {
        currentSurface = mainGround;
        if (comps.entityStats.groundUp != new Vector3(0, 1, 0))
        {
            comps.entityStats.groundUp = new Vector3(0, 1, 0);
        }
        enabled = !disableAttractor;
        onWall = false;
    }
}
