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
    [SerializeField]
    private ParticleSystem glowParticles;

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

    public Vector3 GetCenter()
    {
        return transform.position;
    }

    private void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new GameOverMsg(), GameOver);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            AttempSelection(); 
        }
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseEnter()
    {
        glowParticles.Play();
    }

    private void OnMouseExit()
    {
        glowParticles.Stop();
    }

    public void EnablePick(bool enable)
    {
        if (selectedState == TileState.OPEN)
        {
            GetComponent<Collider2D>().enabled = enable; 
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
            TileSelectedMsg msg = new TileSelectedMsg
            {
                SelectedIndex = tileIndex
            };
            Messenger.GetInstance().BroadCastMessage(msg);
        }
    }

    public void SetPlayerSymbols(Sprite playerOne, Sprite playerTwo)
    {
        playerSymbols = new Sprite[] { playerOne, playerTwo };
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

        StartCoroutine(FadeSymbolOn());
        tileCollider.enabled = false;
    }

    private IEnumerator FadeSymbolOn()
    {
        Color baseColor = symbolRenderer.color;
        float alpha = 0;
        float lerpTime = 0;

        while (baseColor.a < 255f)
        {
            alpha = Mathf.Lerp(0, 1, lerpTime / .5f);
            symbolRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            lerpTime += Time.deltaTime;
            yield return null;
        }

        yield break;
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
