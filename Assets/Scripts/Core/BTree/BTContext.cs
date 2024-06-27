using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BTContext
{
    private Dictionary<string, object> m_variables = new Dictionary<string, object>();
    
    public static void SetVariable(BTContext context, string key, object value)
    {
        if (!context.m_variables.TryAdd(key, value))
        {
            context.m_variables[key] = value;
        }
    }
    public void SetVariable(string key, object value)
    {
        if (!m_variables.TryAdd(key, value))
        {
            m_variables[key] = value;
        }
    }
    public object GetVariable(string key)
    {
        return m_variables[key];
    }
    
    /* getKeys()
     *      returns List<string> of all keys in m_variables         --->        unecessary for now
     */
    
    public void Clear()
    {
        m_variables.Clear();
    }
}
