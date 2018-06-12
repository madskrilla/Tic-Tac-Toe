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
        public Vector3[] winTiles;
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
        winResult.winTiles = new Vector3[boardSize];
        TileMB.TileState selectedState = board[lastSelection].SelectedState;
        int tileRow = lastSelection / boardSize;
        int tileCol = lastSelection % boardSize;
        //Check the Collumn and Row of The Last Selection
        if (CheckCollumnWin(tileCol, selectedState))
            return winResult;
        else if (CheckRowWin(tileRow, selectedState))
            return winResult;
        //Always Check Diagonals
        else if (CheckDiagonalWinLeft(selectedState))
            return winResult;
        else if (CheckDiagonalWinRight(selectedState))
            return winResult;

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
            winResult.winTiles[i] = curBoard[tileIndex].GetCenter();
        }

        winResult.winPresent = true;
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
            winResult.winTiles[i] = curBoard[tileIndex].GetCenter();
        }

        winResult.winPresent = true;
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
            winResult.winTiles[i] = curBoard[tileIndex].GetCenter();
        }

        winResult.winPresent = true;
        return true;
    }

    private bool CheckDiagonalWinRight(TileMB.TileState evalState)
    {
        int tileIndex;
        int largestIndex = boardSize * boardSize - 1;
        int listIndex = 0;
        //Start Eval At TopRight Tile And Move Down and Right Until i is Beyond the Board Bounds
        for (int i = boardSize - 1; i < largestIndex; i += boardSize - 1)
        {
            tileIndex = i;
            if (curBoard[tileIndex].SelectedState != evalState)
            {
                return false;
            }
            winResult.winTiles[listIndex] = curBoard[tileIndex].GetCenter();
            listIndex++;
        }

        winResult.winPresent = true;
        return true;
    }
}
