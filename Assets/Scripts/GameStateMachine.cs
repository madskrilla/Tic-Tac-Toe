using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
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

    private GameBoard gameBoard;
    private BaseAgent aiPlayer;

    private int turnCount = 1;

    private int lastSelectedTile = -1;
    private bool playerOne = true;
    private BoardEvaluator.WinInfo evalResult;

    public GameStateMachine()
    {

    }

    void Start()
    {
        AddState(new State("SetPlayerIcons", OpenIconMenu));
        AddState(new State("WaitForPick", EnablePickState));
        AddState(new State("Evaluate", EvaluateGameState));
        AddState(new State("GameOver", GameOver));

        SwitchState("SetPlayerIcons");

        Messenger.GetInstance().RegisterListener(new TileSelectedMsg(), WaitForPick);
        Messenger.GetInstance().RegisterListener(new ResetGameMsg(), Reset);
        Messenger.GetInstance().RegisterListener(new SymbolSelectionMsg(), SetPlayerSymbol);

        gameBoard = GetComponent<GameBoard>();
        aiPlayer = new EasyAgent();
        aiPlayer.Board = gameBoard;
    }

    private void OpenBoardSizeMenu()
    {
        Messenger.GetInstance().BroadCastMessage(new OpenBoardSelectMenuMsg());
    }

    private void OpenIconMenu()
    {
        Messenger.GetInstance().BroadCastMessage(new OpenSymbolSelectMenuMsg());
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

        
        gameBoard.CreateGameBoard(resizeMsg.Size);

        SwitchState("WaitForPick");
    }

    private void SetPlayerSymbol(Message msg)
    {
        SymbolSelectionMsg selection = msg as SymbolSelectionMsg;
        if (selection.player == 0)
        {
            PlayerOneSymbol.sprite = selection.symbol;
            PlayerOneSymbol.color = activePlayerColor;
        }
        else
        {
            PlayerTwoSymbol.sprite = selection.symbol;
            PlayerTwoSymbol.color = inactivePlayerColor;

            if (gameBoard.BoardSize == 0)
            {
                gameBoard.SetPlayerSymbols(PlayerOneSymbol.sprite, PlayerTwoSymbol.sprite);
                gameBoard.CreateGameBoard(3);
            }
            else
            {
                ResetGame();
            }
        }
    }

    public void Reset(Message msg)
    {
        ResetGame();
    }

    private void ResetGame()
    {
        gameBoard.ResetBoard();

        SwitchState("WaitForPick");

        playerOne = true;
        PlayerOneSymbol.color = activePlayerColor;
        PlayerTwoSymbol.color = inactivePlayerColor;

        turnCount = 1;
    }

    void OnDestroy()
    {
        Messenger.GetInstance().UnregisterListener(new TileSelectedMsg(), WaitForPick);
        Messenger.GetInstance().UnregisterListener(new ResetGameMsg(), Reset);
        Messenger.GetInstance().UnregisterListener(new ResizeBoardMsg(), ResizeBoard);
        Messenger.GetInstance().UnregisterListener(new SymbolSelectionMsg(), SetPlayerSymbol);
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

    private void EnablePickState()
    {
        gameBoard.EnablePicks(true);
        if (playerOne == false)
        {
            StartCoroutine(aiPlayer.Pick());
        }
    }

    private void WaitForPick(Message msg)
    {
        TileSelectedMsg tileSelected = msg as TileSelectedMsg;
        lastSelectedTile = tileSelected.SelectedIndex;
        TileMB.TileState tileOwner = playerOne ? TileMB.TileState.PLAYER1 : TileMB.TileState.PLAYER2;
        gameBoard.SelectTile(lastSelectedTile, tileOwner);

        Debug.Log(string.Format("Tile {0} Was Selected", lastSelectedTile));
        SwitchState("Evaluate");

        gameBoard.EnablePicks(false);
    }

    private void EvaluateGameState()
    {
        evalResult = gameBoard.EvaluateBoard(lastSelectedTile);
        if (evalResult.winPresent || turnCount == gameBoard.MaxTurns)
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
            msg.winStartPos = evalResult.winTiles[0];
            msg.winEndPos = evalResult.winTiles[gameBoard.BoardSize - 1];
        }
        else
        {
            msg.winner = TileMB.TileState.OPEN;
        }

        Messenger.GetInstance().BroadCastMessage(msg);
    }

    #region Debug Code

    public struct DebugSettings
    {
        public int[] tileSelections;
        public float playBackSpeed;
    }

    public void PlayDebugGame(DebugSettings settings)
    {
        Messenger.GetInstance().BroadCastMessage(new ResetGameMsg());
        StartCoroutine(SelectTilesCoroutine(settings));
    }

    private IEnumerator SelectTilesCoroutine(DebugSettings settings)
    {
        WaitForSeconds delayBetweenMoves = new WaitForSeconds(1f / settings.playBackSpeed);
        int[] moves = settings.tileSelections;
        for (int i = 0; i < moves.Length; i++)
        {
            TileSelectedMsg msg = new TileSelectedMsg();
            msg.SelectedIndex = moves[i];
            Messenger.GetInstance().BroadCastMessage(msg);

            yield return delayBetweenMoves;
        }
    }
    #endregion
}
