using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : IPlayable
{
    private Team team;

    public BoardState MakeMove(BoardState boardState, Vector2Int move, PieceController movePiece)
    {
        return CheckState(boardState);
    }

    BoardState CheckState(BoardState board)
    {
        List<Piece> enemyPieces = new List<Piece>();
        List<Piece> teamPieces = new List<Piece>();
        List<Vector2Int> goalPos = new List<Vector2Int>();
        foreach (Piece p in board.Pieces)
        {
            if (p.team != team && p.team != Team.Neutral)
                enemyPieces.Add(p);
            else if (p.team == team)
                teamPieces.Add(p);
        }

        for (int i = 0; i < board.boardSize; i++)
        {
            for (int j = 0; j < board.boardSize; j++)
            {
                if(board.Board[i,j] == TileType.Goal)
                {
                    goalPos.Add(new Vector2Int(i, j));
                }
            }
        }

        //Defend
        float score = Rules.UtilityFunction(board, team);
        if (score < 0)
        {
            foreach (Piece p in enemyPieces)
            {
                List<Vector2Int> moveableTiles = p.Move(board.Pieces);
                foreach (Vector2Int tile in moveableTiles)
                {
                    if (board.Pieces[tile.x, tile.y] is BallPiece)
                    {
                        foreach (Vector2Int goal in goalPos)
                        {
                            if(p.position.x == goal.x || p.position.y == goal.y)
                                return States.Defending(board, p, team);
                        }
                    }
                }
            }

            foreach (Piece p in enemyPieces)
            {
                List<Vector2Int> moveableTiles = p.Move(board.Pieces);
                foreach (Vector2Int tile in moveableTiles)
                {
                    if(board.Pieces[tile.x, tile.y] is BallPiece)
                    {
                        return States.Defending(board, p, team);
                    }
                }
            }
        }//Attack
        else if(score > 0)
        {
            foreach (Piece p in teamPieces)
            {
                List<Vector2Int> moveableTiles = p.Move(board.Pieces);
                foreach (Vector2Int tile in moveableTiles)
                {
                    if (board.Pieces[tile.x, tile.y] is BallPiece)
                    {
                        foreach (Vector2Int goal in goalPos)
                        {
                            if ((p.position.x == goal.x && tile.x == p.position.x) || (p.position.y == goal.y && tile.y == p.position.y))
                            {
                                foreach (Piece enemyP in enemyPieces)
                                {
                                    if(enemyP.position.y == p.position.y || enemyP.position.x == p.position.x)
                                    {
                                        if(p.position.x > tile.x && p.position.x > enemyP.position.x)
                                        {
                                            return States.Neutral(board, team);
                                        }
                                        else if (p.position.x < tile.x && p.position.x < enemyP.position.x)
                                        {
                                            return States.Neutral(board, team);
                                        }
                                        else if (p.position.y > tile.y && p.position.y > enemyP.position.y)
                                        {
                                            return States.Neutral(board, team);
                                        }
                                        else if (p.position.y < tile.y && p.position.y < enemyP.position.y)
                                        {
                                            return States.Neutral(board, team);
                                        }
                                    }
                                }
                                return States.Attacking(board, p, team);
                            }
                        }
                    }
                }
            }
        }

        return States.Neutral(board, team);
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
