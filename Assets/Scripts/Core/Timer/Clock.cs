using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private List<TimerInstance> objectsWaiting;

    void Update()
    {
        foreach (TimerInstance inst in objectsWaiting) 
        {
            if (Time.time > inst.time)
            {
                inst.setWaiting(false);
                objectsWaiting.Remove(inst);
            }
        }
    }

    public void Wait(TimerInstance inst)
    {
        objectsWaiting.Add(inst);
    }
}
