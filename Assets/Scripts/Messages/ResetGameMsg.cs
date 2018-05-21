using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameMsg : Message
{
    public ResetGameMsg() : base()
    {
        msgName = "ResetMsg";
    }
}
