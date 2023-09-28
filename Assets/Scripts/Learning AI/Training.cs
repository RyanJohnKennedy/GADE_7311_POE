using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;

public class Training
{
    public string gamePlayed = "";
    //public List<string> games = new List<string>();
    string bluePath = "Assets/Scripts/Learning AI/BlueGames.txt";
    string redPath = "Assets/Scripts/Learning AI/RedGames.txt";

    public void WritingToFile(Team team)
    {
        string path = Application.dataPath + "/Training/" + "BlueGames.txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(gamePlayed);
        writer.Close();


        //gamePlayed = "";
    }

    public string ReadFromFile()
    {
        string fileString = "";
        string path = Application.dataPath + "/Training/" + "BlueGames.txt";

        StreamReader reader = new StreamReader(path);
        fileString = reader.ReadToEnd();

        reader.Close();

        return fileString;
    }

    public void AddToString(string _board, string _utilityFunction)
    {
        if(gamePlayed != "")
            gamePlayed += "\n&\n";

        gamePlayed += _board + "\n:" + _utilityFunction;
    }

    public void ClearText(Team team)
    {
        string bluePath = "Assets/Scripts/Learning AI/BlueGames.txt";
        string redPath = "Assets/Scripts/Learning AI/RedGames.txt";
        string path;

        if (team == Team.Blue)
            path = bluePath;
        else
            path = redPath;

        File.WriteAllText(path, "");
    }

    public void AddEndGameString(bool won, int movesMade)
    {
        gamePlayed += "\n/\n";
        
        if (won)
        {
            gamePlayed += "W:" + movesMade;
        }
        else
        {
            gamePlayed += "L:" + movesMade;
        }

        gamePlayed += "\n|";

        //games.Add(gamePlayed);
    }
}
