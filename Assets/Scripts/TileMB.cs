using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMB : MonoBehaviour
{
    public enum TileState
    {
        OPEN = 0,
        PLAYER1,
        PLAYER2
    }

    private TileState selectedState;
    public TileState SelectedState { get { return selectedState; } }

    [SerializeField]
    private Sprite[] playerSymbols;
    [SerializeField]
    private SpriteRenderer symbolRenderer;
    private Collider2D tileCollider;
    public int tileIndex = -1;

    // Use this for initialization
    void Start()
    {
        selectedState = TileState.OPEN;
        tileCollider = GetComponent<Collider2D>();

        Messenger.GetInstance().RegisterListener(new GameOverMsg(), GameOver);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new GameOverMsg(), GameOver);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            AttempSelection(); 
        }
    }

    private void AttempSelection()
    {
        if (selectedState != TileState.OPEN)
        {
            Debug.Log("This Space Is Taken!");
        }
        else
        {
            tileCollider.enabled = false;

            TileSelectedMsg msg = new TileSelectedMsg();
            msg.SelectedIndex = tileIndex;
            Messenger.GetInstance().BroadCastMessage(msg);
        }
    }

    public void SetSelectedStatus(TileState state)
    {
        selectedState = state;
        switch (state)
        {
            case TileState.PLAYER1:
                symbolRenderer.sprite = playerSymbols[0];
                break;
            case TileState.PLAYER2:
                symbolRenderer.sprite = playerSymbols[1];
                break;
            default:
                break;
        }
    }

    private void GameOver(Message msg)
    {
        tileCollider.enabled = false;
    }

    public void ResetTile()
    {
        tileCollider.enabled = true;
        symbolRenderer.sprite = null;
        selectedState = TileState.OPEN;
    }
}
