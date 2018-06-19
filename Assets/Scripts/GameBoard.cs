using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{

    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private Transform BoardCenter;
    private TileMB[] board;
    private BoardEvaluator evaluator;
    private int boardSize = 0;
    public int BoardSize
    {
        get { return boardSize; }
    }
    public int MaxTurns
    {
        get { return boardSize * boardSize; }
    }

    private Sprite playerOneSprite;
    private Sprite playerTwoSprite;

    private List<int> openTiles = new List<int>();
    public List<int> OpenTiles
    {
        get { return openTiles; }
    }

    // Use this for initialization
    void Start()
    {

    }

    public void SetPlayerSymbols(Sprite playerOne, Sprite playerTwo)
    {
        playerOneSprite = playerOne;
        playerTwoSprite = playerTwo;
    }


    public void CreateGameBoard(int boardSize)
    {
        this.boardSize = boardSize;
        DestroyGameBoard();
        CreateBoardTiles();
        StartCoroutine(ShowBoardTiles());
        evaluator = new BoardEvaluator(boardSize);

    }

    private void CreateBoardTiles()
    {
        board = new TileMB[boardSize * boardSize];
        Vector2 topLeft = new Vector2();
        Vector2 tileSize = TilePrefab.GetComponent<SpriteRenderer>().size;
        float betweenTiles = tileSize.x * 0.5f;

        topLeft.x = BoardCenter.position.x - ((tileSize.x + betweenTiles) * (boardSize * 0.5f));
        topLeft.y = BoardCenter.position.y + ((tileSize.y + betweenTiles) * (boardSize * 0.5f));
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int tileIndex = i + (j * boardSize);
                openTiles.Add(tileIndex);
                GameObject newTile = GameObject.Instantiate<GameObject>(TilePrefab);
                //Initialize Object Transform
                newTile.transform.parent = BoardCenter;
                newTile.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                newTile.transform.position = topLeft + new Vector2(i * (tileSize.x + betweenTiles), j * (-tileSize.y - betweenTiles));

                //Initialize Tile Settings
                TileMB tile = newTile.GetComponent<TileMB>();
                tile.tileIndex = tileIndex;
                tile.SetPlayerSymbols(playerOneSprite, playerTwoSprite);
                board[tileIndex] = tile;
            }
        }

    }

    private IEnumerator ShowBoardTiles()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i].GetComponent<Animator>().SetTrigger("ShowTile");
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void EnablePicks(bool enable)
    {
        foreach (TileMB tile in board)
        {
            tile.EnablePick(enable);
        }
    }

    public void SelectTile(int index, TileMB.TileState tileOwner)
    {
        board[index].SetSelectedStatus(tileOwner);
        openTiles.Remove(index);
    }

    public BoardEvaluator.WinInfo EvaluateBoard(int lastSelection)
    {
        return evaluator.EvaluateBoard(board, lastSelection);
    }

    public void ResetBoard()
    {
        openTiles.Clear();

        int tileIndex = 0;
        foreach (TileMB tile in board)
        {
            openTiles.Add(tileIndex);
            tileIndex++;
            tile.ResetTile();
        }
    }

    private void DestroyGameBoard()
    {
        if (board != null)
        {
            foreach (TileMB tile in board)
            {
                GameObject.Destroy(tile.gameObject);
            }
        }
    }
}
