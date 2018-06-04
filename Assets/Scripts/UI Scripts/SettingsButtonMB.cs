using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonMB : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsMenuParent;
    [SerializeField]
    private GameObject SettingsMenu;


    // Use this for initialization
    void Start()
    {
        if(SettingsMenu != null)
        {
            SettingsMenu.SetActive(true);
        }
    }

    public void ToggleSettingsActive()
    {
        SettingsMenu.SetActive(!SettingsMenuParent.activeSelf);
        SettingsMenuParent.SetActive(!SettingsMenuParent.activeSelf);
    }
}
