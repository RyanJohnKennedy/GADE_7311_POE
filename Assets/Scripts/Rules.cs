using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rules
{
    public static BoardState DoMoves(BoardState boardState, Vector2Int move, Piece movePiece, Team team)
    {
        if (team == Team.Blue) { team = Team.Red; }
        else { team = Team.Blue; }

        boardState.Pieces[movePiece.position.x, movePiece.position.y] = new EmptyPiece();

        if (boardState.Pieces[move.x, move.y] is EmptyPiece)
            boardState.Pieces[move.x, move.y] = movePiece;
        else if (boardState.Pieces[move.x, move.y] is BallPiece)
        {
            int tempX, tempZ;
            //Move ball left or right
            if (movePiece.position.x > move.x)
            {
                if (move.x > 1)
                {
                    tempX = move.x - 2;
                }
                else
                {
                    tempX = move.x - 1;
                }
            }
            else if (movePiece.position.x < move.x)
            {
                if (move.x < boardState.Pieces.GetLength(0) - 2)
                {
                    tempX = move.x + 2;
                }
                else
                {
                    tempX = move.x + 1;
                }
            }
            else
            {
                tempX = move.x;
            }

            //move ball up or down
            if (movePiece.position.y > move.y)
            {
                if (move.y > 1)
                {
                    tempZ = move.y - 2;
                }
                else
                {
                    tempZ = move.y - 1;
                }
            }
            else if (movePiece.position.y < move.y)
            {
                if (move.y < boardState.Pieces.GetLength(0) - 2)
                {
                    tempZ = move.y + 2;
                }
                else
                {
                    tempZ = move.y + 1;
                }
            }
            else
            {
                tempZ = move.y;
            }

            boardState.Pieces[move.x, move.y] = movePiece;

            if (!(boardState.Pieces[tempX, tempZ] is EmptyPiece))
            {
                if (tempX > move.x)
                {
                    tempX--;
                }
                else if (tempX < move.x)
                {
                    tempX++;
                }
                if (tempZ > move.y)
                {
                    tempZ--;
                }
                else if (tempZ < move.y)
                {
                    tempZ++;
                }
            }

            boardState.Pieces[tempX, tempZ] = new BallPiece(new Vector2Int(tempX, tempZ));

            if ((tempX == 0 && tempZ == 0) || (tempX == boardState.boardSize - 1 && tempZ == boardState.boardSize - 1) || (tempX == boardState.boardSize - 1 && tempZ == 0) || (tempX == 0 && tempZ == boardState.boardSize - 1))
            {
                boardState.ResetBall(new Vector2Int(tempX, tempZ));
            }
            boardState.CheckWinPoint();
        }
        else
        {
            int tempX, tempZ;
            //Move piece left or right
            if (movePiece.position.x > move.x)
            {
                tempX = move.x - 1;
            }
            else if (movePiece.position.x < move.x)
            {
                tempX = move.x + 1;
            }
            else
            {
                tempX = move.x;
            }

            //move piece up or down
            if (movePiece.position.y > move.y)
            {
                tempZ = move.y - 1;
            }
            else if (movePiece.position.y < move.y)
            {
                tempZ = move.y + 1;
            }
            else
            {
                tempZ = move.y;
            }

            if (boardState.Pieces[move.x, move.y] is MidFieldPiece)
            {
                boardState.Pieces[move.x, move.y] = movePiece;
                boardState.Pieces[tempX, tempZ] = new MidFieldPiece(team, new Vector2Int(tempX, tempZ));
            }
            else
            {
                boardState.Pieces[move.x, move.y] = movePiece;
                boardState.Pieces[tempX, tempZ] = new StrikerPiece(team, new Vector2Int(tempX, tempZ));
            }
        }
        
        boardState.Pieces[move.x, move.y].position = move;
        boardState.Pieces[move.x, move.y].moveableTiles.Clear();
        boardState.ReInitialiseBoard();

        foreach (Piece p in boardState.Pieces)
        {
            if(p is MidFieldPiece || p is MidFieldPiece)
            {
                if (boardState.Board[p.position.x, p.position.y] == TileType.Goal)
                {
                    int tempX = p.position.x;
                    int tempZ = p.position.y;

                    if (p.team == Team.Blue)
                    {
                        if (boardState.Pieces[boardState.boardSize / 2, 0] is EmptyPiece)
                        {
                            boardState.Pieces[boardState.boardSize / 2, 0] = p;
                            boardState.Pieces[boardState.boardSize / 2, 0].position = new Vector2Int(boardState.boardSize / 2, 0);
                        }
                        else if (boardState.Pieces[boardState.boardSize / 2 - 1, 0] is EmptyPiece)
                        {
                            boardState.Pieces[boardState.boardSize / 2 - 1, 0] = p;
                            boardState.Pieces[boardState.boardSize / 2 - 1, 0].position = new Vector2Int(boardState.boardSize / 2 - 1, 0);

                        }
                        else
                        {
                            boardState.Pieces[boardState.boardSize / 2 + 1, 0] = p;
                            boardState.Pieces[boardState.boardSize / 2 + 1, 0].position = new Vector2Int(boardState.boardSize / 2 + 1, 0);
                        }

                        boardState.Pieces[tempX, tempZ] = new EmptyPiece();
                    }
                    else
                    {
                        if (boardState.Pieces[boardState.boardSize / 2, boardState.boardSize - 1] is EmptyPiece)
                        {
                            boardState.Pieces[boardState.boardSize / 2, boardState.boardSize - 1] = p;
                            boardState.Pieces[boardState.boardSize / 2, boardState.boardSize - 1].position = new Vector2Int(boardState.boardSize / 2, boardState.boardSize - 1);
                        }
                        else if (boardState.Pieces[boardState.boardSize / 2 - 1, boardState.boardSize - 1] is EmptyPiece)
                        {
                            boardState.Pieces[boardState.boardSize / 2 - 1, boardState.boardSize - 1] = p;
                            boardState.Pieces[boardState.boardSize / 2 - 1, boardState.boardSize - 1].position = new Vector2Int(boardState.boardSize / 2 - 1, boardState.boardSize - 1);
                        }
                        else
                        {
                            boardState.Pieces[boardState.boardSize / 2 + 1, boardState.boardSize - 1] = p;
                            boardState.Pieces[boardState.boardSize / 2 + 1, boardState.boardSize - 1].position = new Vector2Int(boardState.boardSize / 2 + 1, boardState.boardSize - 1);
                        }

                        boardState.Pieces[tempX, tempZ] = new EmptyPiece();
                    }
                }
            }
        }

        boardState.ReInitialiseBoard();

        return boardState;
    }

    public static float UtilityFunction(BoardState board, Team team)
    {
        float answer;

        int numTeamPiecesToHitBall = 0;
        int numEnemyPiecesToHitBall = 0;
        int numTeamGoals;
        int numEnemyGoals;
        float ballDistanceToGoal = 0;

        List<Piece> teamPieces = new List<Piece>();
        List<Piece> enemyPieces = new List<Piece>();

        foreach (Piece p in board.Pieces)
        {
            if (p.team == team)
            {
                teamPieces.Add(p);
            }
            else if (p.team != Team.Neutral)
            {
                enemyPieces.Add(p);
            }
        }

        //Allocate the goals to the right team
        if (team == Team.Blue)
        {
            numTeamGoals = board.blueScore;
            numEnemyGoals = board.redScore;
        }
        else
        {
            numTeamGoals = board.redScore;
            numEnemyGoals = board.blueScore;
        }

        //Find all pieces able to hit the ball
        foreach (Piece p in teamPieces)
        {
            List<Vector2Int> moveTiles = p.Move(board.Pieces);
            foreach (Vector2Int tile in moveTiles)
            {
                if (board.Pieces[tile.x, tile.y] is BallPiece)
                {
                    numTeamPiecesToHitBall++;
                }
            }
        }

        foreach (Piece p in enemyPieces)
        {
            List<Vector2Int> moveTiles = p.Move(board.Pieces);
            foreach (Vector2Int tile in moveTiles)
            {
                if (board.Pieces[tile.x, tile.y] is BallPiece)
                {
                    numEnemyPiecesToHitBall++;
                }
            }
        }


        //Calculate the distance to closest goal
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
                float tempDis;

                ballDistanceToGoal = Vector2.Distance(p.position, goalPos[0]);

                foreach (Vector2Int goal in goalPos)
                {
                    tempDis = Vector2.Distance(p.position, goal);

                    if (tempDis < ballDistanceToGoal) { ballDistanceToGoal = tempDis; }
                }
            }
        }

        //UtilityFunction
        answer = (numTeamPiecesToHitBall - numEnemyPiecesToHitBall) / (ballDistanceToGoal + 1);
        return answer;
    }

    public static BoardState AddWaterTiles(BoardState boardState, int startNum, int totalNum)
    {
        List<Vector2Int> waterPos = new List<Vector2Int>();
        int posX, posZ;

        for (int i = 0; i < startNum; i++)
        {
            posX = Random.Range(1, boardState.boardSize - 1);
            posZ = Random.Range(1, boardState.boardSize - 1);

            while (!(boardState.Pieces[posX, posZ] is EmptyPiece) 
                && !(boardState.Pieces[posX - 1, posZ] is EmptyPiece)
                && !(boardState.Pieces[posX, posZ - 1] is EmptyPiece)
                && !(boardState.Pieces[posX + 1, posZ] is EmptyPiece)
                && !(boardState.Pieces[posX, posZ + 1] is EmptyPiece))
            {
                posX = Random.Range(0, boardState.boardSize - 1);
                posZ = Random.Range(0, boardState.boardSize - 1);
            }

            boardState.Pieces[posX, posZ] = new WaterPiece();
            boardState.Board[posX, posZ] = TileType.Water;
            waterPos.Add(new Vector2Int(posX, posZ));
        }

        int counter = 0;
        int failsafe = 0;
        while (counter < totalNum || failsafe == 100)
        {
            int randomWater = Random.Range(0, waterPos.Count);
            int randomDir = Random.Range(0, 4);
            Vector2Int water = waterPos[randomWater];

            var info = AddWater(boardState, water, randomDir);

            if (info.answer)
            {
                boardState = info.board;
                waterPos.Add(info.pos);
                counter++;
            }

            //failsafe++;
        }

        return boardState;
    }

    static (bool answer, BoardState board, Vector2Int pos) AddWater(BoardState boardState, Vector2Int waterPos, int Dir)
    {
        bool answer = false;
        Vector2Int pos = new Vector2Int(0,0);

        if(Dir == 0 && waterPos.x - 1 > 0 && boardState.Pieces[waterPos.x - 1, waterPos.y] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[waterPos.x - 1, waterPos.y] = new WaterPiece();
            boardState.Board[waterPos.x - 1, waterPos.y] = TileType.Water;
            pos = new Vector2Int(waterPos.x - 1, waterPos.y);
        }
        else if (Dir == 1 && waterPos.x + 1 < boardState.boardSize - 1 && boardState.Pieces[waterPos.x + 1, waterPos.y] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[waterPos.x + 1, waterPos.y] = new WaterPiece();
            boardState.Board[waterPos.x + 1, waterPos.y] = TileType.Water;
            pos = new Vector2Int(waterPos.x + 1, waterPos.y);
        }
        else if (Dir == 2 && waterPos.y - 1 > 0 && boardState.Pieces[waterPos.x, waterPos.y - 1] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[waterPos.x, waterPos.y - 1] = new WaterPiece();
            boardState.Board[waterPos.x, waterPos.y - 1] = TileType.Water;
            pos = new Vector2Int(waterPos.x, waterPos.y - 1);
        }
        else if (Dir == 3 && waterPos.y + 1 < boardState.boardSize - 1 && boardState.Pieces[waterPos.x, waterPos.y + 1] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[waterPos.x, waterPos.y + 1] = new WaterPiece();
            boardState.Board[waterPos.x, waterPos.y + 1] = TileType.Water;
            pos = new Vector2Int(waterPos.x, waterPos.y + 1);
        }

        return (answer, boardState, pos);
    }

    public static BoardState AddRockTiles(BoardState boardState, int startNum, int totalNum)
    {
        List<Vector2Int> rockPos = new List<Vector2Int>();
        int posX, posZ;

        for (int i = 0; i < startNum; i++)
        {
            posX = Random.Range(1, boardState.boardSize - 1);
            posZ = Random.Range(1, boardState.boardSize - 1);

            while (!(boardState.Pieces[posX, posZ] is EmptyPiece)
                && !(boardState.Pieces[posX - 1, posZ] is EmptyPiece)
                && !(boardState.Pieces[posX, posZ - 1] is EmptyPiece)
                && !(boardState.Pieces[posX + 1, posZ] is EmptyPiece)
                && !(boardState.Pieces[posX, posZ + 1] is EmptyPiece))
            {
                posX = Random.Range(0, boardState.boardSize - 1);
                posZ = Random.Range(0, boardState.boardSize - 1);
            }

            boardState.Pieces[posX, posZ] = new RockPiece();
            boardState.Board[posX, posZ] = TileType.Rocks;
            rockPos.Add(new Vector2Int(posX, posZ));
        }

        int counter = 0;
        int failsafe = 0;
        while (counter < totalNum || failsafe == 100)
        {
            int randomRock = Random.Range(0, rockPos.Count);
            int randomDir = Random.Range(0, 4);
            Vector2Int rock = rockPos[randomRock];

            var info = AddRocks(boardState, rock, randomDir);

            if (info.answer)
            {
                boardState = info.board;
                rockPos.Add(info.pos);
                counter++;
            }

            //failsafe++;
        }

        return boardState;
    }

    static (bool answer, BoardState board, Vector2Int pos) AddRocks(BoardState boardState, Vector2Int rockPos, int Dir)
    {
        bool answer = false;
        Vector2Int pos = new Vector2Int(0, 0);

        if (Dir == 0 && rockPos.x - 1 > 0 && boardState.Pieces[rockPos.x - 1, rockPos.y] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[rockPos.x - 1, rockPos.y] = new RockPiece();
            boardState.Board[rockPos.x - 1, rockPos.y] = TileType.Rocks;
            pos = new Vector2Int(rockPos.x - 1, rockPos.y);
        }
        else if (Dir == 1 && rockPos.x + 1 < boardState.boardSize - 1 && boardState.Pieces[rockPos.x + 1, rockPos.y] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[rockPos.x + 1, rockPos.y] = new RockPiece();
            boardState.Board[rockPos.x + 1, rockPos.y] = TileType.Rocks;
            pos = new Vector2Int(rockPos.x + 1, rockPos.y);
        }
        else if (Dir == 2 && rockPos.y - 1 > 0 && boardState.Pieces[rockPos.x, rockPos.y - 1] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[rockPos.x, rockPos.y - 1] = new RockPiece();
            boardState.Board[rockPos.x, rockPos.y - 1] = TileType.Rocks;
            pos = new Vector2Int(rockPos.x, rockPos.y - 1);
        }
        else if (Dir == 3 && rockPos.y + 1 < boardState.boardSize - 1 && boardState.Pieces[rockPos.x, rockPos.y + 1] is EmptyPiece)
        {
            answer = true;
            boardState.Pieces[rockPos.x, rockPos.y + 1] = new RockPiece();
            boardState.Board[rockPos.x, rockPos.y + 1] = TileType.Rocks;
            pos = new Vector2Int(rockPos.x, rockPos.y + 1);
        }

        return (answer, boardState, pos);
    }
}
