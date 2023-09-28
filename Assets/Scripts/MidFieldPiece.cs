using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MidFieldPiece : Piece
{
    public MidFieldPiece(Team _team, Vector2Int _position)
    {
        team = _team;
        position = _position;
    }

    public override List<Vector2Int> Move(Piece[,] _pieces)
    {
        moveableTiles = new List<Vector2Int>();
        if (position.x != 0)
        {
            int temp = position.x - 1;
            for (int x = temp; x > -1; x--)
            {
                if (_pieces[x, position.y] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                }
                else if (_pieces[x, position.y].team != team && x != 0 && _pieces[x - 1, position.y] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        if (position.x != _pieces.GetLength(0) - 1)
        {
            int temp = position.x + 1;
            for (int x = temp; x < _pieces.GetLength(0); x++)
            {
                if (_pieces[x, position.y] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                }
                else if (_pieces[x, position.y].team != team && x != _pieces.GetLength(0) - 1 && _pieces[x + 1, position.y] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        if (position.y != 0)
        {
            int temp = position.y - 1;
            for (int z = temp; z > -1; z--)
            {
                if (_pieces[position.x, z] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                }
                else if (_pieces[position.x, z].team != team && z != 0 && _pieces[position.x, z - 1] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        if (position.y != _pieces.GetLength(0) - 1)
        {
            int temp = position.y + 1;
            for (int z = temp; z < _pieces.GetLength(0); z++)
            {
                if (_pieces[position.x, z] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                }
                else if (_pieces[position.x, z].team != team && z < _pieces.GetLength(0) - 1 && _pieces[position.x, z + 1] is EmptyPiece)
                {
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                    break;
                }
                else
                {
                    break;
                }
            }
        }

        return moveableTiles;
    }
}
