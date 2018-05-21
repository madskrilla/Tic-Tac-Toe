using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardEvaluator
{
    private int boardSize = 0;
    TileMB[] curBoard;

    public struct WinInfo
    {
        public bool winPresent;
        public Transform winStart;
        public Transform winEnd;
    }
    private WinInfo winResult;

    public BoardEvaluator(int size)
    {
        boardSize = size;
    }

    public WinInfo EvaluateBoard(TileMB[] board, int lastSelection)
    {
        curBoard = board;
        winResult.winPresent = false;
        TileMB.TileState selectedState = board[lastSelection].SelectedState;
        int tileRow = lastSelection / boardSize;
        int tileCol = lastSelection % boardSize;
        //Check the Collumn and Row of The Last Selection
        CheckCollumnWin(tileCol, selectedState);
        CheckRowWin(tileRow, selectedState);
        //Always Check Diagonals
        CheckDiagonalWinLeft(selectedState);
        CheckDiagonalWinRight(selectedState);

        return winResult;
    }

    private bool CheckCollumnWin(int evalCol, TileMB.TileState evalState)
    {
        int tileIndex;
        for (int i = 0; i < boardSize; i++)
        {
            tileIndex = evalCol + (i * boardSize);
            if (curBoard[tileIndex].SelectedState != evalState)
            {
                return false;
            }
        }

        winResult.winPresent = true;
        Transform start = curBoard[evalCol + (0 * boardSize)].transform;
        Transform end = curBoard[evalCol + ((boardSize - 1) * boardSize)].transform;
        winResult.winStart = start;
        winResult.winEnd = end;

        return true;
    }

    private bool CheckRowWin(int evalRow, TileMB.TileState evalState)
    {
        int tileIndex;
        for (int i = 0; i < boardSize; i++)
        {
            tileIndex = i + (evalRow * boardSize);
            if (curBoard[tileIndex].SelectedState != evalState)
            {
                return false;
            }
        }

        winResult.winPresent = true;
        Transform start = curBoard[0 + (evalRow * boardSize)].transform;
        Transform end = curBoard[(boardSize - 1) + (evalRow * boardSize)].transform;
        winResult.winStart = start;
        winResult.winEnd = end;

        return true;
    }

    private bool CheckDiagonalWinLeft(TileMB.TileState evalState)
    {
        int tileIndex;
        for (int i = 0; i < boardSize; i++)
        {
            tileIndex = i + (i * boardSize);
            if (curBoard[tileIndex].SelectedState != evalState)
            {
                return false;
            } 
        }

        winResult.winPresent = true;
        Transform start = curBoard[0].transform;
        Transform end = curBoard[(boardSize - 1) + ((boardSize- 1) * boardSize)].transform;
        winResult.winStart = start;
        winResult.winEnd = end;

        return true;
    }

    private bool CheckDiagonalWinRight(TileMB.TileState evalState)
    {
        int tileIndex;
        int largestIndex = boardSize * boardSize - 1;
        //Start Eval At TopRight Tile And Move Down and Right Until i is Beyond the Board Bounds
        for (int i = boardSize - 1; i < largestIndex; i += boardSize -1)
        {
            tileIndex = i;
            if (curBoard[tileIndex].SelectedState != evalState)
            {
                return false;
            }
        }

        winResult.winPresent = true;
        Transform start = curBoard[(boardSize - 1)].transform;
        Transform end = curBoard[((boardSize - 1) * boardSize)].transform;
        winResult.winStart = start;
        winResult.winEnd = end;

        return true;
    }
}
