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

    private bool selected;
    private bool clicked;
    private bool goUp;
    private bool goUpPiece;

    private void Update()
    {
        if(selected)
        {    
            if(goUp)
            {            
             this.transform.position = Lerp(this.transform.position, transform.position -= new Vector3(0, 0, 0.2f), 0.02f);
                if (transform.position.z <= Random.Range(-0.1f,-.4f))
                {
                    goUp = false;
                }
            }
            else
            {
                this.transform.position = Lerp(this.transform.position, transform.position += new Vector3(0, 0, 0.2f), 0.02f);
                if (transform.position.z >= Random.Range(0.1f, .4f))
                {
                    goUp = true;
                }
            }
        }
        if(clicked == true && _MyPiece != null )
        {
            if (goUpPiece)
            {
                _MyPiece.transform.position = Lerp(_MyPiece.transform.position, _MyPiece.transform.position -= new Vector3(0, 0, 0.3f), 0.02f);
                if (_MyPiece.transform.position.z <= Random.Range(-0.41f, -.6f))
                {
                    goUpPiece = false;
                }
            }
            else
            {
                _MyPiece.transform.position = Lerp(_MyPiece.transform.position, _MyPiece.transform.position += new Vector3(0, 0, 0.1f), 0.02f);
                if (_MyPiece.transform.position.z >= Random.Range(-0.4f, -.2f))
                {
                    goUpPiece = true;
                }
            }         
        }
    }

    void OnMouseDown()
    {
        Movement.Instance.SelectionCheck(this);
        if(selected != true && _MyType != TileType.empty)
        {
          SetClicked(true);
        }
    }
    public static Vector3 Lerp(Vector3 start, Vector3 finish, float percentage)
    {
        //Make sure percentage is in the range [0.0, 1.0]
        percentage = Mathf.Clamp01(percentage);

        //(finish-start) is the Vector3 drawn between 'start' and 'finish'
        Vector3 startToFinish = finish - start;

        //Multiply it by percentage and set its origin to 'start'
        return start + startToFinish * percentage;    
    }
   public void SetSelected(bool state)
   {
        selected = state;
   }
    public void SetClicked(bool state)
    {
        clicked = state;
    }
}
