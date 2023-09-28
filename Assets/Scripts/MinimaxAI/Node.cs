using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public BoardState board;
    public float score;
    public Team team;
    public List<Node> children = new List<Node>();
    public Node parent;

    public Node(BoardState _board, Team _team)
    {
        board = _board;
        team = _team;
        parent = null;
        CalculateScore();
    }

    public Node(BoardState _board, Team _team, Node _parent)
    {
        board = _board;
        team = _team;
        parent = _parent;
        CalculateScore();
    }

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    public void CalculateScore()
    {
        score = Rules.UtilityFunction(board, team);
    }
}
