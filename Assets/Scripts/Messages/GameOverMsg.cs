using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMsg : Message
{
    public Vector3 winStartPos;
    public Vector3 winEndPos;

    public TileMB.TileState winner;

    public GameOverMsg() : base()
    {
        msgName = "GameOver";
    }
}
