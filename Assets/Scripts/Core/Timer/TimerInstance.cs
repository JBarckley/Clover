using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Object = UnityEngine.Object;

/// <summary>
/// A TimerInstance can be delayed using TimerInstance t += float delay.
/// 
/// A TimerInstance can be used as a boolean value where it will be false until the delay is over, where it will be true.
/// </summary>
public class TimerInstance
{
    public bool isWaiting = false;
    public float time;

    public TimerInstance(float seconds = 0)
    {
        if (seconds == 0)
        {
            time = 0;
        }
        else
        {
            time = Time.time + seconds;
            isWaiting = true;

            Timer.Wait(this);
        }
    }

    public static implicit operator bool(TimerInstance inst)
    {
        return inst.isWaiting;
    }

    public static TimerInstance operator +(TimerInstance inst, float b) 
    {
        inst.time = Time.time + b;
        inst.isWaiting = true;
        Timer.Wait(inst);
        return inst;
    }

    private static class Timer
    {
        private static readonly GameObject go = (GameObject)Object.Instantiate(Resources.Load("Utility/Clock"));
        private static readonly Clock clock = go.AddComponent<Clock>();

        public static void Wait(TimerInstance inst)
        {
            clock.Wait(inst);
        }
    }
};
