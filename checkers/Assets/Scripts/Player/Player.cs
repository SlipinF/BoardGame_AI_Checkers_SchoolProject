
using System.Collections.Generic;
using Minimax;

///<summary>
/// Player class contains:
/// -player id and it supplies Minimax with Iplayer interface.
/// -ListOfPieces used for calculeting all possible moves for each player
/// -WinningPositions- list that contains all winning positions for player
///</summary>

public class Player : IPlayer
{
   public List<TileScript> ListOfPieces = new List<TileScript>();
   
   public List<TileScript> WinningPositions = new List<TileScript>();
    
   public List<TileScript> StartingPositions = new List<TileScript>();
    
   public int PlayerWinningSpot;
   
   public TileType id;

    public Player(TileType id)
    {
        this.id = id;
    }
}
