using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugWindow : EditorWindow
{
    private enum WIN_TYPES
    {
        HORIZONTAL = 0,
        VERTICAL,
        DIAGONAL,
        DRAW
    }

    private GameBoard gameBoard;
    private BigWinMB bigWin;
    private bool coinsActive, bigWinActive;

    private float playBackSpeed = 1f;

    private int selectedPlayer = 0;
    private int selectedWinType = 0;
    private int selectedWinLine = 0;
    private string[] playerChoices = new string[] { "Player One", "Player Two" };
    private string[] winChoices = new string[] { "Horizontal", "Vertical", "Diagonal", "Draw" };
    private string[] lineChoices = new string[] { "One", "Two", "Three", "Four" };
    private string[] diagonalChoices = new string[] { "Top to Bottom", "Bottom to Top" };

    [MenuItem("Window/Game Outcome Debugger")]
    static void Init()
    {
        DebugWindow window = EditorWindow.GetWindow(typeof(DebugWindow)) as DebugWindow;
        window.Show();
        window.titleContent = new GUIContent("Outcome Debugger");
    }

    private void OnGUI()
    {
        if (gameBoard == null)
        {
            gameBoard = GameObject.FindObjectOfType<GameBoard>();
        }

        if (bigWin == null)
        {
            bigWin = GameObject.FindObjectOfType<BigWinMB>();
        }

        DisplayOutcomeOptions();
    }

    private void DisplayOutcomeOptions()
    {
        GUILayout.Label("Player");
        selectedPlayer = GUILayout.SelectionGrid(selectedPlayer, playerChoices, playerChoices.Length);

        EditorGUILayout.Space();

        GUILayout.Label("Win Type");
        selectedWinType = GUILayout.SelectionGrid(selectedWinType, winChoices, 2);

        EditorGUILayout.Space();

        GUILayout.Label("Win Options");
        GUILayoutOption[] buttonOptions = new GUILayoutOption[] { GUILayout.MaxWidth(100f) };
        if (selectedWinType == 0 || selectedWinType == 1)
        {
            string label = selectedWinType == 0 ? "Row" : "Collumn";
            GUILayout.Label(label);
            string[] displayOptions = new string[gameBoard.BoardSize];
            for (int i = 0; i < displayOptions.Length; i++)
            {
                displayOptions[i] = lineChoices[i];
            }
            selectedWinLine = GUILayout.SelectionGrid(selectedWinLine, displayOptions, 1, buttonOptions);
        }
        else if (selectedWinType == 2)
        {
            GUILayout.Label("Direction");
            selectedWinLine = GUILayout.SelectionGrid(selectedWinLine, diagonalChoices, 1, buttonOptions);
        }
        else if (selectedWinType == 3)
        {
            //No Options
        }

        EditorGUILayout.Space();

        playBackSpeed = EditorGUILayout.Slider("Playback Speed", playBackSpeed, 1, 4, new GUILayoutOption[0]);

        if (Application.isPlaying && gameBoard.BoardSize != 0)
        {
            if (GUILayout.Button("Play"))
            {
                PlayDebugGame();
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("BigWin Animation");
        if (bigWinActive == false)
        {
            if (GUILayout.Button("Start", buttonOptions))
            {
                bigWin.ActivateBigWin();
                bigWinActive = true;
            } 
        }
        else
        {
            if (GUILayout.Button("Stop", buttonOptions))
            {
                bigWin.DeactivateBigWin();
                bigWinActive = false;
            }

        }

        EditorGUILayout.Space();

        GUILayout.Label("CoinSpew");

        if (coinsActive == false)
        {
            if (GUILayout.Button("Start", buttonOptions))
            {
                bigWin.ActivateCoins();
                coinsActive = true;
            }
        }
        else
        {
            if (GUILayout.Button("Stop", buttonOptions))
            {
                bigWin.DeactivateCoins();
                coinsActive = false;
            }

        }

    }

    private GameStateMachine.DebugSettings GenerateDebugGame()
    {
        GameStateMachine.DebugSettings settings = new GameStateMachine.DebugSettings();
        settings.playBackSpeed = playBackSpeed;

        int boardSize = gameBoard.BoardSize;
        List<int> winningMoves = new List<int>();

        switch ((WIN_TYPES)selectedWinType)
        {
            case WIN_TYPES.HORIZONTAL:
                {
                    for (int i = 0; i < boardSize; i++)
                    {
                        winningMoves.Add(i + (selectedWinLine * boardSize));
                    }
                    break;
                }
            case WIN_TYPES.VERTICAL:
                {
                    for (int i = 0; i < boardSize; i++)
                    {
                        winningMoves.Add(selectedWinLine + (i * boardSize));
                    }
                    break;
                }
            case WIN_TYPES.DIAGONAL:
                {
                    if (selectedWinLine == 0)
                    {
                        for (int i = 0; i < boardSize; i++)
                        {
                            winningMoves.Add(i + (i * boardSize));
                        }

                    }
                    else
                    {
                        int currTile = boardSize - 1;
                        for (int i = 0; i < boardSize; i++)
                        {
                            winningMoves.Add(currTile);
                            currTile += boardSize - 1;
                        }
                    }
                    break;
                }
            case WIN_TYPES.DRAW:
                {
                    if (boardSize == 3)
                    {
                        settings.tileSelections = new int[] { 0, 1, 4, 8, 5, 6, 7, 3, 2 };
                    }
                    else if (boardSize == 4)
                    {
                        settings.tileSelections = new int[] { 0, 1, 4, 8, 5, 6, 7, 3, 2, 9, 10, 11, 12, 13, 14, 15 };
                    }
                    return settings;
                }
            default:
                break;
        }

        //Generate losing move list and make sure it doesn't include a win
        List<int> losingMoves;
        do
        {
            losingMoves = RandomNumberUtil.GetRandomValues(0, boardSize * boardSize, boardSize, true, new List<int>(winningMoves));

        } while (CheckForWins(losingMoves));

        settings.tileSelections = CombineMoveLists(winningMoves, losingMoves);

        return settings;
    }

    private bool CheckForWins(List<int> moves)
    {
        //We are only checking for row and collumn wins since exclusions make it impossible to achieve a diagonal win
        bool colWin = true, rowWin = true;
        List<int> testMoves = new List<int>(moves);
        //Sorting to make 
        testMoves.Sort();

        int firstMove = testMoves[0];
        int col = firstMove % gameBoard.BoardSize;
        int lastMove = firstMove;

        for (int i = 1; i < testMoves.Count; i++)
        {
            if (colWin)
            {
                colWin = testMoves[i] % gameBoard.BoardSize == col;
            }
            if (rowWin)
            {
                rowWin = testMoves[i] == lastMove + 1;
            }
            lastMove = testMoves[i];
        }

        return colWin || rowWin;
    }

    private int[] CombineMoveLists(List<int> winner, List<int> loser)
    {
        int listSize = winner.Count + loser.Count;
        int[] result = new int[listSize];

        int curPlayer = 0;
        for (int i = 0; i < listSize; i++)
        {
            curPlayer = i % 2;
            if (curPlayer == selectedPlayer)
            {
                result[i] = winner[0];
                winner.RemoveAt(0);
            }
            else
            {
                result[i] = loser[0];
                loser.RemoveAt(0);
            }
        }

        return result;
    }

    private void PlayDebugGame()
    {
        GameStateMachine.DebugSettings settings = GenerateDebugGame();
        FindObjectOfType<GameStateMachine>().PlayDebugGame(settings);
    }
}
