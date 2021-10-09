using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityStats : MonoBehaviour
{
    [HideInInspector]
    public bool grounded = false;
    [HideInInspector]
    public float moveSpeedRatio = 1;
    public EntityMeter meter;
    public List<string> blocks = new List<string>();

    private Vector3 groundUpBackUp;
    [HideInInspector]
    public (Transform surface, Vector3 groundUp) lastSurface = (null, Vector3.zero);
    [HideInInspector]
    public Vector3 groundUp //vector of the upwards direction of the ground the player is currently on
    {
        get => groundUpBackUp;
        set {
            groundUpBackUp = value;
            if (value.x != 0)
                upAxis = (MoveAxis.HORIZONTAL, value.x > 0);
            else if (value.y != 0)
                upAxis = (MoveAxis.VERTICAL, value.y > 0);
            else if (value.z != 0)
                upAxis = (MoveAxis.DIAGONAL, value.z > 0);
            else
                Debug.LogError($"Entity {transform.name} doesn't have it's axis clarified! Make sure the upside of the ground it's standing on is set.");
            horAxis = MoveAxis.AXES.First(x => x != upAxis.index);
            verAxis = MoveAxis.AXES.Last(x => x != upAxis.index);
        }
    }

    [HideInInspector]
    public (int index, bool positive) upAxis { get; private set; } //index up-axis of the player (0=x, 1=y, 2=z). It's the axis the player will jump along etc.
    public int horAxis, verAxis;

    private void Awake()
    {
        meter = new EntityMeter();
        groundUp = new Vector3(0, 1, 0);
        meter.visualMeter = GameObject.Find("SprintMeter").transform;
        meter.currMeter = meter.maxMeter;
    }

    private void Start()
    {
        meter.Start();
    }

    private void Update()
    {
        meter.ManageMeter();
    }
}
