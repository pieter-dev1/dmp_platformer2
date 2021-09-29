using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Controls", menuName = "Controls", order = 1)]
public class Controls : ScriptableObject
{
    public InputAction move;
    public InputAction enableLook;
    public InputAction axisLook;
    public InputAction look;
    public InputAction jump;
    public InputAction sprint;
}
