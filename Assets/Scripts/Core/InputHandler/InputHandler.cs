using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Action = System.Action;
using Object = UnityEngine.Object;

public class InputHandler : MonoSingleton<InputHandler>
{
    public static InputSystem input;
    public static Vector2 moveVector;

    void Update()
    {
        moveVector = input.Player.Move.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        input = new InputSystem();

        input.Player.Enable();
        input.UIUX.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
        input.UIUX.Disable();
    }

    public Action<InputAction.CallbackContext> enterBattleAction
    {
        set => input.UIUX.Battle.performed += value;
    }
}

public static class InputHandlerConstructor
{
    public static T AddComponent<T>(this Object obj, Action<InputAction.CallbackContext> enterBattleMethod) where T : InputHandler
    {
        T comp = obj.AddComponent<T>();
        comp.enterBattleAction = enterBattleMethod;
        return comp;
    }
}
