using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonMB : MonoBehaviour
{
    [SerializeField]
    private SettingsMenuMB SettingsMenuParent;


    // Use this for initialization
    void Start()
    {

    }

    public void ToggleSettingsActive()
    {
        SettingsMenuParent.ToggleMenuActive();
    }
}
