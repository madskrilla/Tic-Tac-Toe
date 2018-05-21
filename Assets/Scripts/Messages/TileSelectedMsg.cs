using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectedMsg : Message
{
    public TileSelectedMsg() : base()
    {
        msgName = "TileSelect";
    }
    public int SelectedIndex;
}
