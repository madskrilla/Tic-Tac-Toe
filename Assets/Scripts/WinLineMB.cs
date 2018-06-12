using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLineMB : MonoBehaviour
{
    [SerializeField]
    private LineRenderer winLine;
    [SerializeField]
    private float drawTime = 0.75f;

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
            StartCoroutine(DrawWinLine(gameOverMsg.winStartPos, gameOverMsg.winEndPos));
        }
    }

    private IEnumerator DrawWinLine(Vector3 startPos, Vector3 endPos)
    {
        Vector3 currPos;
        float time;
        float timePassed;

        while (true)
        {
            currPos = startPos;
            time = 0f;
            while (currPos != endPos)
            {
                timePassed = time / drawTime;
                currPos = Vector3.Lerp(startPos, endPos, timePassed);
                winLine.SetPositions(new Vector3[] { startPos, currPos });
                time += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.15f);

            currPos = startPos;
            time = 0f;
            while (currPos != endPos)
            {
                timePassed = time / drawTime;
                currPos = Vector3.Lerp(startPos, endPos, timePassed);
                winLine.SetPositions(new Vector3[] { currPos, endPos });
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void Reset(Message msg)
    {
        winLine.gameObject.SetActive(false);
    }
}
