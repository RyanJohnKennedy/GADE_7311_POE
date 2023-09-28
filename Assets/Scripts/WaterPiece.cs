using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPiece : Piece
{
    public WaterPiece()
    {
        team = Team.Neutral;
    }

    public override List<Vector2Int> Move(Piece[,] _pieces)
    {
        return moveableTiles;
    }
}
