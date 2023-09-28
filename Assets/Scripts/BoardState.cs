using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Neutral,
    Red,
    Blue
}

public enum TileType
{
    Empty,
    Water,
    Rocks,
    MoveableRed,
    MoveableBlue,
    Goal
}

[Serializable]
public class BoardState
{
    public int boardSize;
    public int goalNumber;
    public GameController gc;

    private TileType[,] board;

    public TileType[,] Board
    {
        get { return board; }
        set { board = value; }
    }

    private Piece[,] pieces;

    public Piece[,] Pieces
    {
        get { return pieces; }
        set { pieces = value; }
    }

    public int redScore = 0;
    public int blueScore = 0;

    public BoardState(int _boardSize, int _goalNumber)
    {
        boardSize = _boardSize;
        goalNumber = _goalNumber;

        board = new TileType[boardSize, boardSize];
        pieces = new Piece[boardSize, boardSize];

        //InitialiseBoard();
        //PlacePieces();
    }

    public BoardState(int _boardSize, int _goalNumber, GameController _gc)
    {
        boardSize = _boardSize;
        goalNumber = _goalNumber;
        gc = _gc;

        board = new TileType[boardSize, boardSize];
        pieces = new Piece[boardSize, boardSize];

        InitialiseBoard();
        PlacePieces();
    }

    public void UpdateScores(int _redScore, int _blueScore)
    {
        redScore = _redScore;
        blueScore = _blueScore;
    }

    public void InitialiseBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                board[x, z] = TileType.Empty;
            }
        }

        PlaceGoals();
    }

    public void ReInitialiseBoard()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                if(board[x, z] == TileType.MoveableBlue || board[x, z] == TileType.MoveableRed)
                {
                    board[x, z] = TileType.Empty;
                }
            }
        }

        PlaceGoals();
    }

    public void PlaceGoals()
    {
        int middle = boardSize / 2;

        board[0, middle] = TileType.Goal;
        board[boardSize - 1, middle] = TileType.Goal;
    }

    public void ColourTiles(List<Vector2Int> _colourTiles, Team team)
    {
        ReInitialiseBoard();

        foreach (Vector2Int tilePos in _colourTiles)
        {
            if (team == Team.Blue)
                board[tilePos.x, tilePos.y] = TileType.MoveableBlue;
            else
                board[tilePos.x, tilePos.y] = TileType.MoveableRed;
        }
    }

    public void PlacePieces()
    {
        int middle = boardSize / 2;

        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                pieces[x, z] = new EmptyPiece();
            }
        }

        //Place blue peices
        pieces[middle, 0] = new MidFieldPiece(Team.Blue, new Vector2Int(middle, 0));
        pieces[middle - 1, 0] = new MidFieldPiece(Team.Blue, new Vector2Int(middle - 1, 0));
        pieces[middle + 1, 0] = new MidFieldPiece(Team.Blue, new Vector2Int(middle + 1, 0));

        //pieces[0, 0] = new StrikerPiece(Team.Blue, new Vector2Int(0, 0));
        //pieces[boardSize - 1, 0] = new StrikerPiece(Team.Blue, new Vector2Int(boardSize - 1, 0));

        //Place red pieces
        pieces[middle, boardSize - 1] = new MidFieldPiece(Team.Red, new Vector2Int(middle, boardSize - 1));
        pieces[middle - 1, boardSize - 1] = new MidFieldPiece(Team.Red, new Vector2Int(middle - 1, boardSize - 1));
        pieces[middle + 1, boardSize - 1] = new MidFieldPiece(Team.Red, new Vector2Int(middle + 1, boardSize - 1));

        //pieces[boardSize - 1, boardSize - 1] = new StrikerPiece(Team.Red, new Vector2Int(boardSize - 1, boardSize - 1));
        //pieces[0, boardSize - 1] = new StrikerPiece(Team.Red, new Vector2Int(0, boardSize - 1));

        //Place balls
        pieces[middle, middle] = new BallPiece(new Vector2Int(middle, middle));
    }

    public void ResetBall(Vector2Int ballPos)
    {
        int middle = boardSize / 2;
        pieces[ballPos.x, ballPos.y] = new EmptyPiece();
        
        if(pieces[middle, middle] is EmptyPiece)
        {
            pieces[middle, middle] = new BallPiece(new Vector2Int(middle, middle));
        }
        else if(pieces[middle, middle].team == Team.Blue)
        {
            pieces[middle, middle - 1] = new BallPiece(new Vector2Int(middle, middle - 1));
        }
        else
        {
            pieces[middle, middle + 1] = new BallPiece(new Vector2Int(middle, middle + 1));
        }
    }

    public string[,] ConvertToString(Piece[,] pieces)
    {
        string[,] stringPieces = new string[pieces.GetLength(0), pieces.GetLength(1)];

        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                if(pieces[i,j] is EmptyPiece)
                {
                    stringPieces[i, j] = "_";
                }
                else if(pieces[i, j] is MidFieldPiece)
                {
                    if(pieces[i, j].team == Team.Blue)
                    {
                        stringPieces[i, j] = "B";
                    }
                    else
                    {
                        stringPieces[i, j] = "R";
                    }
                }
                else if (pieces[i, j] is StrikerPiece)
                {
                    if (pieces[i, j].team == Team.Blue)
                    {
                        stringPieces[i, j] = "BS";
                    }
                    else
                    {
                        stringPieces[i, j] = "RS";
                    }
                }
                else if(pieces[i, j] is BallPiece)
                {
                    stringPieces[i, j] = "A";
                }
            }
        }

        return stringPieces;
    }

    public BoardState FillFromString(string[,] stringPieces, BoardState board)
    {
        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                if(stringPieces[i,j] == "_")
                {
                    board.pieces[i, j] = new EmptyPiece();
                }
                else if(stringPieces[i, j] == "B")
                {
                    board.pieces[i, j] = new MidFieldPiece(Team.Blue, new Vector2Int(i, j));
                }
                else if (stringPieces[i, j] == "R")
                {
                    board.pieces[i, j] = new MidFieldPiece(Team.Red, new Vector2Int(i, j));
                }
                else if (stringPieces[i, j] == "A")
                {
                    board.pieces[i, j] = new BallPiece(new Vector2Int(i, j));
                }
            }
        }

        return board;
    }

    public void CheckWinPoint()
    {
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                if (Pieces[x, z] is BallPiece)
                {
                    if (Board[x, z] == TileType.Goal)
                    {
                        if (gc.currentTeam == Team.Blue) { gc.BlueTeamScore++; }
                        else { gc.RedTeamScore++; }

                        ResetBall(new Vector2Int(x, z));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

}
