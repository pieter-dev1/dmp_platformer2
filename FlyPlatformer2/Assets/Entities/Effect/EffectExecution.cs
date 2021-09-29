using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExecution
{
    public string effect;
    public object amount;
    public float duration; //0 means effect is infinite (isn't a timer effect or will be manually removed)
    public int ticksAmount; //0 or 1 means no ticks over time

    public EffectExecution(string effect, object amount, float duration = 0, int ticksAmount = 0)
    {
        this.effect = effect;
        this.amount = amount;
        this.duration = duration;
        this.ticksAmount = ticksAmount;
    }
}
