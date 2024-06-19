using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Defines an abstract monobehavior that can only have one instance.
/// </summary>
/// <remarks>
/// Any class that extends MonoSingleton cannot implement Awake(). Instead, use Start() to run code pre-Update().
/// Similarly, because the Awake() layer executes before/concurrently with MonoSingleton, it should be avoided on non-singleton monobehaviors as well.
/// </remarks>
/// <typeparam name="T">
/// Replace T with the class being extended such that <code> public class ClassA : MonoSingleton<ClassA> </code>
/// </typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }

    public static Vector2 GetPosition2()
    {
        return new Vector2(Instance.transform.position.x, Instance.transform.position.y);
    }

    public static Vector3 GetPosition3()
    {
        return Instance.transform.position;
    }
}
