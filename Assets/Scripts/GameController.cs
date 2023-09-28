using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player
{
    Human,
    RandomAI,
    MinimaxAI,
    FSM,
    LearningAI
}

public class GameController : MonoBehaviour
{
    [Header("Player Settings")]
    public Player player1Type;
    public Player player2Type;

    private IPlayable player1;
    private IPlayable player2;

    [Header("Board Settings")]
    public int boardSize;
    public int goalNumber;

    [Header("UI")]
    public Text turnText;
    public Text blueScoreText;
    public Text redScoreText;
    public Text winnerText;

    [Header("Tiles")]
    public GameObject emptyTile;
    public GameObject waterTile;
    public GameObject rockTile;
    public GameObject goalTile;
    public GameObject moveRedTile;
    public GameObject moveBlueTile;

    [Header("Team Materials")]
    public Material redMaterial;
    public Material blueMaterial;

    [Header("Pieces")]
    public GameObject MidFielder;
    //public GameObject Striker;
    public GameObject Ball;

    [Header("Procedural")]
    public bool procedural = false;
    public int startNumWater;
    public int totalNumWater;
    public int startNumRock;
    public int totalNumRock;

    [HideInInspector]
    public PieceController selectedPiece;
    public BoardState board;
    public Team currentTeam = Team.Blue;
    private Vector2Int currentMove;
    public int BlueTeamScore = 0;
    public int RedTeamScore = 0;
    private Vector3Int cameraRePos = new Vector3Int(10, 19, 10);

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            player1Type = GameManager.Instance.player1;
            player2Type = GameManager.Instance.player2;
        }
        catch { Debug.Log("GameManager not loaded"); }

        procedural = GameManager.Instance.Procedual;
        UpdateUI();
        selectedPiece = null;
        currentMove = new Vector2Int();

        if (procedural)
            ProceduralMap();
        else
            board = new BoardState(boardSize, goalNumber, this);

        SetPlayerTypes();
        InstantiateTiles();
        InstantiatePieces();
    }

    // Update is called once per frame
    void Update()
    {
        AITurn();
        ChangeTeam();
    }

    void SetPlayerTypes()
    {
        //Create Players depending on type of player
        switch (player1Type)//Blue Team
        {
            case Player.Human:
                player1 = new Human();
                break;
            case Player.RandomAI:
                player1 = new RandomAI();
                break;
            case Player.MinimaxAI:
                player1 = new MinimaxAI();
                break;
            case Player.FSM:
                player1 = new FSM();
                break;
            case Player.LearningAI:
                player1 = new LearningAI();
                break;
            default:
                break;
        }
        player1.SetTeam(Team.Blue);

        switch (player2Type)//Red Team
        {
            case Player.Human:
                player2 = new Human();
                break;
            case Player.RandomAI:
                player2 = new RandomAI();
                break;
            case Player.MinimaxAI:
                player2 = new MinimaxAI();
                break;
            case Player.FSM:
                player2 = new FSM();
                break;
            case Player.LearningAI:
                player2 = new LearningAI();
                break;
            default:
                break;
        }
        player2.SetTeam(Team.Red);
    }

    void UpdateUI()
    {
        turnText.text = currentTeam + "'s turn";
        blueScoreText.text = BlueTeamScore.ToString();
        redScoreText.text = RedTeamScore.ToString();
    }

    void ChangeTeam()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
        {
            if (currentTeam == Team.Blue)
                currentTeam = Team.Red;
            else
                currentTeam = Team.Blue;

            UpdateUI();
            board.UpdateScores(RedTeamScore, BlueTeamScore);
        }
    }

    public bool CheckWinner()
    {
        if (BlueTeamScore >= 1) 
        {
            if(GameManager.Instance.training)
            {
                if (player1 is LearningAI)
                {
                    LearningAI endgame = (LearningAI)player1;
                    endgame.GameOver(Team.Blue);
                    InstantiatePieces();
                    ResetGame();
                    endgame.moves = 0;
                }
                else if (player2 is LearningAI)
                {
                    LearningAI endgame = (LearningAI)player2;
                    endgame.GameOver(Team.Blue);
                    InstantiatePieces();
                    ResetGame();
                    endgame.moves = 0;
                }
            }
            winnerText.text = "Blue team wins!";
            winnerText.color = Color.blue;
            winnerText.gameObject.SetActive(true);
            return true; 
        }
        else if(RedTeamScore >= 1)
        {
            if (GameManager.Instance.training)
            {
                if (player1 is LearningAI)
                {
                    LearningAI endgame = (LearningAI)player1;
                    endgame.GameOver(Team.Red);
                    InstantiatePieces();
                    ResetGame();
                    endgame.moves = 0;
                }
                else if (player2 is LearningAI)
                {
                    LearningAI endgame = (LearningAI)player2;
                    endgame.GameOver(Team.Red);
                    InstantiatePieces();
                    ResetGame();
                    endgame.moves = 0;
                }
            }
            winnerText.text = "Red team wins!";
            winnerText.color = Color.red;
            winnerText.gameObject.SetActive(true);
            return true;
        }
        else
        { 
            return false; 
        }
    }

    public void InstantiateTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject go in tiles)
            Destroy(go);

        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                if (board.Board[x, z] == TileType.Empty)
                    Instantiate(emptyTile, new Vector3(x, 0f, z), Quaternion.identity);
                else if(board.Board[x, z] == TileType.Goal)
                    Instantiate(goalTile, new Vector3(x, 0f, z), Quaternion.identity);
                else if (board.Board[x, z] == TileType.MoveableBlue)
                    Instantiate(moveBlueTile, new Vector3(x, 0f, z), Quaternion.identity);
                else if (board.Board[x, z] == TileType.MoveableRed)
                    Instantiate(moveRedTile, new Vector3(x, 0f, z), Quaternion.identity);
                else if (board.Board[x, z] == TileType.Water)
                    Instantiate(waterTile, new Vector3(x, 0f, z), Quaternion.identity);
                else if (board.Board[x, z] == TileType.Rocks)
                    Instantiate(rockTile, new Vector3(x, 0f, z), Quaternion.identity);
            }
        }
    }

    public void InstantiatePieces()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject go in pieces) 
            Destroy(go); 

        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                if (board.Pieces[x, z] != null)
                {
                    if (board.Pieces[x, z] is MidFieldPiece)
                    {
                        GameObject go = Instantiate(MidFielder, new Vector3(x, 0, z), Quaternion.identity);
                        go.GetComponent<PieceController>().connectedPiece = board.Pieces[x, z];

                        if (board.Pieces[x, z].team == Team.Red)
                            go.GetComponent<MeshRenderer>().material = redMaterial;
                        else
                            go.GetComponent<MeshRenderer>().material = blueMaterial;
                    }
                    else if (board.Pieces[x, z] is StrikerPiece)
                    {
                        //GameObject go = Instantiate(Striker, new Vector3(x, 0, z), Quaternion.identity);
                        //go.GetComponent<PieceController>().connectedPiece = board.Pieces[x, z];

                        if (board.Pieces[x, z].team == Team.Red) { }
                        //go.GetComponent<MeshRenderer>().material = redMaterial;
                        else { }
                            //go.GetComponent<MeshRenderer>().material = blueMaterial;
                    }
                    else if(board.Pieces[x, z] is BallPiece)
                    {
                        GameObject go = Instantiate(Ball, new Vector3(x, 0, z), Quaternion.identity);
                    }
                }
            }
        }
    }

    public void MovePiece(Vector2Int _moveLocation)
    {
        if ((currentTeam == Team.Blue && player1 is Human) || (currentTeam == Team.Red && player2 is Human))
        {
            currentMove = _moveLocation;
            TakeTurn();
            InstantiatePieces();
            InstantiateTiles();
        }
    }

    public void AITurn()
    {
        if (!CheckWinner())
        {
            if ((currentTeam == Team.Blue && !(player1 is Human)) || (currentTeam == Team.Red && !(player2 is Human)))
            {
                //if (Input.GetKeyDown(KeyCode.Space) || GameManager.Instance.training)
                //{
                    TakeTurn();
                    if (!GameManager.Instance.training)
                    {
                        InstantiatePieces();
                        InstantiateTiles();
                    }
                //}
            }
        }
    }

    

    void TakeTurn()
    {
        if (!CheckWinner())
        {
            if (currentTeam == Team.Blue)
            {
                board = player1.MakeMove(board, currentMove, selectedPiece);
                currentTeam = Team.Red;
            }
            else
            {
                board = player2.MakeMove(board, currentMove, selectedPiece);
                currentTeam = Team.Blue;
            }
            selectedPiece = null;
            UpdateUI();
            board.UpdateScores(RedTeamScore, BlueTeamScore);
        }
    }

    void ProceduralMap()
    {
        Camera.main.transform.position = cameraRePos;
        boardSize = 21;
        board = new BoardState(boardSize, goalNumber, this);
        board = Rules.AddWaterTiles(board, startNumWater, totalNumWater);
        board = Rules.AddRockTiles(board, startNumRock, totalNumRock);
    }

    public void ResetGame()
    {
        selectedPiece = null;
        currentMove = new Vector2Int();
        BlueTeamScore = 0;
        RedTeamScore = 0;
        winnerText.gameObject.SetActive(false);
        currentTeam = Team.Blue;
        board.InitialiseBoard();
        board.PlacePieces();
        UpdateUI();
        InstantiateTiles();
        InstantiatePieces();
        Debug.Log("Reset done");
    }
}
