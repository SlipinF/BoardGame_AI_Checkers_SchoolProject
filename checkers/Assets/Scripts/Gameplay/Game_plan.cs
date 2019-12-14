using System;
using System.Collections.Generic;
using UnityEngine;
using Minimax;
public enum TileType {invalid, empty, red, blue, yellow, orange, green, purple};
public enum Direction {N,E,S,W,NW,NE,SE,WS};

///<summary>
/// Game_plan class contains:
/// Setup method that instantiet board and paws on the screen 
/// PlayerInstantiator that recives number of players from mainmenu and instantient that many players in game.
/// Goal of this script is to instantiet board and pieces and manage logic of the game
///</summary>


public class Game_plan : MonoBehaviour
{  
    //Variables for Setup()
    public TileScript item;
    public GameObject piece;
    public const int maxRow = 13;
    public const int maxCol = 17;
    readonly TileScript Copy;
    //End of Variables for Setup()

    public TileScript[,] PositionArray = new TileScript[maxCol, maxRow]; //Visual representation of board           
    public List<Vector2Int> AiBoardCordinates = new List<Vector2Int>(); //Cordinates for Ai movement
    public bool onePlayerwon;
    public bool winningSpotChanged = false;
    public int PlayerNumber;

    public TwoPlayerBoard board = new TwoPlayerBoard();
    public Board startBoard = new Board();
    public Board boardForCompereson = new Board();


    //Singelton logic
    public static Game_plan Instance { get; private set; }

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


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            MoveAi();
        }
    }
     void Start()
    {
        PlayerNumber = MainMenu.NumberOfPlayers;
        if (PlayerNumber == 2)
        {
            startBoard.states = board.states;
            boardForCompereson.states = board.states;
        }
        if (MainMenu.startFromLoad)
        {
            Save_Load_Logic.LoadGame();
        }
        PlayerInstantiator();
        Setup(Copy);
    }
    void Setup(TileScript ProxyTile)
    {

        int CurrentRow = 0;
        int CurrentCol = 0;
        Board BoardForWinningSpots = new Board();
        TwoPlayerBoard playerTwoBoard = new TwoPlayerBoard();

            foreach (var cor in startBoard.states) 
            {
            int offset = new int();
            offset++;
            winningSpotChanged = false;
            if (CurrentRow % 2 == 0)
            {
                ProxyTile = Instantiate(item, new Vector2(CurrentCol + 0.5f * offset, CurrentRow), item.transform.rotation);
                PositionArray[CurrentRow, CurrentCol] = ProxyTile;
                
                

                 if (MainMenu.startFromLoad && PlayerNumber != 2)
                {
                    SetupPlayerNumber(BoardForWinningSpots.states[CurrentRow, CurrentCol], ProxyTile);
                    winningSpotChanged = true;
                }
                else if(MainMenu.startFromLoad && PlayerNumber == 2)
                {
                    SetupPlayerNumber(playerTwoBoard.states[CurrentRow, CurrentCol], ProxyTile);
                    winningSpotChanged = true;
                }
                ProxyTile._MyType = SetupPlayerNumber(cor, ProxyTile);
                ProxyTile._MyCore = new Vector2Int(CurrentCol, CurrentRow);
                TypeHandler(ProxyTile._MyType, ProxyTile, CurrentRow, CurrentCol, offset);
            }
            else if (CurrentRow % 2 != 0)
            {
            
            
               
               
               ProxyTile = Instantiate(item, new Vector2(CurrentCol, CurrentRow), item.transform.rotation);
                PositionArray[CurrentRow, CurrentCol] = ProxyTile;
                if (MainMenu.startFromLoad && PlayerNumber != 2)
                {
                    SetupPlayerNumber(BoardForWinningSpots.states[CurrentRow, CurrentCol], ProxyTile);
                    winningSpotChanged = true;
                }
                else if (MainMenu.startFromLoad && PlayerNumber == 2)
                {
                    SetupPlayerNumber(playerTwoBoard.states[CurrentRow, CurrentCol], ProxyTile);
                    winningSpotChanged = true;
                }
                ProxyTile._MyType = SetupPlayerNumber(cor, ProxyTile);
                ProxyTile._MyCore = new Vector2Int(CurrentCol, CurrentRow);
                TypeHandler(ProxyTile._MyType, ProxyTile, CurrentRow, CurrentCol, offset);
            }
            CurrentCol++;

            if (CurrentCol >= 13)
            {
                CurrentRow++;
                CurrentCol = 0;
            }
            if (CurrentRow >= 17)
            {
                CurrentRow = 0;
            }          
        }
    } //Instantiet board with offsets on odd rows. 

    void TypeHandler(TileType current, TileScript copy, int row, int col, int offset)
    {
        if (current == 0)
        {
            Destroy(copy.gameObject);
        }
        if (current != TileType.empty && current != TileType.invalid)
        {
            InstantiatePiece(col, row, offset, copy);
        }

    } // Take care of types of tiles and calls InstantiatePieces.

    void InstantiatePiece(int col, int row, int offset, TileScript tile)
    {

        GameObject CopyOfPiece = null;
        if (row % 2 == 0)
        {
            CopyOfPiece = Instantiate(piece, new Vector3(col + 0.5f * offset, row, -0.5f), piece.transform.rotation);
        }
        else
        {
            CopyOfPiece = Instantiate(piece, new Vector3(col, row, -0.5f), piece.transform.rotation);
        }

        CopyOfPiece.GetComponent<Renderer>().material.color = ColorChange(tile);

        for (int i = 0; i <= Movement.Instance.ListOfPlayers.Count - 1; i++)
        {
            if (tile._MyType == Movement.Instance.ListOfPlayers[i].id)
            {
                Movement.Instance.ListOfPlayers[i].ListOfPieces.Add(tile);
            }
        }
        tile._MyPiece = CopyOfPiece;
    } // Instantiet pieces on correct possions 

    Color ColorChange(TileScript tile)
    {
        int currentColor = (int)tile._MyType;
        switch (currentColor)
        {
            case 0:
                return Color.white;
            case 1:
                return Color.white;
            case 2:
                return Color.red;
            case 3:
                return Color.blue;
            case 4:
                return Color.yellow;
            case 5:
                return new Color(1, 0.5f, 0, 1);
            case 6:
                return Color.green;
            case 7:
                return new Color(0.5f, 0, 1, 1);
            default:
                return new Color32(0, 0, 0, 1);
        }
    }
    public bool WinCheck(Player p) // this script checks if the game was won by any player.
    {
        int Counter = 0;
        foreach (var piece in p.ListOfPieces)
        {
            foreach (var winningposition in p.WinningPositions)
            {

                if (piece._MyCore == winningposition._MyCore)
                {
                    Counter++;
                }
            }
        }
        if (Counter == 10 && Movement.Instance.ListOfPlayers.Count != 2)
        {
            Game_plan.Instance.onePlayerwon = true;
            return true;
        }
        else if(Counter == 15 && Movement.Instance.ListOfPlayers.Count == 2)
        {
            Game_plan.Instance.onePlayerwon = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    void PlayerInstantiator()
    {
        switch (PlayerNumber)
        {
            case 2:
                Player red = new Player(TileType.red);
                Movement.Instance.ListOfPlayers.Add(red);

                Player green = new Player(TileType.green);
                Movement.Instance.ListOfPlayers.Add(green);
                break;
            case 3:
                Player redCase3 = new Player(TileType.red);
                Movement.Instance.ListOfPlayers.Add(redCase3);

                Player blueCase3 = new Player(TileType.blue);
                Movement.Instance.ListOfPlayers.Add(blueCase3);

                Player yellowCase3 = new Player(TileType.yellow);
                Movement.Instance.ListOfPlayers.Add(yellowCase3);
                break;
            case 4:
                Player redCase4 = new Player(TileType.red);
                Movement.Instance.ListOfPlayers.Add(redCase4);

                Player greenCase4 = new Player(TileType.green);
                Movement.Instance.ListOfPlayers.Add(greenCase4);

                Player purpleCase4 = new Player(TileType.purple);
                Movement.Instance.ListOfPlayers.Add(purpleCase4);

                Player yellowCase4 = new Player(TileType.yellow);
                Movement.Instance.ListOfPlayers.Add(yellowCase4);
                break;
            case 6:
                for (int i = 0; i < PlayerNumber; i++)
                {
                    Player player = new Player((TileType)i + 2);
                    Movement.Instance.ListOfPlayers.Add(player);
                }
                break;
            default:
                break;
        }

        Movement.Instance.SetCurrentPlayer();

    } // instatiante different number of players depending on choosen number in menu


    /// <summary>
    /// SetupPlayerNumber sets cordinates recived from Setup() to empty in case if those coordinates don't match with colors of any active player.
    /// This method also populate WinningPoistion List with tiles.
    /// lastly this method sets PlayerWinningSpot in to correct value and populate startingPositions list with correct tiles.
    /// </summary>
     
     //I realize that this method is very long and could have been sperated into 3 different methods. 
    TileType SetupPlayerNumber(TileType cor, TileScript proxyTile)
    {
        switch (PlayerNumber)
        {
            case 2:
                if (cor == TileType.green && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red )
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 14;

                        }

                        if (player.id == TileType.green)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 0;
                        }

                        if (player.id == TileType.red)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                if (cor != TileType.red && cor != TileType.green && cor != TileType.invalid)
                {
                    cor = TileType.empty;
                }
                return cor;
            case 3:

                if (cor == TileType.green && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 0;
                        }
                    }
                }
                else if (cor == TileType.orange && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.blue)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 9;
                        }
                    }
                }
                else if (cor == TileType.purple && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }
                    }
                }
                else if (cor == TileType.red && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.yellow && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.blue && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.blue)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                if (cor != TileType.red && cor != TileType.blue && cor != TileType.yellow && cor != TileType.invalid)
                {
                    cor = TileType.empty;
                }
                return cor;
            case 4:
                if (cor == TileType.green && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }

                        if (player.id == TileType.green)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }
                        if (player.id == TileType.red)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.yellow && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.purple)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }
                        if (player.id == TileType.yellow)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.purple && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }
                        if (player.id == TileType.purple)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }

                    }
                }
                if (cor != TileType.red && cor != TileType.green && cor != TileType.purple && cor != TileType.yellow && cor != TileType.invalid)
                {
                    cor = TileType.empty;
                }
                return cor;
            case 6:
                if (cor == TileType.green && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 9;
                        }
                        if (player.id == TileType.green)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 0;
                        }
                       if (player.id == TileType.red)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.yellow && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.purple)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 6;
                        }
                       if (player.id == TileType.yellow)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.purple && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 3;
                        }
                        if (player.id == TileType.purple)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.blue && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.orange)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 0;
                        }
                       if (player.id == TileType.blue)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.orange && winningSpotChanged == false)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.blue)
                        {
                            player.WinningPositions.Add(proxyTile);
                            player.PlayerWinningSpot = 9;
                        }
                        if (player.id == TileType.orange)
                        {
                            player.StartingPositions.Add(proxyTile);
                        }
                    }
                }
                return cor;
            default:
                return cor;
        }
    }

   
    /// <summary>
    /// MoveAi() executes the Nimimax script. Sends in list of players except red player that is controlled by player.
    /// Calls changeVissibleBoard that  Comperes the difference between previous board and new recived board and moves pawns accordingly.
    /// </summary>

    public void MoveAi()
    {
        boardForCompereson = startBoard;
        List<IPlayer> ListOfPlayerForMiniMax = new List<IPlayer>();
        foreach (var playerForList in Movement.Instance.ListOfPlayers)
        {
            ListOfPlayerForMiniMax.Add(playerForList);
            if (playerForList != Movement.Instance.ListOfPlayers[0])
            {
                ListOfPlayerForMiniMax.Add(playerForList);
            }
        }

        foreach (var player in Movement.Instance.ListOfPlayers)
        {
            startBoard = (Board)MiniMax.Select(startBoard, Movement.Instance.currentPlayer, ListOfPlayerForMiniMax, Movement.Instance.PlayerTwo, 1, true);
            ChangeVisibleBoard(Movement.Instance.currentPlayer, boardForCompereson);

            if (Movement.Instance.currentPlayer != Movement.Instance.ListOfPlayers[0])
            {
                startBoard = (Board)MiniMax.Select(startBoard, Movement.Instance.currentPlayer, ListOfPlayerForMiniMax, Movement.Instance.PlayerTwo , 1 , true);
                ChangeVisibleBoard(Movement.Instance.currentPlayer, boardForCompereson);
            }
        }      
    }

    public void ChangeVisibleBoard(Player p, Board boardForExchange)
    {
;        AiBoardCordinates.Clear();
        for (int i = 0; i < boardForExchange.states.GetLength(0); i++)
        {
            for (int j = 0; j < boardForExchange.states.GetLength(1); j++)
            {
                if (boardForExchange.states[i, j] != startBoard.states[i, j])
                {
                    AiBoardCordinates.Add(new Vector2Int(i, j));
                }
            }
        }
        if (AiBoardCordinates.Count == 2)
        {
            TileScript firstTile = PositionArray[AiBoardCordinates[0].x, AiBoardCordinates[0].y];
            TileScript secondTile = PositionArray[AiBoardCordinates[1].x, AiBoardCordinates[1].y];

            foreach (var piece in p.ListOfPieces.ToArray())
            {
                if (piece._MyCore == firstTile._MyCore && secondTile._MyType == TileType.empty)
                {
                    Movement.Instance._MySelected = piece;
                    Movement.Instance.MovePiece(secondTile);
                }
                else if (piece._MyCore == secondTile._MyCore && firstTile._MyType == TileType.empty)
                {
                    Movement.Instance._MySelected = piece;
                    Movement.Instance.MovePiece(firstTile);
                }
            }
            AiBoardCordinates.Clear();
        }
    } //changes visible representation of the board, recives new board from Minimax.  

}




