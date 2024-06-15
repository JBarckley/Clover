using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Object = UnityEngine.Object;

public struct TimerInstance
{
    public bool isWaiting;
    public float time;

    //private static readonly Timer timer = Timer.Get();

    public TimerInstance(float seconds)
    {
        time = Time.time + seconds;
        isWaiting = false;

        Timer.Wait(this);
    }

    public void setWaiting(bool waiting)
    {
        isWaiting = waiting;
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
        Timer.Wait(inst);
        return inst;
    }

    /*
    private class Timer
    {
        private static readonly GameObject go;
        private static readonly Clock clock;

        static readonly Timer _instance = new Timer();
        public static Timer Instance { get { return _instance; } }

        static Timer()
        {
            go = (GameObject)Object.Instantiate(Resources.Load("Utility/Clock"));
            clock = go.AddComponent<Clock>();
        }

        public static Timer Get()
        {
            return Instance;
        }

        public void Wait(TimerInstance inst)
        {
            clock.Wait(inst);
        }
    }
    */

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
