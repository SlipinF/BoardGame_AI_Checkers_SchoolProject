using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This Script is responsible for all the movement made in the program. 
/// It holds all the Ai movement elements.
/// Goal with this script was to separate boards from movement in order to make Minimax functional.
/// 
/// The most important methods for player movement in this scipt are : 
/// MovePiece() that is managing all the moves made to the piece.
/// SelectionCheck() that handles on click selection, IndexCheck() that passes forward informations recived from SelectionCheck(), DirectionCheck()...
/// ...that makes check for every dirrection possible for piece and AddTileToList() that creats list of all possibles moves used later to determine...
/// ...where to move by player
/// Importants methods for Ai are :
/// CreatListOfMovesForAI that creats list of moves for all pieces of Ai player and MoveAi that handles movement of the AI
///</summary


public class Movement : MonoBehaviour {
    [SerializeField]
    private TileScript mySelected;
    public TileScript _MySelected { get { return mySelected; } set { mySelected = value; } }
    [SerializeField]
    public List<Player> ListOfPlayers = new List<Player>();
    [SerializeField]
    public List<TileScript> PossibleMoves = new List<TileScript>();
    public List<TileScript> PossibleMovesForAi = new List<TileScript>();
    public int playerIndex = 0;
    [SerializeField]
    public Player currentPlayer;
    public Player PlayerTwo;
    public Uicontroller methodcallback;

    
    //Singelton logic
    public static Movement Instance { get; private set; }
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;      
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    //End of Singelton logic

    public void MovePiece(TileScript tile)
    {
        if (tile._MyType != TileType.empty)
        {
            
        }
        else
        {
            Game_plan.Instance.startBoard.states[_MySelected._MyCore.y, _MySelected._MyCore.x] = tile._MyType;
            Game_plan.Instance.startBoard.states[tile._MyCore.y, tile._MyCore.x] = _MySelected._MyType;
            _MySelected._MyPiece.transform.position = tile.transform.position + new Vector3(0, 0, -0.5f);
            tile._MyPiece = _MySelected._MyPiece;
            currentPlayer.ListOfPieces.Remove(_MySelected);
            currentPlayer.ListOfPieces.Add(tile);

            _MySelected._MyPiece = null;
            tile._MyType = _MySelected._MyType;
            _MySelected._MyType = TileType.empty;
            _MySelected = null;


            ClearSelectedPieces();
            EndTurn();
        }
    }

    public List<TileScript> CreatListOfMovesForAI(TileScript currentPiece)
    {
        List<TileScript> PossibleMovesLocalList = new List<TileScript>();
        if (currentPiece._MyType != TileType.invalid && currentPiece._MyType != TileType.empty)
        {
            _MySelected = currentPiece;
            IndexCheck(currentPiece);

            for (int j = 0; j < PossibleMoves.Count; j++)
            {
                PossibleMovesLocalList.Add(PossibleMoves[j]);
            }
            ClearSelectedPieces();
            _MySelected = null;
            return PossibleMovesLocalList;
        }
    return PossibleMovesLocalList;
    }

    public void SelectionCheck(TileScript Tile)
    {
        if (Tile._MyType != TileType.empty)
        {
            if (currentPlayer.id == Tile._MyType)
            {
                if (_MySelected != Tile)
                {
                    _MySelected = Tile;
                    ClearSelectedPieces();
                    IndexCheck(Tile);
                }
            }
        }
        if (_MySelected != null && Tile._MyType == TileType.empty && PossibleMoves.Contains(Tile))
        {
            MovePiece(Tile);
        }
    }

    public void IndexCheck(TileScript Tile)
    {
        if (_MySelected == null)
        {
            return;
        }
        else
        {

            for (int i = 0; i <= (int)Direction.WS; i++)
            {
                DirectionCheck((Direction)i, Tile, true, false);
            }
            foreach (var positon in PossibleMoves)
            {
                positon.GetComponent<Renderer>().material.color = Color.green;
                positon.SetSelected(true);
            }
        }
    }

    void AddTileToList(List<TileScript> PossibleMovesToAdd, TileScript Tile, Direction TakenDirection, bool firstCheck, bool hasMoved)
    {
        if (PossibleMoves.Contains(Tile))
        {
            return;
        }


        if (Tile._MyType == TileType.empty)
        {
            if (firstCheck == true && hasMoved == false)
            {
                PossibleMoves.Add(Tile);
            }
            else if (firstCheck == false && hasMoved == true)
            {
                PossibleMoves.Add(Tile);
            }

            if (firstCheck == false && hasMoved == true)
            {
                for (int i = 0; i <= (int)Direction.WS; i++)
                {
                    DirectionCheck((Direction)i, Tile, false, false);
                }
            }

        }
        if (Tile._MyType != TileType.empty && Tile._MyType != TileType.invalid)
        {
            if (firstCheck == true && hasMoved == false)
            {
                DirectionCheck(TakenDirection, Tile, false, true);
            }
            else if (firstCheck == false && hasMoved == false)
            {
                DirectionCheck(TakenDirection, Tile, false, true);
            }
        }
    }

    public void ClearSelectedPieces()
    {
        foreach (var positon in PossibleMoves)
        {
            positon.GetComponent<Renderer>().material.color = Color.white;
            positon.SetSelected(false);
            if(positon._MyType != TileType.empty)
            {
              positon.SetClicked(false);
            }
        }
        PossibleMoves.Clear();
    }

    public void EndTurn()
    {
        if (Game_plan.Instance.WinCheck(currentPlayer))
        {
            methodcallback.DisplayEndScreen(currentPlayer);
        }
        playerIndex++;

        if (playerIndex >= ListOfPlayers.Count)
        {
            playerIndex = 0;
            PlayerTwo = ListOfPlayers[playerIndex + 1];
        }
        else if (playerIndex + 1 == ListOfPlayers.Count)
        {
            PlayerTwo = ListOfPlayers[0];
        }
        else
        {
            PlayerTwo = ListOfPlayers[playerIndex + 1];
        }
        currentPlayer = ListOfPlayers[playerIndex];
        PossibleMovesForAi.Clear();
        PossibleMoves.Clear();
        if (MainMenu.currentMode == Mode.VsAi && currentPlayer.id != TileType.red)
        {
            Game_plan.Instance.MoveAi();
        }
        methodcallback.ChangeThePlayerColorIcon((int)currentPlayer.id);
    } // manages playerIndex and resets it after it reaches last player

    public void SetCurrentPlayer()
    {
        currentPlayer = ListOfPlayers[playerIndex];
        PlayerTwo = ListOfPlayers[playerIndex + 1];
    }

    void DirectionCheck(Direction NewDirection, TileScript Tile, bool firstCheck, bool hasMoved)
    {
        int row = Tile._MyCore.x;
        int column = Tile._MyCore.y;

        try
        {
            switch (NewDirection)
            {
                case Direction.E:
                    AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column, row - 1], Direction.E, firstCheck, hasMoved);
                    break;
                case Direction.W:
                    AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column, row + 1], Direction.W, firstCheck, hasMoved);
                    break;
                case Direction.NW:
                    if (column % 2 != 0)
                    {
                     AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column + 1, row - 1], Direction.NW, firstCheck, hasMoved);
                    }
                    else
                    {
                     AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column + 1, row], Direction.NW, firstCheck, hasMoved);
                    }
                    break;
                case Direction.NE:
                    if (column % 2 != 0)
                    {
                     AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column + 1, row], Direction.NE, firstCheck, hasMoved);
                    }
                    else
                    {
                        AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column + 1, row + 1], Direction.NE, firstCheck, hasMoved);
                    }
                    break;
                case Direction.SE:
                    if (column % 2 != 0)
                    {
                        AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column - 1, row], Direction.SE, firstCheck, hasMoved);
                    }
                    else
                    {
                        AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column - 1, row + 1], Direction.SE, firstCheck, hasMoved);
                    }
                    break;
                case Direction.WS:
                    if (column % 2 != 0)
                    {
                        AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column - 1, row - 1], Direction.WS, firstCheck, hasMoved);
                    }
                    else
                    {
                        AddTileToList(PossibleMoves, Game_plan.Instance.PositionArray[column - 1, row], Direction.WS, firstCheck, hasMoved);
                    }
                    break;

                default:
                    break;

            }
        }
        catch (IndexOutOfRangeException e)
        {

        }
    } //makes direction check everytime piece is selected


}
