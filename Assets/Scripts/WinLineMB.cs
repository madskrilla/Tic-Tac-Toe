using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLineMB : MonoBehaviour
{
    [SerializeField]
    private LineRenderer winLine;

    // Use this for initialization
    void Start()
    {
        winLine.gameObject.SetActive(false);
        Messenger.GetInstance().RegisterListener(new GameOverMsg(), DrawWinLine);
        Messenger.GetInstance().RegisterListener(new ResetGameMsg(), Reset);
    }

    private void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new GameOverMsg(), DrawWinLine);
        Messenger.GetInstance().UnregisterListener(new ResetGameMsg(), Reset);
    }

    private void DrawWinLine(Message msg)
    {
        GameOverMsg gameOverMsg = msg as GameOverMsg;

        if (gameOverMsg.winner != TileMB.TileState.OPEN)
        {
            winLine.gameObject.SetActive(true);
            winLine.SetPositions(new Vector3[] { gameOverMsg.winStartPos, gameOverMsg.winEndPos }); 
        }
    }

    private void Reset(Message msg)
    {
        winLine.gameObject.SetActive(false);
    }
}
