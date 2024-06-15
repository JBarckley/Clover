using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Object = UnityEngine.Object;

public class TimerInstance
{
    public bool isWaiting;
    public float time;

    //private static readonly Timer timer = Timer.Get();

    public TimerInstance(float seconds = 0)
    {
        if (seconds == 0)
        {
            time = 0;
            isWaiting = false;
        }
        else
        {
            time = Time.time + seconds;
            Debug.Log(time);
            isWaiting = true;

            Timer.Wait(this);
        }
    }

    public static bool operator true(TimerInstance inst)
    {
        return inst.isWaiting == true;
    }

    public static bool operator false(TimerInstance inst)
    {
        return inst.isWaiting == false;
    }

    public static bool operator !(TimerInstance inst)
    {
        return !inst.isWaiting;
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
