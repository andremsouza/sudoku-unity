using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuDataGenerator : MonoBehaviour
{
    public static List<SudokuData.SudokuBoardData> GenerateSudokuData(int size, string difficulty)
    {
        List<SudokuData.SudokuBoardData> sudokuData = new List<SudokuData.SudokuBoardData>();
        SudokuData.SudokuBoardData data = new SudokuData.SudokuBoardData();
        (data.board, data.solution) = GenerateSudokuBoard(size);
        sudokuData.Add(data);
        return sudokuData;
    }

    public static (List<int>, List<int>) GenerateSudokuBoard(int size)
    {
        List<int> board = new List<int>(), solution = new List<int>();
        for (int i = 0; i < size * size; i++)
        {
            for (int j = 0; j < size * size; j++)
            {
                board.Add(i + j);
                solution.Add(j);
                // board[i * size + j] = 0;
            }
        }
        return (board, solution);
    }
}

public class SudokuEasyData : MonoBehaviour
{
    public static List<SudokuData.SudokuBoardData> GetData()
    {
        List<SudokuData.SudokuBoardData> data = new List<SudokuData.SudokuBoardData>();

        data.Add(new SudokuData.SudokuBoardData(
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            },
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            }
        ));

        return data;
    }

    public static List<SudokuData.SudokuBoardData> GetData(int size, string difficulty)
    {
        List<SudokuData.SudokuBoardData> data = SudokuDataGenerator.GenerateSudokuData(size, difficulty);
        return data;
    }
}

public class SudokuMediumData : MonoBehaviour
{
    public static List<SudokuData.SudokuBoardData> GetData()
    {
        List<SudokuData.SudokuBoardData> data = new List<SudokuData.SudokuBoardData>();

        data.Add(new SudokuData.SudokuBoardData(
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            },
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            }
        ));

        return data;
    }
}

public class SudokuHardData : MonoBehaviour
{
    public static List<SudokuData.SudokuBoardData> GetData()
    {
        List<SudokuData.SudokuBoardData> data = new List<SudokuData.SudokuBoardData>();

        data.Add(new SudokuData.SudokuBoardData(
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            },
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            }
        ));

        return data;
    }
}

public class SudokuVeryHardData : MonoBehaviour
{
    public static List<SudokuData.SudokuBoardData> GetData()
    {
        List<SudokuData.SudokuBoardData> data = new List<SudokuData.SudokuBoardData>();

        data.Add(new SudokuData.SudokuBoardData(
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            },
            new List<int>(81) {
                0, 1, 4, 0, 0, 0, 0, 3, 0,
                3, 0, 0, 5, 1, 0, 8, 0, 0,
                0, 8, 0, 0, 0, 9, 0, 0, 6,
                0, 0, 1, 8, 0, 0, 6, 0, 0,
                0, 0, 3, 2, 5, 6, 4, 0, 0,
                0, 0, 6, 0, 0, 7, 2, 0, 0,
                9, 0, 0, 7, 0, 0, 0, 4, 0,
                0, 0, 5, 0, 8, 4, 0, 0, 2,
                0, 4, 0, 0, 0, 0, 7, 1, 0
            }
        ));

        return data;
    }
}


public class SudokuData : MonoBehaviour
{
    public static SudokuData Instance;

    public struct SudokuBoardData
    {
        public List<int> board;
        public List<int> solution;

        public SudokuBoardData(List<int> board, List<int> solution) : this()
        {
            this.board = board;
            this.solution = solution;
        }
    };

    public Dictionary<string, List<SudokuBoardData>> SudokuGame = new Dictionary<string, List<SudokuBoardData>>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        SudokuGame.Add("Easy", SudokuEasyData.GetData(3, "Easy"));
        SudokuGame.Add("Medium", SudokuMediumData.GetData());
        SudokuGame.Add("Hard", SudokuHardData.GetData());
        SudokuGame.Add("VeryHard", SudokuVeryHardData.GetData());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
