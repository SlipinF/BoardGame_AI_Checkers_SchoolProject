using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    
    
    public int numberOfPlayers;
    public int[,] BoardState;
    public int[,] WinningPositions;
    public PlayerData(Board currentBoard)
    {
        numberOfPlayers = MainMenu.NumberOfPlayers;

        BoardState = new int[17,13];

        for (int i = 0; i < BoardState.GetLength(0); i++)
        {
            for (int j = 0; j < BoardState.GetLength(1); j++)
            {
                BoardState[i, j] = (int)currentBoard.states[i,j];
            }
        }
    }
}
