using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : IPlayable
{
    private Team team;
    public BoardState MakeMove(BoardState boardState, Vector2Int move, PieceController movePiece)
    {
        Piece chosenPiece = SelectPiece(boardState);
        List<Vector2Int> moveableTiles = chosenPiece.Move(boardState.Pieces);
        Vector2Int chosenTile = SelectTile(moveableTiles);

        return Rules.DoMoves(boardState, chosenTile, chosenPiece, team);
    }

    private Piece SelectPiece(BoardState boardState)
    {
        Piece chosenPiece;
        int Size = boardState.boardSize;

        chosenPiece = boardState.Pieces[Random.Range(0, Size), Random.Range(0, Size)];

        while(chosenPiece.team != team)
        {
            chosenPiece = boardState.Pieces[Random.Range(0, Size), Random.Range(0, Size)];
        }

        return chosenPiece;
    }

    private Vector2Int SelectTile(List<Vector2Int> moveableTiles)
    {
        Vector2Int chosenTile = moveableTiles[Random.Range(0, moveableTiles.Count)];

        return chosenTile;
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
