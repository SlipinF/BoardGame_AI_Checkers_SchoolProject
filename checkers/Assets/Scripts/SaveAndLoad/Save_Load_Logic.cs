using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Load_Logic : MonoBehaviour
{
    public static void LoadGame() // This method uses playerData to move pawns on board. information is recived from file on the hardDrive.
    {
        PlayerData data = SaveSystem.LoadPlayer();
        for (int i = 0; i < Game_plan.Instance.startBoard.states.GetLength(0); i++)
        {
            for (int j = 0; j < Game_plan.Instance.startBoard.states.GetLength(1); j++)
            {
                Game_plan.Instance.startBoard.states[i, j] = (TileType)data.BoardState[i, j];
            }
        }
    }
    public void SavePlayer()
    {
        SaveSystem.SaveGame();
        MainMenu.ChangeSaveStatus();
    }
}
