using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Threading.Tasks;
using Extension;

public static class EffectEffector
{
    //Undo means all the effects will be reversed. So the opposite value will be applied. Comes in handy when you need to manually need to undo multiple effects,
    //because you don't need to make an opposite EffectExecution for every effect.
    public static void ExecuteEffects(this GameObject target, GameObject source, bool undo = false, params EffectExecution[] effectExecutions)
    {
        var type = typeof(EffectEffector);
        object currEffectValue = null;
        MethodInfo currMethod = null;
        foreach (var effect in effectExecutions)
        {
            currEffectValue = effect.amount;
            if (undo)
                currEffectValue = currEffectValue.GetOppositeValue();
            currMethod = type.GetMethod($"Change{effect.effect}");
            //Evt kijken of de params van de currMethod opgehaald kunnen worden en alleen die meegegeven kunnen worden. Biedt dit echter wel voordelen?
            currMethod.Invoke(null, new object[] { target, source, currEffectValue, effect.duration, effect.ticksAmount });
        }
    }

    private static bool HasEffectTimer(float duration) => duration > 0;

    //Amount is speed change in percentage
    public async static void ChangeMoveSpeed(this GameObject target, GameObject source, float amount, float duration, int ticksAmount)
    {
        var ratioAmount = amount / 100;
        target.GetComponent<EntityStats>().moveSpeedRatio += ratioAmount;

        //UNTESTED
        if (HasEffectTimer(duration))
        {
            await Task.Delay((int) duration * 1000);
            target.GetComponent<EntityStats>().moveSpeedRatio -= ratioAmount;
        }
    }
}
