using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Piece
{
    public Team team;
    public List<Vector2Int> moveableTiles = new List<Vector2Int>();
    public Vector2Int position;

    public abstract List<Vector2Int> Move(Piece[,] _pieces);
}
