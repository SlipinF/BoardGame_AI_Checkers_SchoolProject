using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Minimax;
public enum TileType {invalid, empty, taken, red, blue, yellow, orange, green, purple};
public enum Direction {N,E,S,W,NW,NE,SE,WS};


///<summary>
/// Game_plan class contains:
/// Setup method that instantiet board and paws on the screen 
/// PlayerInstantiator that recives amount of players from ' and instantient that many players in game.
/// 
/// This class was supposed to operate as GameManager for program, instantiet and operate with visual part of the game.
/// 
/// 
/// 
/// Goal of this script is to instantiet board and pawns and manage logic of the game
/// 
///</summary>


public class Game_plan : MonoBehaviour 
{
    //Variables for Setup()
    public TileScript item;
    public GameObject pawn;
    public const int maxRow = 13;
    public const int maxCol = 17;
    readonly TileScript Copy;
    //End of Variables for Setup()


    public TileScript[,] PositionArray = new TileScript[maxCol,maxRow]; //Visual representation of board           
    public List<Vector2Int> AiBoardCordinates = new List<Vector2Int>(); //Cordinates for Ai movement
    public bool onePlayerwon;
    public int PlayerAmount; // Int for PlayerInstantiator send from MainMenu buttons, it is set to 6 because MainMenu methods didn't work as i planned.

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

    void Start()
    {
    PlayerAmount =  MainMenu.NumberOfPlayers;
    if(PlayerAmount == 2)
    {
     startBoard.states = TwoPlayerBoard.states;
    }
    if (MainMenu.startFromLoad)
    {
     Save_Load_Logic.LoadGame();
    }
     PlayerInstantiator();
     Setup(Copy);
     CreatStartingPositions();
    }
    void Setup(TileScript ProxyTile)
    {

     int CurrentRow = 0;
     int CurrentCol = 0;
  

    foreach (var cor in startBoard.states)
        {
            int offset = new int();
            offset++;
            if (CurrentRow % 2 == 0)
            {
                ProxyTile = Instantiate(item, new Vector2(CurrentCol + 0.5f * offset, CurrentRow), transform.rotation);
                PositionArray[CurrentRow, CurrentCol] = ProxyTile;
                ProxyTile._MyType = SetupPlayerAmount(cor, ProxyTile);
                ProxyTile._MyCore = new Vector2Int(CurrentCol, CurrentRow);
                TypeHandler(ProxyTile._MyType, ProxyTile, CurrentRow, CurrentCol, offset);
            }
            else if (CurrentRow % 2 != 0)
            {
                ProxyTile = Instantiate(item, new Vector2(CurrentCol, CurrentRow), transform.rotation);
                PositionArray[CurrentRow, CurrentCol] = ProxyTile;
                ProxyTile._MyType = SetupPlayerAmount(cor, ProxyTile);
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
          InstantiatePawn(col, row, offset, copy);
        }

    } // Take care of types of tiles and calls InstantiatePawn.

    void InstantiatePawn(int col, int row, int offset, TileScript tile)
    {

        GameObject CopyOfPawn = null;
        if (row % 2 == 0)
        {
            CopyOfPawn = Instantiate(pawn, new Vector3(col + 0.5f * offset, row, -0.5f), transform.rotation);
        }
        else
        {
            CopyOfPawn = Instantiate(pawn, new Vector3(col, row, -0.5f), transform.rotation);
        }

        CopyOfPawn.GetComponent<Renderer>().material.color = ColorChange(tile);
        
        for (int i = 0; i <= Movement.Instance.ListOfPlayers.Count -1; i++)
        {
            if(tile._MyType == Movement.Instance.ListOfPlayers[i].id)
            {
                Movement.Instance.ListOfPlayers[i].ListOfPawns.Add(tile);
            }
        }
        tile._MyPiece = CopyOfPawn;
    } // Instantiet pawns on correct possions 
 
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
                return Color.white;
            case 3:
                return Color.red;
            case 4:
                return Color.blue;
            case 5:
                return Color.yellow;
            case 6:
                return new Color(1, 0.5f, 0, 1);
            case 7:
                return Color.green;
            case 8:
                return new Color(0.5f, 0, 1, 1);

            default:
                return new Color32(0, 0, 0, 1);
        }
    }


    public bool WinCheck(Player p) // this script checks if the game was won by any player.
    {
        int Counter = 0;
        foreach (var pawn in p.ListOfPawns)
        {
            foreach (var winningposition in p.WinningPositions)
            {

                if (pawn._MyCore == winningposition._MyCore)
                {
                    Counter++;
                }
            }
        }
        if (Counter == 10)
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
            switch (PlayerAmount)
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
                    for (int i = 0; i < PlayerAmount; i++)
                    {
                        Player player = new Player((TileType)i + 3);
                        Movement.Instance.ListOfPlayers.Add(player);
                    }
                    break;
                default:
                    break;
            }

        Movement.Instance.currentPlayer = Movement.Instance.ListOfPlayers[Movement.Instance.playerIndex];
        Movement.Instance.PlayerTwo = Movement.Instance.ListOfPlayers[Movement.Instance.playerIndex + 1];   
    } // instatiante different amout of players depending on choosen amount in menu

    void CreatStartingPositions()
    {
        foreach (var player in Movement.Instance.ListOfPlayers)
        {
            foreach (var pawn in player.ListOfPawns)
            {
                player.StartingPositions.Add(pawn);
            }
        }
    }
        TileType SetupPlayerAmount(TileType cor,TileScript proxyTile)
    {
        switch (PlayerAmount)
        {
            case 2:
                if (cor == TileType.green)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                if (cor != TileType.red && cor != TileType.green && cor != TileType.invalid)
                {                   
                    cor = TileType.empty;
                }
                return cor;
            case 3:

                if (cor == TileType.green)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.orange)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.blue)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.purple)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                if (cor != TileType.red && cor != TileType.blue && cor != TileType.yellow && cor != TileType.invalid)
                {
                    cor = TileType.empty;
                }
                return cor;
            case 4:
                if (cor == TileType.green)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.yellow)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.purple)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.purple)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                if (cor != TileType.red && cor != TileType.green && cor != TileType.purple && cor != TileType.yellow && cor != TileType.invalid)
                {
                    cor = TileType.empty;
                }
                return cor;
            case 6:
                if (cor == TileType.green)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.red)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.yellow)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.purple)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.purple)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.yellow)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.blue)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.orange)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (cor == TileType.orange)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.blue)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                return cor;
            default:
                return cor;
        }
    } // this is way to long. Fix that 
        void SetupPlayerWinningPositions(TileScript proxyTile)
        {
            switch (PlayerAmount)
            {
            case 2:
                if(proxyTile._MyType == TileType.red)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if(player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                        } 
                    }
                }else if (proxyTile._MyType == TileType.green)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.red)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                break;
            case 3:
                if (proxyTile._MyType == TileType.red)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.green)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (proxyTile._MyType == TileType.blue)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.orange)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                else if (proxyTile._MyType == TileType.yellow)
                {
                    foreach (var player in Movement.Instance.ListOfPlayers)
                    {
                        if (player.id == TileType.purple)
                        {
                            player.WinningPositions.Add(proxyTile);
                        }
                    }
                }
                break;
            case 4:
                break;
            case 6:
                break;
            default:
                break;
        }
    }
        

    public void MoveAi() //Move AI - Is called in from TileScript
    {
        foreach (var player in Movement.Instance.ListOfPlayers)
        {
            if (Movement.Instance.currentPlayer != Movement.Instance.ListOfPlayers[0])
            {
                boardForCompereson = startBoard;
                startBoard = (Board)MiniMax.Select(startBoard, Movement.Instance.currentPlayer, Movement.Instance.PlayerTwo, 2, true);
                ChangeVisibleBoard(Movement.Instance.currentPlayer, boardForCompereson);
            }
        }
    }
    public void ChangeVisibleBoard(Player p, Board boardForExchange)
    {
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
        if (AiBoardCordinates.Count != 2)
        {
            return;
        }

        TileScript firstTile = PositionArray[AiBoardCordinates[0].x, AiBoardCordinates[0].y];
        TileScript secondTile = PositionArray[AiBoardCordinates[1].x, AiBoardCordinates[1].y];

        foreach (var pawn in p.ListOfPawns.ToArray())
        {
            if (pawn._MyCore == firstTile._MyCore && secondTile._MyType == TileType.empty)
            {
                Movement.Instance._MySelected = pawn;
                Movement.Instance.MovePiece(secondTile);
            }
            else if (pawn._MyCore == secondTile._MyCore && firstTile._MyType == TileType.empty)
            {
                Movement.Instance._MySelected = pawn;
                Movement.Instance.MovePiece(firstTile);
            }
        }
        AiBoardCordinates.Clear();
    } //changes visible representation of the board, recives new board from Minimax.  
}

///<summary>
/// Player class contains:
/// -player id and it supplies Minimax with Iplayer interface.
/// -ListOfPawns used for calculeting all possible moves for each player
/// -WinningPositions- list that contains all winning positions for player
///</summary>



[Serializable]
public class Player : IPlayer
{
    public List<TileScript> ListOfPawns = new List<TileScript>();
    public List<TileScript> WinningPositions = new List<TileScript>();
    public List<TileScript> StartingPositions = new List<TileScript>();


    public int winningSpotNumber;
    public TileType id;

    public Player(TileType id)
    {
        this.id = id;
    }
}

///<summary>
/// Board class contains:
/// "blueprint" of board in 2D array called states.
/// Expand method for Minimax - this method loops through each pawn of specifik player (currentPlayer) and creats instances of Board class for each new move made.
///  Value - this method callculates distance between each pawn of currentPlayer and winning position for player. Returns value. 
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


        for (int i = 0; i < Movement.Instance.currentPlayer.ListOfPawns.Count; i++)
        {
            Movement.Instance.CreatListOfMovesForAI(Movement.Instance.currentPlayer.ListOfPawns[i]);
            for (int b = 0; b < Movement.Instance.PossibleMovesForAi.Count; b++)
            {
                Board UniqBoard = new Board();

                for (int j = 0; j < UniqBoard.states.GetLength(0); j++)
                {
                    for (int y = 0; y < UniqBoard.states.GetLength(1); y++)
                    {
                        UniqBoard.states[j, y] = Game_plan.Instance.startBoard.states[j, y];
                    }
                }

                UniqBoard.states[Movement.Instance.currentPlayer.ListOfPawns[i]._MyCore.y, Movement.Instance.currentPlayer.ListOfPawns[i]._MyCore.x] = TileType.empty;
                UniqBoard.states[Movement.Instance.PossibleMovesForAi[b]._MyCore.y, Movement.Instance.PossibleMovesForAi[b]._MyCore.x] = Movement.Instance.currentPlayer.ListOfPawns[i]._MyType;
                output.Add(UniqBoard);
            }
            Movement.Instance.PossibleMovesForAi.Clear();
        }
        return output;
    }

    public int currentValue { set; get; }

    public int Value(IPlayer p)

    {
        Player player = (Player)p;

        int RandomNumberForValue = UnityEngine.Random.Range(0, 9);

        int output = 0;

        foreach (var piece in player.ListOfPawns)
        {
            foreach (var winningSpot in player.WinningPositions)
            {
                if (piece._MyCore == winningSpot._MyCore)
                {
                    output += 100;
                }
            }

            output -= (int)Vector3.Distance(piece.transform.position, player.WinningPositions[RandomNumberForValue].transform.position);

            foreach (var startPieces in player.StartingPositions)
            {
                if (piece._MyCore != startPieces._MyCore)
                {
                    output += (10 + (int)Vector3.Distance(piece.transform.position, startPieces.transform.position));

                }
                else if (piece._MyCore == startPieces._MyCore)
                {
                    output -= 100;
                }
            }
        }
        return ((int)output);
    }
}