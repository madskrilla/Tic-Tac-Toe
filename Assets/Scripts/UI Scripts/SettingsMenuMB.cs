using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuMB : MonoBehaviour
{
    [SerializeField]
    private GameObject boardSelectionMenu;
    [SerializeField]
    private GameObject symbolSelectionMenu;
    private Animator menuController;

    private void Start()
    {
        menuController = GetComponent<Animator>();
    }

    public void ActivateBoardSelectionMenu()
    {
        Messenger.GetInstance().BroadCastMessage(new OpenBoardSelectMenuMsg());
        ToggleMenuActive();
    }

    public void ActivateSymbolSelectionMenu()
    {
        Messenger.GetInstance().BroadCastMessage(new OpenSymbolSelectMenuMsg());
        ToggleMenuActive();
    }

    public void ToggleMenuActive()
    {
        bool show = menuController.GetBool("Show");
        menuController.SetBool("Show", !show);
    }
}
