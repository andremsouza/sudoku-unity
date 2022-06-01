using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    public enum EGameMode
    {
        NOT_SET,
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD
    }

    public static GameSettings Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private EGameMode _gameMode;
    private ushort _gameSize;


    void Start()
    {
        _gameMode = EGameMode.NOT_SET;
        _gameSize = 3;
    }


    public void SetGameMode(EGameMode gameMode)
    {
        _gameMode = gameMode;
    }


    public void SetGameMode(string mode)
    {
        switch (mode)
        {
            case "Easy":
                SetGameMode(EGameMode.EASY);
                break;
            case "Medium":
                SetGameMode(EGameMode.MEDIUM);
                break;
            case "Hard":
                SetGameMode(EGameMode.HARD);
                break;
            case "VeryHard":
                SetGameMode(EGameMode.VERY_HARD);
                break;
            default:
                SetGameMode(EGameMode.NOT_SET);
                throw new System.ArgumentException("Invalid difficulty. Must be one of: Easy, Medium, Hard, VeryHard");
        }
    }


    public string GetGameMode()
    {
        switch (_gameMode)
        {
            case EGameMode.EASY:
                return "Easy";
            case EGameMode.MEDIUM:
                return "Medium";
            case EGameMode.HARD:
                return "Hard";
            case EGameMode.VERY_HARD:
                return "VeryHard";
        }

        Debug.LogError("[ERROR] Game difficulty is not set.");
        return " ";
    }


    public void SetGameSize(ushort size)
    {
        if (size < 1 || size > 9)
        {
            Debug.LogError("[ERROR] Game size is invalid. Must be between 1 and 9.");
            throw new System.ArgumentOutOfRangeException("size", "Size must be between 1 and 9");
        }
        _gameSize = size;
    }


    public ushort GetGameSize()
    {
        return _gameSize;
    }
}
