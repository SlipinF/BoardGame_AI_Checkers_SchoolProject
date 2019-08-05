using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This scipts can be found on every tile in the game, it holds multiple properties needed in other scripts and calls SelectionCheck OnMouseDown
/// It is a building block for every other method in the program. 
///</summary

public class TileScript : MonoBehaviour {

    [SerializeField]
    private TileType myType;
    public TileType _MyType { get { return myType; } set { myType = value;}}
    public GameObject _MyPiece { get; set; }
    public Vector2Int _MyCore { get; set; }
    

    void OnMouseDown()
    {
        Movement.Instance.SelectionCheck(this);

        if (MainMenu.currentMode == Mode.VsAi)
        {
            Game_plan.Instance.MoveAi();
        }
    }
    
}
