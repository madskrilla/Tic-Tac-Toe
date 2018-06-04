using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSelectionMsg : Message
{
    public int player = 0;
    public Sprite symbol = null;

    public SymbolSelectionMsg() : base()
    {
        msgName = "SelectSymbol";
    }
}
