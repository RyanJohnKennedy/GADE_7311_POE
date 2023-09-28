using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmptyPiece : Piece
{
    public EmptyPiece()
    {
        team = Team.Neutral;
    }

    public override List<Vector2Int> Move(Piece[,] _pieces)
    {
        return moveableTiles;
    }
}
