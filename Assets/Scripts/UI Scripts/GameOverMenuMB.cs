using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuMB : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winText;

    // Use this for initialization
    void Start()
    {
        Messenger.GetInstance().RegisterListener(new GameOverMsg(), ActivateGameOverMenu);
        gameObject.SetActive(false);
    }

    public void SendResetMessage()
    {
        Messenger.GetInstance().BroadCastMessage(new ResetGameMsg());
        gameObject.SetActive(false);
    }

    private void ActivateGameOverMenu(Message msg)
    {
        GameOverMsg overMsg = msg as GameOverMsg;

        gameObject.SetActive(true);
        if (overMsg.winner != TileMB.TileState.OPEN)
        {
            string winningPlayer = overMsg.winner == TileMB.TileState.PLAYER1 ? "One" : "Two";
            winText.text = "Player " + winningPlayer + " Has Won The Game!";
        }
        else
        {
            winText.text = "This Game Was A Tie!";
        }
    }
}
