using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerControls controls = new PlayerControls();

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
