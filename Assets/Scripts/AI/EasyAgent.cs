using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyAgent : BaseAgent
{
    public override IEnumerator Pick()
    {
        int pick = RandomNumberUtil.GetRandomValue(board.OpenTiles);
        yield return new WaitForSeconds(0.25f);
        SendPick(pick);
    }
}
