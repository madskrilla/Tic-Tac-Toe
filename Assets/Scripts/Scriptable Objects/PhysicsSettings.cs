using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Physics")]
public class PhysicsSettings : ScriptableObject
{
    public float Gravity = -9.8f;
}
