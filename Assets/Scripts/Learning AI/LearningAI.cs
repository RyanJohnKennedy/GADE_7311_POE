using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningAI : IPlayable
{
    private Team team;
    
    IPlayable trainingAI;
    Training trainer = new Training();
    BoardState trainingBoard;
    public int moves = 0;

    public BoardState MakeMove(BoardState boardState, Vector2Int move, PieceController movePiece)
    {
        trainingAI = new RandomAI();
        trainingAI.SetTeam(team);
        trainingBoard = boardState;
        if (GameManager.Instance.training)
        {
            if(moves == 0)
            {
                SaveState(new BoardState(boardState.boardSize, boardState.goalNumber, boardState.gc));
            }

            if (moves < 50)
            {
                SaveState(trainingBoard);

                trainingBoard = trainingAI.MakeMove(boardState, move, movePiece);
                moves++;

                SaveState(trainingBoard);
            }
            else
            {
                trainer.gamePlayed = "";
                moves = 0;
                trainingBoard.gc.ResetGame();
                Debug.Log("startReset");
            }

            return trainingBoard;
        }
        else
        {
            return SearchForState(boardState);
        }
    }

    BoardState SearchForState(BoardState board)
    {
        //Converting current board to String
        string[,] currentboardStringArray = board.ConvertToString(board.Pieces);
        string currentboardString = ConvertBoardArrayToString(currentboardStringArray);

        //Getting string from file and splitting to games and moves
        string fileInfo = trainer.ReadFromFile();

        string[] gameStrings = SplitToGames(fileInfo);
        string[][] games = new string[gameStrings.Length - 1][];
        int[] moveNums = new int[games.Length - 1];

        for (int i = 0; i < games.Length - 1; i++)
        {
            var winAndMovesSplit = SplitWinAndMoves(gameStrings[i]);
            games[i] = winAndMovesSplit.moves;
            moveNums[i] = winAndMovesSplit.moveNum;
        }
        
        int tempNum;
        string[] tempStringArr;
        for (int i = 0; i < moveNums.Length; i++)
        {
            for (int j = 0; j < moveNums.Length; j++)
            {
                if(moveNums[i] < moveNums[j])
                {
                    tempNum = moveNums[i];
                    moveNums[i] = moveNums[j];
                    moveNums[j] = tempNum;

                    tempStringArr = games[i];
                    games[i] = games[j];
                    games[j] = tempStringArr;
                }
            }
        }

        //Checking current board for compatible moves
        for (int i = 0; i < games.Length - 1; i++)
        {
            for (int j = 0; j < games[i].Length; j+=2)
            {
                string temp1 = games[i][j].Trim();
                string temp2 = currentboardString.Trim();

                if (String.Equals(temp1, temp2))
                {
                    string trimmedString = games[i][j + 1];
                    string[,] boardToBe = ConvertStringToBoardArray(trimmedString);

                    return board.FillFromString(boardToBe, board);
                }
            }
        }

        FSM statemachine = new FSM();
        statemachine.SetTeam(team);
        return statemachine.MakeMove(board, new Vector2Int(0,0), null);
    }

    (string[] moves, int moveNum) SplitWinAndMoves(string gameStrings)
    {
        string[] types = gameStrings.Split('/');
        string[] movesAndUtility = types[0].Split('&');
        string[] moves = new string[movesAndUtility.Length];

        for (int i = 0; i < movesAndUtility.Length; i++)
        {
            string[] temp = movesAndUtility[i].Split(':');

            moves[i] = temp[0];
        }

        string[] moveNumAndWin = types[1].Split(':');
        int moveNum = Convert.ToInt32(moveNumAndWin[1]);

        return (moves, moveNum);
    }

    string[] SplitToGames(string fileInfo)
    {
        string[] games = fileInfo.Split('|');

        return games;
    }

    void SaveState(BoardState board)
    {
        string[,] stringArray = new string[board.boardSize, board.boardSize];
        string currentBoard = "";

        stringArray = board.ConvertToString(board.Pieces);

        for (int x = 0; x < board.boardSize; x++)
        {
            for (int z = 0; z < board.boardSize; z++)
            {
                currentBoard += stringArray[x, z] + ".";
            }
        }

        string utilityFunction = Rules.UtilityFunction(board, team).ToString();
        trainer.AddToString(currentBoard, utilityFunction);
    }

    public void GameOver(Team winningTeam)
    {
        bool won;
        if (winningTeam == team)
        {
            won = true;

            trainer.AddEndGameString(won, moves);
            trainer.WritingToFile(team);
            trainer.gamePlayed = "";
        }
    }

    string ConvertBoardArrayToString(string[,] stringArray)
    {
        string currentBoard = "";
        currentBoard += "\n";

        for (int x = 0; x < trainingBoard.boardSize; x++)
        {
            for (int z = 0; z < trainingBoard.boardSize; z++)
            {
                currentBoard += stringArray[x, z] + ".";
            }
        }
        currentBoard += "\n";


        return currentBoard;
    }

    string[,] ConvertStringToBoardArray(string stringBoard)
    {
        string[,] stringArray = new string[trainingBoard.boardSize, trainingBoard.boardSize];
        string[] tempArray = stringBoard.Split('.');

        int counter = 0;
        for (int i = 0; i < trainingBoard.boardSize; i++)
        {
            for (int j = 0; j < trainingBoard.boardSize; j++)
            {
                stringArray[i, j] = tempArray[counter];
                counter++;
            }
        }

        return stringArray;
    }

    public void SetTeam(Team t)
    {
        team = t;
    }
}
