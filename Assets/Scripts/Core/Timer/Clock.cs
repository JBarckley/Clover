using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private List<TimerInstance> objectsWaiting = new List<TimerInstance>();
    private List<TimerInstance> toRemove = new List<TimerInstance>();

    void Update()
    {
        foreach (TimerInstance inst in objectsWaiting) 
        {
            if (Time.time > inst.time)
            {
                inst.isWaiting = false;
                toRemove.Add(inst);
            }
        }

        foreach (TimerInstance inst in toRemove)
        {
            objectsWaiting.Remove(inst);
        }

        toRemove.Clear();
    }

    public void Wait(TimerInstance inst)
    {
        objectsWaiting.Add(inst);
    }
}
