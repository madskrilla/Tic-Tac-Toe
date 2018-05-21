using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History
{
    private Dictionary<int, List<int>> tileSelectionHistory;
    private List<Dictionary<int, List<int>>> previousGames;

    public History()
    {
        previousGames = new List<Dictionary<int, List<int>>>();
    }

    public void StartNewGame()
    {
        if (tileSelectionHistory != null)
        {
            previousGames.Add(tileSelectionHistory);
        }
        tileSelectionHistory = new Dictionary<int, List<int>>();
    }

    public void AddMoveToRecord(int tileSelected, int player)
    {
        if (tileSelectionHistory.ContainsKey(player) == false)
        {
            tileSelectionHistory.Add(player, new List<int>());
        }
        tileSelectionHistory[player].Add(tileSelected);
    }
}
