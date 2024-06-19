using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoSingleton<GameCamera>
{
    public static Camera Cam;

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    public static void Teleport(Vector3 position)
    {
        Instance.transform.position = new Vector3(position.x, position.y, -10f);
    }
}
