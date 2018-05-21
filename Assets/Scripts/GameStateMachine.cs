using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private Transform BoardCenter;
    [SerializeField]
    private Image PlayerOneSymbol;
    [SerializeField]
    private Image PlayerTwoSymbol;

    private Color activePlayerColor = Color.white;
    private Color inactivePlayerColor = Color.gray;

    private struct State
    {
        public State(string name, Action func)
        {
            stateAction = func;
            stateName = name;
        }
        public Action stateAction;
        public string stateName;
    }
    private Dictionary<string, State> states = new Dictionary<string, State>();
    private State currentState;

    private TileMB[] board;
    private int boardSize = 3;
    private BoardEvaluator evaluator;
    private BoardEvaluator.WinInfo evalResult;
    private int turnCount = 0;
    private History sessionHistory;

    private int lastSelectedTile = -1;
    private bool playerOne = true;

    public GameStateMachine()
    {

    }

    void Start()
    {
        AddState(new State("WaitForPick", null));
        AddState(new State("Evaluate",EvaluateGameState));
        AddState(new State("GameOver", GameOver));

        SwitchState("WaitForPick");

        Messenger.GetInstance().RegisterListener(new TileSelectedMsg(), WaitForPick);
        Messenger.GetInstance().RegisterListener(new ResetGameMsg(), Reset);
        Messenger.GetInstance().RegisterListener(new ResizeBoardMsg(), ResizeBoard);

        sessionHistory = new History();
        sessionHistory.StartNewGame();

        PlayerOneSymbol.color = activePlayerColor;
        PlayerTwoSymbol.color = inactivePlayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }

    private void ResizeBoard(Message msg)
    {
        ResizeBoardMsg resizeMsg = msg as ResizeBoardMsg;
        boardSize = resizeMsg.Size;

        DestroyGameBoard();
        CreateGameBoard(boardSize);
        evaluator = new BoardEvaluator(boardSize);
    }

    public void Reset(Message msg)
    {
        ResetGame();
    }

    private void ResetGame()
    {
        foreach (TileMB tile in board)
        {
            tile.ResetTile();
        }

        SwitchState("WaitForPick");

        playerOne = true;
        PlayerOneSymbol.color = activePlayerColor;
        PlayerTwoSymbol.color = inactivePlayerColor;

        turnCount = 0;
        sessionHistory.StartNewGame();
    }

    void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new TileSelectedMsg(), WaitForPick);
        Messenger.GetInstance().UnregisterListener(new ResetGameMsg(), Reset);
        Messenger.GetInstance().UnregisterListener(new ResizeBoardMsg(), ResizeBoard);
    }

    private void AddState(State newState)
    {
        Debug.Assert(states.ContainsKey(newState.stateName) == false, String.Format("State {0} Already Exists", name));
        states.Add(newState.stateName, newState);
    }

    private void SwitchState(string nextState)
    {
        Debug.Assert(states.ContainsKey(nextState), string.Format("State {0} Does Not Exist", nextState));
        currentState = states[nextState];

        Debug.Log(string.Format("Switching to {0} State!", nextState));
        if (currentState.stateAction != null)
        {
            currentState.stateAction(); 
        }
    }

    private void CreateGameBoard(int size)
    {
        board = new TileMB[size * size];
        Vector2 topLeft = new Vector2();
        Vector2 tileSize = TilePrefab.GetComponent<SpriteRenderer>().size;

        topLeft.x = BoardCenter.position.x - (tileSize.x * (size * 0.5f));
        topLeft.y = BoardCenter.position.y + (tileSize.y * (size * 0.5f));
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int tileIndex = i + (j * size);
                GameObject newTile = GameObject.Instantiate<GameObject>(TilePrefab);
                newTile.transform.parent = BoardCenter;
                newTile.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
                newTile.transform.position = topLeft + new Vector2(i * tileSize.x, j * -tileSize.y);
                newTile.GetComponent<TileMB>().tileIndex = tileIndex;
                board[tileIndex] = newTile.GetComponent<TileMB>();
            }
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

    private void WaitForPick(Message msg)
    {
        //Ignore Tile Selection Msg if Not Waiting For Pick
        if (currentState.stateName == "WaitForPick")
        {
            TileSelectedMsg tileSelected = msg as TileSelectedMsg;
            lastSelectedTile = tileSelected.SelectedIndex;
            TileMB.TileState tileOwner = playerOne ? TileMB.TileState.PLAYER1 : TileMB.TileState.PLAYER2;
            board[lastSelectedTile].SetSelectedStatus( tileOwner);
            sessionHistory.AddMoveToRecord(lastSelectedTile, (int)tileOwner);
            Debug.Log(string.Format("Tile {0} Was Selected", lastSelectedTile));
            SwitchState("Evaluate");
        }
    }

    private void EvaluateGameState()
    {
        evalResult = evaluator.EvaluateBoard(board, lastSelectedTile);
        if (evalResult.winPresent == true || turnCount >= board.Length - 1)
        {
            SwitchState("GameOver");
        }
        else
        {
            turnCount++;
            playerOne = !playerOne;
            PlayerOneSymbol.color = playerOne ? activePlayerColor : inactivePlayerColor;
            PlayerTwoSymbol.color = !playerOne ? activePlayerColor : inactivePlayerColor;

            SwitchState("WaitForPick");
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver!");
        GameOverMsg msg = new GameOverMsg();
        if (evalResult.winPresent)
        {
            msg.winner = playerOne ? TileMB.TileState.PLAYER1 : TileMB.TileState.PLAYER2;
            msg.winStartPos = evalResult.winStart.position;
            msg.winEndPos = evalResult.winEnd.position;

            //Shifting Tile Positions From Topleft to Center
            Vector2 tileSize = TilePrefab.GetComponent<SpriteRenderer>().size;
            Vector2 halfSize = new Vector2(tileSize.x * 0.5f, tileSize.y * 0.5f);
            msg.winStartPos += new Vector3(halfSize.x, -halfSize.y, 0);
            msg.winEndPos += new Vector3(halfSize.x, -halfSize.y, 0); 
        }
        else
        {
            msg.winner = TileMB.TileState.OPEN;
        }

        Messenger.GetInstance().BroadCastMessage(msg);
    }
}
