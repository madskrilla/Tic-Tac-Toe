using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAgent
{
    protected GameBoard board = null;
    public GameBoard Board
    {
        set { board = value; }
    }

    protected void SendPick(int index)
    {
        TileSelectedMsg msg = new TileSelectedMsg
        {
            SelectedIndex = index
        };
        Messenger.GetInstance().BroadCastMessage(msg);

    }

    public abstract IEnumerator Pick();

}
