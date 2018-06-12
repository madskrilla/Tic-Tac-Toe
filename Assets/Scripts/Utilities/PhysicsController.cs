using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    [SerializeField]
    public PhysicsSettings settings;

    public float Gravity
    {
        get { return settings.Gravity; }
    }
}
