using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityMeter : MonoBehaviour
{
    public string name = "Sprint";
    public readonly float maxMeter = 1.5f;
    public readonly float usageMinimum = 0.5f;
    public EntityComponents comps;

    [HideInInspector]
    public bool currUsing = false;
    [HideInInspector]
    public bool allowUsage = true;
    [HideInInspector]
    public float currMeter;
    public EffectExecution[] undoEffects;

    public Transform visualMeter;

    public void ManageMeter()
    {
        if (currUsing && allowUsage && currMeter > 0)
        {
            currMeter -= 0.01f;
            visualMeter.GetComponent<Image>().color = Color.yellow;
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
        else if (currMeter <= 0 && allowUsage)
        {
            currMeter = 0;
            allowUsage = false;
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
                if (currUsing)
                    comps.fauxAttractor.enabled = true;
                allowUsage = true;
                visualMeter.GetComponent<Image>().color = Color.white;
            }
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
        else if (currMeter > maxMeter)
        {
            currMeter = maxMeter;
            visualMeter.localScale = new Vector3(maxMeter * currMeter, visualMeter.localScale.y, visualMeter.localScale.z);
        }
    }
}
