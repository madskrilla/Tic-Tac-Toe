using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBoardButtonMB : MonoBehaviour
{
    private ResizeBoardMsg resizeMsg = new ResizeBoardMsg();
    private BoardSelectMenuMB boardMenu;

    private void Start()
    {
        boardMenu = GetComponent<BoardSelectMenuMB>();
    }

    public void SendResizeMsg(int size)
    {
        resizeMsg.Size = size;
        Messenger.GetInstance().BroadCastMessage(resizeMsg);
        boardMenu.CloseMenu();
    }
}
