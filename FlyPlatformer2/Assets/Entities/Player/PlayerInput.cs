using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Controls controls;
    private EntityComponents comps;
    private int enableLookButtonsPressed = 0; //when enableLook and axisLook are both held, releasing one will disable looking around, while it should still be allowed. So if both are held, releasing one shouldnt disable.

    //Sprint/wallrun effects
    private readonly EffectExecution sprintEffect = new EffectExecution(Effect.MOVESPEED, 30);

    // Start is called before the first frame update
    void Start()
    {
        comps = GetComponent<EntityComponents>();
        comps.entityStats.meter.comps = comps;
        comps.entityStats.meter.undoEffects = new EffectExecution[] { sprintEffect };
        comps.entityStats.meter.triggerCallInfo = (this, "TriggerSprint", new object[0]);
        comps.entityStats.meter.triggerConditionsInfo = new List<(Component script, string variable, object value)>()
        {
            (comps.entityMovement, "moving", true),
            (comps.entityJump, "jumped", false),
        };

        GameObject.Find("PlayerVcam").GetComponent<CinemachineInputActionProvider>().XYAxis = controls.look;

        controls.move.started += _ => { comps.entityMovement.moving = true; };
        controls.move.performed += ctx => comps.entityMovement.direction = ctx.ReadValue<Vector2>();
        controls.move.canceled += _ => { comps.entityMovement.CancelMovement(); };

        //controls.enableLook.started += _ => { controls.look.Enable(); enableLookButtonsPressed++; };
        //controls.enableLook.canceled += _ => ReleaseLookButton();

        //controls.axisLook.started += _ => { controls.look.Enable(); enableLookButtonsPressed++; };
        //controls.axisLook.canceled += _ => ReleaseLookButton();

        //pressedJump tracks if the jump input has succesfully come through. With this it can be forced that the jumpcancel input only comes through on the actual jump
        //(the first time you press the button). This means every press (jumpcancel) after (whem youre in the air) will not come through, which prevents a bug
        //where you would stop falling a brief moment even though you were already falling from your jump.
        var pressedJump = false;
        controls.jump.performed += _ => { if (comps.entityStats.grounded ) {
                comps.entityStats.lastSurface = (comps.fauxAttractor.currentSurface, comps.entityStats.groundUp);
                comps.entityJump.Jump(); pressedJump = true;
            }
        };
        //if it's certain the player has jumped, doesn't touch the ground and isn't going down anyways, the jump can be cancelled.
        controls.jump.canceled += _ => { if (comps.entityJump.jumped && pressedJump && !comps.entityStats.grounded
            //Last condition makes sure the player is still jumping up, because when falling cancelling the jump has no use and is buggy.
            && ((comps.entityStats.upAxis.positive && comps.rigidbody.velocity[comps.entityStats.upAxis.index] > 0) 
            || (!comps.entityStats.upAxis.positive && comps.rigidbody.velocity[comps.entityStats.upAxis.index] < 0))) {
                comps.entityJump.CancelJump(); pressedJump = false; }
        };

        //Sprint/wallrun
        controls.sprint.started += _ => TriggerSprint();
        controls.sprint.canceled += _ => CancelSprint();
    }

    public void TriggerSprint()
    {
        if (comps.entityStats.meter.currMeter >= comps.entityStats.meter.usageMinimum)
        {
            gameObject.ExecuteEffects(gameObject, false, sprintEffect);
            comps.fauxAttractor.enabled = true;
            comps.entityStats.meter.currUsing = true;
            if (comps.entityJump.jumped)
            {
                comps.fauxAttractor.onWall = comps.entityStats.lastSurface.surface.tag.Equals(Tags.WALL);
                comps.fauxAttractor.currentSurface = comps.entityStats.lastSurface.surface;
                comps.entityStats.groundUp = comps.entityStats.lastSurface.groundUp;
            }
        }
    }

    private void CancelSprint()
    {
        comps.entityStats.blocks.Remove(Blocks.MOVE);
        comps.entityStats.meter.currUsing = false;
        if (comps.fauxAttractor.enabled && comps.entityStats.meter.allowUsage && !comps.entityStats.meter.resetThisUsage)
        {
            gameObject.ExecuteEffects(gameObject, true, sprintEffect);
            comps.fauxAttractor.CancelCustomGravity();
        };
        comps.entityStats.meter.resetThisUsage = false;
    }

    private void ReleaseLookButton()
    {
        if (enableLookButtonsPressed <= 1)
        {
            controls.look.Disable();
            enableLookButtonsPressed = 0;
        }
        else enableLookButtonsPressed--;
    }

    private void OnEnable()
    {
        controls.move.Enable();
        controls.enableLook.Enable();
        controls.axisLook.Enable();
        controls.look.Disable();
        controls.jump.Enable();
        controls.sprint.Enable();
    }

    private void OnDisable()
    {
        controls.move.Disable();
        controls.enableLook.Disable();
        controls.axisLook.Disable();
        controls.look.Disable();
        controls.jump.Disable();
        controls.sprint.Disable();
    }
}
