using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{

    public static Key left = Key.A;
    public static Key right = Key.D;
    public static Key forward = Key.W;
    public static Key back = Key.S;

    public static Key dash = Key.LeftShift;

    public static Key jump = Key.Space;

    public static bool Jump()
    {

        return Keyboard.current[jump].wasPressedThisFrame;

    }

    public static bool Dash()
    {

        return Keyboard.current[dash].wasPressedThisFrame;

    }

    public static Vector2 MovementVector()
    {

        return new Vector2((Keyboard.current[left].isPressed ? -1 : 0) + (Keyboard.current[right].isPressed ? 1 : 0), (Keyboard.current[back].isPressed ? -1 : 0) + (Keyboard.current[forward].isPressed ? 1 : 0));

    }

    public static Vector2 MouseMovement()
    {

        return Mouse.current.delta.ReadValue();

    }

}
