using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuDataGenerator : MonoBehaviour
{
    private const float _easyPercentage = 0.84f, _mediumPercentage = 0.63f, _hardPercentage = 0.42f, _veryHardPercentage = 0.21f;


    private static bool CheckValid(in List<int> board, in int size)
    {
        return false;
    }

    private static bool CheckPossible(in List<int> board, in int size, in int idx, in int number)
    {
        int len = size * size, row = idx / size, col = idx % size;
        // Check row
        for (int i = 0; i < len; i++)
        {
            if (board[i] == number)
                return false;
        }
        return true;
    }

    public static List<SudokuData.SudokuBoardData> GenerateSudokuData(in int size, in string difficulty)
    {
        List<SudokuData.SudokuBoardData> sudokuData = new List<SudokuData.SudokuBoardData>();
        SudokuData.SudokuBoardData data = new SudokuData.SudokuBoardData();
        (data.board, data.solution) = GenerateSudokuBoard(size, difficulty);
        sudokuData.Add(data);
        return sudokuData;
    }

    public static (List<int>, List<int>) GenerateSudokuBoard(in int size, in string difficulty)
    {
        int len = size * size, idx = 0, val = Random.Range(1, len);
        Stack<(int, int)> stack = new Stack<(int, int)>();
        List<int> board = new List<int>(), solution = new List<int>();
        List<HashSet<int>> visited = new List<HashSet<int>>();

        // Initialize collections
        for (var i = 0; i < len * len; i++)
        {
            board.Add(0);
            solution.Add(0);
            visited.Add(new HashSet<int>());
        }

        // Push initial state to stack and start backtracking
        stack.Push((idx, val));
        while (stack.Count > 0)
        {
            // Get top of stack
            (idx, val) = stack.Pop();

            // If already visited, move on
            if (visited[idx].Contains(val))
                continue;

            // Move to next state and increase index
            visited[idx].Add(val);
            board[idx] = val;
            solution[idx] = val;
            idx++;

            // Check for solution (exit condition)
            if (idx == len * len)
                break;

            // Push next possible states into stack
            var nextStates = new HashSet<int>(visited[idx]);
            while (nextStates.Count < len)
            {
                val = Random.Range(1, len);
                if (!nextStates.Contains(val) && CheckPossible(board, size, idx, val))
                {
                    nextStates.Add(val);
                    stack.Push((idx, val));
                }
            }
        }

        // board[i * len + j] = i + j;
        // board[i * size + j] = 0;
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
