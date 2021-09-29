using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponents : MonoBehaviour
{
    //Entity
    [HideInInspector]
    public EntityStats entityStats;
    [HideInInspector]
    public EntityMovement entityMovement;
    [HideInInspector]
    public EntityJump entityJump;
    [HideInInspector]
    public FauxAttractor fauxAttractor;

    //Player
    [HideInInspector]
    public PlayerInput playerInput;

    //Components
    [HideInInspector]
    public new Rigidbody rigidbody;
    [HideInInspector]
    public new Collider collider;
    [HideInInspector]
    public Mesh mesh;


    private void Awake()
    {
        entityStats = GetComponent<EntityStats>();
        entityMovement = GetComponent<EntityMovement>();
        entityJump = GetComponent<EntityJump>();
        fauxAttractor = GetComponent<FauxAttractor>();

        playerInput = GetComponent<PlayerInput>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        mesh = GetComponent<Mesh>();
    }
}
