using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BallPiece : Piece
{
    public BallPiece(Vector2Int _position)
    {
        team = Team.Neutral;
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
                //if (_pieces[x, position.y] is EmptyPiece)
                //{
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                //}
                //else
                //{
                //    break;
                //}
            }
        }

        if (position.x != _pieces.GetLength(0) - 1)
        {
            int temp = position.x + 1;
            for (int x = temp; x < _pieces.GetLength(0); x++)
            {
                //if (_pieces[x, position.y] is EmptyPiece)
                //{
                    Vector2Int tile = new Vector2Int(x, position.y);
                    moveableTiles.Add(tile);
                //}
                //else
                //{
                //    break;
                //}
            }
        }

        if (position.y != 0)
        {
            int temp = position.y - 1;
            for (int z = temp; z > -1; z--)
            {
                //if (_pieces[position.x, z] is EmptyPiece)
                //{
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                //}
                //else
                //{
                //    break;
                //}
            }
        }

        if (position.y != _pieces.GetLength(0) - 1)
        {
            int temp = position.y + 1;
            for (int z = temp; z < _pieces.GetLength(0); z++)
            {
                //if (_pieces[position.x, z] is EmptyPiece)
                //{
                    Vector2Int tile = new Vector2Int(position.x, z);
                    moveableTiles.Add(tile);
                //}
                //else
                //{
                //    break;
                //}
            }
        }

        return moveableTiles;
    }
}
