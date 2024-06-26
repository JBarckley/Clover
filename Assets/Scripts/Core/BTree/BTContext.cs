using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BTContext
{
    private Dictionary<string, object> m_variables = new Dictionary<string, object>();
    
    public void SetVariable(string key, object value)
    {
        m_variables.Add(key, value);
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
