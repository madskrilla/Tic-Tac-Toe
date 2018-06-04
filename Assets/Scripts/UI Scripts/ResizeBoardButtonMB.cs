using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBoardButtonMB : MonoBehaviour
{
    private ResizeBoardMsg resizeMsg = new ResizeBoardMsg();

    public void SendResizeMsg(int size)
    {
        resizeMsg.Size = size;
        Messenger.GetInstance().BroadCastMessage(resizeMsg);
        gameObject.SetActive(false);
    }
}
