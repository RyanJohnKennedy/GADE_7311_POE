using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public bool Selected = false;
    public Piece connectedPiece;
    public List<Vector2Int> moveLocations;
    public GameController GC;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (connectedPiece.team == GC.currentTeam)
        {
            moveLocations.Clear();
            moveLocations = connectedPiece.Move(GC.board.Pieces); //, new Vector2Int((int)transform.position.x, (int)transform.position.z)
            GC.selectedPiece = this;
            GC.board.ColourTiles(moveLocations, connectedPiece.team);
            GC.InstantiateTiles();
        }
    }
}