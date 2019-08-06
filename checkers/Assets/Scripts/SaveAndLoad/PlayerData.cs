using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int numberOfPlayers;
    public float[] position;
    public PlayerData(List<Player> ListOfPlayers)
    {
        numberOfPlayers = MainMenu.NumberOfPlayers;
        position = new float[20 * ListOfPlayers.Count];
        int counter = 0;
        foreach (var players in ListOfPlayers)
        {
            for (int i = 0; i < players.ListOfPawns.Count; i++)
            {
                position[counter++] = players.ListOfPawns[i]._MyCore.x;
                position[counter++] = players.ListOfPawns[i]._MyCore.y;
            }
        }
    }

}
