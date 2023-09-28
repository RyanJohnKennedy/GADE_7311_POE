using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StrikerPiece : Piece
{
    public StrikerPiece(Team _team, Vector2Int _position)
    {
        team = _team;
        position = _position;
    }

    public override List<Vector2Int> Move(Piece[,] _pieces)
    {

        for (int x = 1; x < _pieces.GetLength(0); x++)
        {
            if (position.x - x < 0 || position.y - x < 0)
                break;

            if (_pieces[position.x - x, position.y - x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x - x, position.y - x);
                moveableTiles.Add(tile);
            }
            else if (_pieces[x, position.y].team != team && position.x - x != 0 && position.y - x != 0 && _pieces[position.x - x, position.y - x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x - x, position.y - x);
                moveableTiles.Add(tile);
                break;
            }
            else
            {
                break;
            }
        }

        for (int x = 1; x < _pieces.GetLength(0); x++)
        {
            if (position.x + x > _pieces.GetLength(0) - 1 || position.y + x > _pieces.GetLength(0) - 1)
                break;

            if (_pieces[position.x + x, position.y + x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x + x, position.y + x);
                moveableTiles.Add(tile);
            }
            else if (_pieces[x, position.y].team != team && position.x + x != _pieces.GetLength(0) - 1 && position.y + x != _pieces.GetLength(0) - 1 && _pieces[position.x + x, position.y + x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x + x, position.y + x);
                moveableTiles.Add(tile);
                break;
            }
            else
            {
                break;
            }
        }

        for (int x = 1; x < _pieces.GetLength(0); x++)
        {
            if (position.x + x > _pieces.GetLength(0) - 1 || position.y - x < 0)
                break;

            if (_pieces[position.x + x, position.y - x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x + x, position.y - x);
                moveableTiles.Add(tile);
            }
            else if (_pieces[x, position.y].team != team && position.x + x != _pieces.GetLength(0) - 1 && position.y - x != 0 && _pieces[position.x + x, position.y - x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x + x, position.y - x);
                moveableTiles.Add(tile);
                break;
            }
            else
            {
                break;
            }
        }

        for (int x = 1; x < _pieces.GetLength(0); x++)
        {
            if (position.x - x < 0 || position.y + x > _pieces.GetLength(0) - 1)
                break;

            if (_pieces[position.x - x, position.y + x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x - x, position.y + x);
                moveableTiles.Add(tile);
            }
            else if (_pieces[x, position.y].team != team && position.x - x != 0 && position.y + x != _pieces.GetLength(0) - 1 && _pieces[position.x - x, position.y + x] is EmptyPiece)
            {
                Vector2Int tile = new Vector2Int(position.x - x, position.y + x);
                moveableTiles.Add(tile);
                break;
            }
            else
            {
                break;
            }
        }

        return moveableTiles;
    }
}
