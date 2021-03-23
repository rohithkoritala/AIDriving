using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour, IControls
{
    public float InputX { get; private set; }

    public float InputY { get; private set; }

    private float inputX, inputY;
    void Update()
    {
        inputX = 0;
        inputY = 0;

        if (Input.GetKey(KeyCode.W)) inputY = 1;
        if (Input.GetKey(KeyCode.S)) inputY = -1;
        if (Input.GetKey(KeyCode.D)) inputX = 1;
        if (Input.GetKey(KeyCode.A)) inputX = -1;

        InputX = inputX;
        InputY = inputY;
    }
}
