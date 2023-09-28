using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text Player1txt;
    public Text Player2txt;
    public Text Procedualtxt;

    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "PickPlayerScene")
        {
            if(GameManager.Instance.player1 == Player.Human)
                Player1txt.text = "Player";
            else if(GameManager.Instance.player1 == Player.RandomAI)
                Player1txt.text = "Easy AI";
            else if (GameManager.Instance.player1 == Player.FSM)
                Player1txt.text = "FSM AI";
            else if (GameManager.Instance.player1 == Player.LearningAI)
                Player1txt.text = "Learning AI";

            if (GameManager.Instance.player2 == Player.Human)
                Player2txt.text = "Player";
            else if (GameManager.Instance.player2 == Player.RandomAI)
                Player2txt.text = "Easy AI";
            else if (GameManager.Instance.player2 == Player.FSM)
                Player2txt.text = "Medium AI";

            Procedualtxt.text = "Procedual map: " + GameManager.Instance.Procedual;
        }
    }

    public void ProcedualToggle()
    {
        GameManager.Instance.Procedual = !GameManager.Instance.Procedual;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void VSPlayer()
    {
        GameManager.Instance.player1 = Player.Human;
        GameManager.Instance.player2 = Player.Human;
    }

    public void VSFSM()
    {
        GameManager.Instance.player1 = Player.FSM;
        GameManager.Instance.player2 = Player.Human;
    }

    public void VSLearning()
    {
        GameManager.Instance.player1 = Player.LearningAI;
        GameManager.Instance.player2 = Player.Human;
    }

    public void Player1Pick(int playerType)
    {
        switch (playerType)
        {
            case 0:
                GameManager.Instance.player1 = Player.Human;
                break;
            case 1:
                GameManager.Instance.player1 = Player.RandomAI;
                break;
            case 2:
                GameManager.Instance.player1 = Player.FSM;
                break;
            case 3:
                GameManager.Instance.player1 = Player.FSM;
                break;
            default:
                break;
        }

    }

    public void Player2Pick(int playerType)
    {
        switch (playerType)
        {
            case 0:
                GameManager.Instance.player2 = Player.Human;
                break;
            case 1:
                GameManager.Instance.player2 = Player.RandomAI;
                break;
            case 2:
                GameManager.Instance.player2 = Player.FSM;
                break;
            case 3:
                GameManager.Instance.player2 = Player.FSM;
                break;
            default:
                break;
        }
    }
}
