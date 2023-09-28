using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayable
{
    BoardState MakeMove(BoardState currentState, Vector2Int move, PieceController movePiece);
    void SetTeam(Team team);
}
