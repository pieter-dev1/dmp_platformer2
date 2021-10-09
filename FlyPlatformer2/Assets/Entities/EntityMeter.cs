using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EntityMeter : MonoBehaviour
{
    public string name = "Sprint";
    public readonly float maxMeter = 1.5f;
    public readonly float usageMinimum = 0.5f;
    public EntityComponents comps;

    [HideInInspector]
    public (MonoBehaviour script, string method, object[] parameters) triggerCallInfo;
    public MethodInfo triggerCall { get; private set; }
    [HideInInspector]
    public List<(Component script, string variable, object value)> triggerConditionsInfo = new List<(Component script, string variable, object value)>();
    public List<(Component script, FieldInfo variable, object conditionValue)> triggerConditions { get; private set; }
    private bool TriggerConditionsMet() => triggerConditions.All(x => x.variable.GetValue(x.script).GetHashCode() == x.conditionValue.GetHashCode());
    //private bool TriggerConditionsMet() {
    //    foreach (var c in triggerConditions)
    //    {
    //        var val = c.variable.GetValue(c.script);
    //        print($"{val.GetHashCode()}, {c.conditionValue.GetHashCode()}");
    //        if (val.GetHashCode() != c.conditionValue.GetHashCode())
    //            return false;
    //    }
    //    return true;
    //}

    [HideInInspector]
    public bool currUsing = false;
    [HideInInspector]
    public bool triggered = false;
    [HideInInspector]
    public bool allowUsage = true;
    [HideInInspector]
    public bool resetThisUsage = false; //determines if the meter reached 0 once of the current continoues usage of the meter. For a player this is when a button is held down.
    [HideInInspector]
    public float currMeter;
    public EffectExecution[] undoEffects;

    public Transform visualMeter;

    public void Start()
    {
        triggerCall = triggerCallInfo.script.GetType().GetMethod(triggerCallInfo.method);
        triggerConditions = new List<(Component script, FieldInfo variable, object conditionValue)>();
        foreach (var condition in triggerConditionsInfo)
        {
            var script = condition.script;
            var variable = script.GetType().GetField(condition.variable);
            triggerConditions.Add((script, variable, condition.value));
        }
        visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
    }

    public void ManageMeter()
    {
        if (currUsing && allowUsage && currMeter > 0)
        {
            if (!TriggerConditionsMet())
                return;
            currMeter -= 0.01f;
            visualMeter.GetComponent<Image>().color = Color.yellow;
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
        else if (currMeter <= 0 && allowUsage)
        {
            currMeter = 0;
            allowUsage = false;
            resetThisUsage = true;
            comps.fauxAttractor.CancelCustomGravity();
            comps.gameObject.ExecuteEffects(comps.gameObject, true, undoEffects);
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
        else if (currMeter < maxMeter)
        {
            currMeter += 0.01f;
            if (currMeter <= usageMinimum)
                visualMeter.GetComponent<Image>().color = Color.red;
            else if (currMeter > usageMinimum)
            {
                currUsing = false;
                allowUsage = true;
                visualMeter.GetComponent<Image>().color = Color.white;
            }
            //THIS CAN REPLACE CURRENT ELSE IF, DOESNT WORK PERFECTLY THO SO NEEDS FINETUNING
            //else if (!triggered && currMeter > usageMinimum)
            //{
            //    //if (currUsing)
            //    //    comps.fauxAttractor.enabled = true;
            //    triggered = true;
            //    triggerCall.Invoke(triggerCallInfo.script, triggerCallInfo.parameters);
            //    allowUsage = true;
            //    visualMeter.GetComponent<Image>().color = Color.white;
            //}
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
        else if (currMeter > maxMeter)
        {
            currMeter = maxMeter;
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
    }




    public void FillMeter(float amount)
    {
        currMeter += amount;
        if (currMeter > maxMeter)
            currMeter = maxMeter;
        visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
    }
}
