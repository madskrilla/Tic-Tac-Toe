using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeBoardMsg : Message {

    private int size = 3;
    public int Size { get { return size; } set { size = value; } }

    public ResizeBoardMsg()
    {
        msgName = "ResizeBoardMsg";
    }
}
