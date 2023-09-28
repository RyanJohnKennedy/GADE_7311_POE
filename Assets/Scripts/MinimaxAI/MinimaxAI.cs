using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI : IPlayable
{
    private Team team;
    string[,] bestBoard;
    float bestScore = -Mathf.Infinity;
    Node rootNode = null;
    Node currentNode = null;
    Node bestNode = null;
    int maxDepth = 4;
    List<Vector2Int> moveableTiles;

    public BoardState MakeMove(BoardState boardState, Vector2Int move, PieceController movePiece)
    {
        List<Piece> pieces = new List<Piece>();

        foreach (Piece p in boardState.Pieces)
        {
            pieces.Add(p);
        }
        BestMove(boardState, pieces);

        //bestBoard = bestNode.board.ConvertToString(bestNode.board.Pieces);
        //boardState.FillFromString(bestBoard);
        return boardState;
    }

    void AddMoves(BoardState boardstate, List<Piece> _pieces, bool thisTeam, Node node)
    {
        Debug.Log("AddMoves");

        List<Piece> pieces = new List<Piece>();

        foreach (Piece p in _pieces)
        {
            if(p.team == team && thisTeam) { pieces.Add(p); }
            else if(!thisTeam && p.team != Team.Neutral) { pieces.Add(p); }
        }

        for (int i = 0; i < pieces.Count; i++)
        {
            AddMove(boardstate, pieces[i], node);
        }
    }

    bool AddMove(BoardState boardstate, Piece piece, Node node)
    {
        Debug.Log("AddMove");
        Node tempNode;

        moveableTiles = piece.Move(boardstate.Pieces);
        List<Vector2Int> tempTiles = new List<Vector2Int>();

        foreach (Vector2Int tile in moveableTiles)
        {
            tempTiles.Add(tile);
        }

        foreach (Vector2Int tile in tempTiles)
        {
            string[,] piecesTemp = boardstate.ConvertToString(node.board.Pieces);
            BoardState testBoard = new BoardState(boardstate.boardSize, boardstate.goalNumber);
            testBoard.Board = boardstate.Board;
            testBoard.Pieces = RefillPieces(boardstate.Pieces);            

            Rules.DoMoves(testBoard, tile, piece, team);
            tempNode = new Node(testBoard, team, node);
            node.AddChild(tempNode);
        }

        return true;
    }

    void BestMove(BoardState boardstate, List<Piece> _pieces)
    {
        rootNode = new Node(boardstate, team);
        currentNode = rootNode;
        AddMoves(boardstate, _pieces, true, rootNode);

        //Children(0, boardstate, _pieces, currentNode, false);
        bestBoard = rootNode.children[1].board.ConvertToString(rootNode.children[1].board.Pieces);
        
        //Minimax(rootNode);
    }

    void Children(int depth, BoardState boardstate, List<Piece> _pieces, Node node, bool isTurn)
    {
        Debug.Log("Children");
        if (depth >= maxDepth)
        {
            foreach (Node n in node.children)
            {
                AddMoves(boardstate, _pieces, isTurn, n);
                Children(depth + 1, n.board, _pieces, n, !isTurn);
            }
        }
    }

    void Minimax(Node node)
    {
        CheckChildren(node);

        Node tempNode = null;
        while (tempNode == null || tempNode.parent != rootNode)
        {
            tempNode = bestNode.parent;
        }
    }

    void CheckChildren(Node node)
    {
        while(node.children.Count != 0)
        {

        }

        if(node.score > bestScore)
        {
            bestScore = node.score;
            bestNode = node;
        }

        if(node.children.Count > 0)
        {
            foreach (Node n in node.children)
            {
                CheckChildren(n);
            }
        }
    }

    public Piece[,] RefillPieces(Piece[,] _pieces)
    {
        Piece[,] pieces = new Piece[_pieces.GetLength(0), _pieces.GetLength(0)];

        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                if (_pieces[i,j] is EmptyPiece)
                {
                    pieces[i, j] = new EmptyPiece();
                }
                else if (_pieces[i, j] is MidFieldPiece && _pieces[i,j].team == Team.Blue)
                {
                    pieces[i, j] = new MidFieldPiece(Team.Blue, new Vector2Int(i, j));
                }
                else if (_pieces[i, j] is MidFieldPiece && _pieces[i, j].team == Team.Red)
                {
                    pieces[i, j] = new MidFieldPiece(Team.Red, new Vector2Int(i, j));
                }
                else if (_pieces[i, j] is StrikerPiece && _pieces[i, j].team == Team.Blue)
                {
                    pieces[i, j] = new StrikerPiece(Team.Blue, new Vector2Int(i, j));
                }
                else if (_pieces[i, j] is StrikerPiece && _pieces[i, j].team == Team.Red)
                {
                    pieces[i, j] = new StrikerPiece(Team.Red, new Vector2Int(i, j));
                }
                else if (_pieces[i, j] is BallPiece)
                {
                    pieces[i, j] = new BallPiece(new Vector2Int(i, j));
                }
            }
        }
        return pieces;
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
