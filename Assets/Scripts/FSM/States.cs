using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class States
{
    public static BoardState Attacking(BoardState board, Piece piece, Team team)
    {
        foreach (Piece p in board.Pieces)
        {
            if(p == piece)
            {
                List<Vector2Int> moveableTiles = p.Move(board.Pieces);

                foreach (Vector2Int tile in moveableTiles)
                {
                    if(board.Pieces[tile.x, tile.y] is BallPiece)
                    {
                        return board = Rules.DoMoves(board, tile, piece, team);
                    }
                }
            }
        }

        return board;
    }

    public static BoardState Defending(BoardState board, Piece piece, Team team)
    {
        float chance = 0.5f;
        Piece ball = null;
        List<Piece> teamPieces = new List<Piece>();

        foreach (Piece p in board.Pieces)
        {
            if(p is BallPiece)
            {
                ball = p;
                break;
            }
        }

        //Move Enemy piece
        foreach (Piece p in board.Pieces)
        {
            if(p.team == team)
            {
                teamPieces.Add(p);
            }
        }

        if (Random.Range(0f, 1f) < chance)
        {
            foreach (Piece p in teamPieces)
            {
                List<Vector2Int> moveableTiles = p.Move(board.Pieces);

                foreach (Vector2Int tile in moveableTiles)
                {
                    if (board.Pieces[tile.x, tile.y] == piece)
                    {
                        Rules.DoMoves(board, tile, p, team);
                        return board;
                    }
                }
            }
        }

        Vector2Int ClosestGoal;
        List<Vector2Int> goalPos = new List<Vector2Int>();

        for (int i = 0; i < board.boardSize; i++)
        {
            for (int j = 0; j < board.boardSize; j++)
            {
                if (board.Board[i, j] == TileType.Goal)
                {
                    goalPos.Add(new Vector2Int(i, j));
                }
            }
        }

        foreach (Piece p in board.Pieces)
        {
            if (p is BallPiece)
            {
                float tempDis = Mathf.Infinity;

                foreach (Vector2Int goal in goalPos)
                {
                    if (tempDis < Vector2.Distance(p.position, goal)) 
                    { 
                        tempDis = Vector2.Distance(p.position, goal);
                        ClosestGoal = goal;
                    }
                }
            }
        }

        //intercept enemy piece
        foreach (Piece p in teamPieces)
        {
            List<Vector2Int> moveableTiles = p.Move(board.Pieces);
            foreach (Vector2Int tile in moveableTiles)
            {
                List<Vector2Int> enemyTiles = piece.Move(board.Pieces);
                List<Vector2Int> ballTiles = ball.Move(board.Pieces);
                foreach (Vector2Int eTile in enemyTiles)
                {
                    if(eTile == tile)
                    {
                        foreach (Vector2Int bTile in ballTiles)
                        {
                            if(bTile == tile)
                            {
                                return Rules.DoMoves(board, tile, p, team);
                            }
                        }
                    }
                }
            }
        }

        return Neutral(board, team);
    }

    public static BoardState Neutral(BoardState board, Team team)
    {
        Piece ball = null;
        List<Piece> teamPieces = new List<Piece>();

        foreach (Piece p in board.Pieces)
        {
            if (p is BallPiece)
            {
                ball = p;
                break;
            }
        }

        foreach (Piece p in board.Pieces)
        {
            if (p.team == team)
            {
                teamPieces.Add(p);
            }
        }

        foreach (Piece p in teamPieces)
        {
            bool tryPiece = true;
            List<Vector2Int> moveableTiles = p.Move(board.Pieces);
            List<Vector2Int> ballTiles = ball.Move(board.Pieces);

            foreach (Vector2Int tile in moveableTiles)
            {
                if(board.Pieces[tile.x, tile.y] is BallPiece)
                {
                    tryPiece = false;
                    break;
                }
            }

            if (tryPiece)
            {
                foreach (Vector2Int tile in moveableTiles)
                {
                    foreach (Vector2Int bTile in ballTiles)
                    {
                        if(bTile == tile)
                        {
                            return Rules.DoMoves(board, tile, p, team);
                        }
                    }
                }
            }
        }

        return board;
    }
}
