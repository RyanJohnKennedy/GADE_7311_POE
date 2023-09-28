using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : IPlayable
{
    private Team team;

    public BoardState MakeMove(BoardState boardState, Vector2Int move, PieceController movePiece)
    {
        return Rules.DoMoves(boardState, move, movePiece.connectedPiece, team);
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
