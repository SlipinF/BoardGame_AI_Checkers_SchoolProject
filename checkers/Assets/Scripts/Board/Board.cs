using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minimax;

///<summary>
/// Board class contains:
/// "blueprint" of board in 2D array called states.
/// Expand method for Minimax - this method loops through each piece of specifik player (currentPlayer) and creats instances of Board class for each new move made.
///  Value - this method callculates distance between each piece of currentPlayer and winning position for player. Returns value. 
///</summary>



public class Board : IState
{
    public TileType[,] states = new TileType[Game_plan.maxCol, Game_plan.maxRow] {
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.green,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.green,TileType.green,TileType.green,TileType.green,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.blue,TileType.blue,TileType.blue,TileType.blue, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.yellow,TileType.yellow,TileType.yellow},
      {TileType.invalid,TileType.blue,TileType.blue,TileType.blue, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.yellow,TileType.yellow},
      {TileType.invalid,TileType.blue,TileType.blue,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.yellow,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.blue,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.yellow,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.empty,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.purple,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.invalid},
      {TileType.invalid,TileType.purple,TileType.purple,TileType.empty, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.orange,TileType.invalid},
      {TileType.invalid,TileType.purple,TileType.purple,TileType.purple, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.orange,TileType.orange},
      {TileType.purple,TileType.purple,TileType.purple,TileType.purple, TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.empty,TileType.orange,TileType.orange,TileType.orange,TileType.orange},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.red,TileType.red,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.red,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.red,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      {TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid, TileType.invalid,TileType.invalid,TileType.red,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid,TileType.invalid},
      };

    public List<IState> Expand(IPlayer player)
    {
        List<IState> output = new List<IState>();
        List<TileScript> ListOfPossibleMoves = new List<TileScript>();
        Player playerForCheck = (Player)player;

        for (int i = 0; i < playerForCheck.ListOfPieces.Count; i++)
        {
            ListOfPossibleMoves = Movement.Instance.CreatListOfMovesForAI(playerForCheck.ListOfPieces[i]);

            for (int b = 0; b < ListOfPossibleMoves.Count; b++)
            {
                Board UniqBoard = new Board();

                for (int j = 0; j < UniqBoard.states.GetLength(0); j++)
                {
                    for (int y = 0; y < UniqBoard.states.GetLength(1); y++)
                    {
                        UniqBoard.states[j, y] = Game_plan.Instance.startBoard.states[j, y];
                    }
                }

                UniqBoard.states[playerForCheck.ListOfPieces[i]._MyCore.y, playerForCheck.ListOfPieces[i]._MyCore.x] = TileType.empty;
                UniqBoard.states[ListOfPossibleMoves[b]._MyCore.y, ListOfPossibleMoves[b]._MyCore.x] = playerForCheck.ListOfPieces[i]._MyType;
                output.Add(UniqBoard);
            }
            ListOfPossibleMoves.Clear();
        }
        return output;
    }

    public int currentValue { set; get; }

    public int Value(IPlayer p, IState state)
    {
        Player player = (Player)p;
        Board boardToEvaluate = (Board)state;
        int output = 0;

        for (int i = 0; i < boardToEvaluate.states.GetLength(0); i++)
        {
            for (int j = 0; j < boardToEvaluate.states.GetLength(1); j++)
            {
                if (boardToEvaluate.states[i, j] == player.id)
                {
                    int valueMulitplayer = (int)Vector2Int.Distance(Game_plan.Instance.PositionArray[i, j]._MyCore, player.WinningPositions[player.PlayerWinningSpot]._MyCore);

                    for (int x = 0; x < valueMulitplayer; x++)
                    {
                        output -= 10 * x;
                    }
                }
                foreach (var startingPosition in player.StartingPositions)
                {
                    if (boardToEvaluate.states[i, j] == startingPosition._MyType && new Vector2Int(j, i) == startingPosition._MyCore)
                    {
                        output -= 100;
                    }
                }
                foreach (var winningPosition in player.WinningPositions)
                {
                    if (boardToEvaluate.states[i, j] == player.id && new Vector2Int(j, i) == winningPosition._MyCore)
                    {
                        output += 1000;

                        if (winningPosition == player.WinningPositions[player.PlayerWinningSpot])
                        {
                            output *= 10;
                        }
                    }
                }
            }
        }
        return ((int)output);
    }
}
