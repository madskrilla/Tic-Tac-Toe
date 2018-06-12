using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelectMenuMB : OptionMenuMB
{
    private void Start()
    {
        Messenger.GetInstance().RegisterListener(new OpenBoardSelectMenuMsg(), OpenMenu);
    }

    protected void OpenMenu(Message msg)
    {
        OpenMenu();
    }

    private void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new OpenBoardSelectMenuMsg(), OpenMenu);
    }
}
