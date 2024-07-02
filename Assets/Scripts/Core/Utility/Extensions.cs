using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static void Print<T>(this List<T> list)
    {
        foreach (T item in list)
        {
            Debug.Log(item);
        }
    }
}