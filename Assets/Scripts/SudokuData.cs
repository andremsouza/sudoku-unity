using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.DateTime;
using static System.Int32;

public class SudokuDataGenerator : MonoBehaviour
{
    private const float _easyPercentage = 0.16f, _mediumPercentage = 0.37f, _hardPercentage = 0.58f, _veryHardPercentage = 0.79f;


    private static bool CheckValidBoard(in List<int> board, in int size, in bool checkFull)
    {
        var len = size * size;

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                if ((checkFull && board[i * len + j] == 0) || !CheckPossible(board, size, i * len + j, board[i * len + j]))
                    return false;
            }
        }
        return true;
    }


    private static bool CheckPossible(in List<int> board, in int size, in int idx, in int number)
    {
        var len = size * size;
        int row = idx / len, col = idx % len;
        // Check row
        for (int j = 0; j < len; j++)
            if (row * len + j != idx && board[row * len + j] == number)
                return false;
        // Check column
        for (int i = 0; i < len; i++)
            if (i * len + col != idx && board[i * len + col] == number)
                return false;
        // Check square
        for (int i = size * (int)(row / size); i < size * ((int)(row / size) + 1); i++)
        {
            for (int j = size * (int)(col / size); j < size * ((int)(col / size) + 1); j++)
            {
                if (i * len + j != idx && board[i * len + j] == number)
                    return false;
            }
        }
        return true;
    }

    private static float GetDifficultyPercentage(in string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                return _easyPercentage;
            case "Medium":
                return _mediumPercentage;
            case "Hard":
                return _hardPercentage;
            case "Very Hard":
                return _veryHardPercentage;
            default:
                return 0.0f;
        }
    }


    private static List<int> GenerateRandomStates(in int size, int randomState = -1)
    {
        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);
        var len = size * size;
        // Push next possible states into stack
        var nextStates = new List<int>();
        // Add numbers to list of next states
        for (int i = 1; i <= len; i++)
            nextStates.Add(i);
        // Shuffle list of next states
        for (int i = 0; i < nextStates.Count; i++)
        {
            var temp = nextStates[i];
            var randomIndex = Random.Range(0, nextStates.Count - 1);
            nextStates[i] = nextStates[randomIndex];
            nextStates[randomIndex] = temp;
        }
        return nextStates;
    }


    private static List<int> RemoveRandomValues(in List<int> board, in int size, in float removePercentage, int randomState = -1)
    {

    }


    private static List<int> GenerateSudokuBoard(in int size, int randomState = -1)
    {
        int len = size * size, lastIdx = 0, maxIdx = 0;
        Stack<(int, int)> stack = new Stack<(int, int)>();
        List<int> board = new List<int>();
        List<HashSet<int>> visited = new List<HashSet<int>>();

        // Initialize collections
        for (var i = 0; i < len * len; i++)
        {
            board.Add(0);
            visited.Add(new HashSet<int>());
        }
        // Set Random Initial States
        if (randomState == -1)
        {
            System.DateTime dateTime = System.DateTime.Now;
            Random.InitState((int)dateTime.TimeOfDay.TotalMilliseconds);
        }
        else
        {
            Random.InitState(randomState);
        }

        // First stage: Generate full board
        // Push initial state to stack and start backtracking
        var nextStates = GenerateRandomStates(size, randomState);
        foreach (var state in nextStates)
            stack.Push((0, state));
        // While stack is not empty, search for solution
        while (stack.Count > 0)
        {
            // Get top of stack and check if visited
            var (idx, val) = stack.Pop();
            if (!visited[idx].Contains(val))
            {
                // If not visited, mark as visited and "make a move"
                visited[idx].Add(val);
                board[idx] = val;

                // Check for solution (exit condition)
                if (idx == len * len - 1)
                    break;

                // Clear visited sets from idx + 1 to lastIdx
                // Used to clear visited sets for backtracking
                for (var i = idx + 1; i <= lastIdx; i++)
                {
                    visited[i].Clear();
                    board[i] = 0;
                }
                lastIdx = idx;
                maxIdx = idx > maxIdx ? idx : maxIdx;
            }
            // Get all adjacent vertices of current vertex
            // If an adjacent vertex is not visited, push it to stack
            nextStates = GenerateRandomStates(size, randomState);
            foreach (var state in nextStates)
            {
                if (!visited[idx + 1].Contains(state) && CheckPossible(board, size, idx + 1, state))
                {
                    stack.Push((idx + 1, state));
                }
            }
        }


    private static List<int> RemoveRandomValues(in List<int> solution, in int size, in float removePercentage, int randomState = -1)
    {
        int len = size * size, removedValues = 0, removeCount = (int)(removePercentage * solution.Count);
        List<int> board = new List<int>(solution);
        Stack<int> removed = new Stack<int>();

        // Set Random Initial States
        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);

        // Second stage: remove random numbers from board while ensuring only one solution
        while (removedValues < removeCount)
        {
            var removeIdx = Random.Range(0, board.Count - 1);

            if (!removed.Contains(removeIdx))
            {
                board[removeIdx] = 0;
                removed.Push(removeIdx);

                var (solutionCount, tmp) = SolveBoard(board, size, randomState);
                if (solutionCount == 1)
                    removedValues++;
                else
                    board[removeIdx] = removed.Pop();
            }
        }
        return board;
    }


    private static (int, List<int>) SolveBoard(List<int> board, in int size, int randomState = -1)
    {
        int solutionCount = 0, len = size * size;
        Stack<(int, int)> stack = new Stack<(int, int)>(), unvisit = new Stack<(int, int)>();
        List<int> solution = new List<int>(board);
        List<HashSet<int>> visited = new List<HashSet<int>>();
        HashSet<int> emptyCells = new HashSet<int>();

        // Set Random Initial States
        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);

        // Initialize collections
        for (var i = 0; i < solution.Count; i++)
        {
            visited.Add(new HashSet<int>());
            // Find empty cells
            if (solution[i] == 0)
                emptyCells.Add(i);
        }

        // Solve board
        // Push initial state to stack and start backtracking
        foreach (var idx in emptyCells)
        {
            var nextStates = GenerateRandomStates(size, randomState);
            foreach (var state in nextStates)
                if (!visited[idx].Contains(state) && CheckPossible(board, size, idx, state))
                    stack.Push((idx, state));
        }
        // While stack is not empty, search for solution
        while (stack.Count > 0)
        {
            // Get top of stack and check if visited
            var (idx, val) = stack.Pop();
            if (!visited[idx].Contains(val))
            {
                // If not visited, mark as visited and "make a move"
                visited[idx].Add(val);
                board[idx] = val;
                emptyCells.Remove(idx);
                // Check for solution (exit condition)
                if (CheckValidBoard(solution, size, true))
                {
                    solutionCount++;
                    // We're only interested in cases with:
                    //  "no solution" -> solutionCount == 0,
                    //  "one solution" -> solutionCount == 1, 
                    //  "multiple solutions" -> solutionCount == 2
                    if (solutionCount > 1)
                        break;
                }
            }
            else
            {
                // backtrack
                board[idx] = 0;
                emptyCells.Add(idx);
            }

            // Get all adjacent vertices of current vertex
            // If an adjacent vertex is not visited, push it to stack
            foreach (var emptyIdx in emptyCells)
            {
                var nextStates = GenerateRandomStates(size, randomState);
                foreach (var state in nextStates)
                    if (!visited[emptyIdx].Contains(state) && CheckPossible(board, size, emptyIdx, state))
                        stack.Push((emptyIdx, state));
            }
        }

        return (solutionCount, solution);
    }
}

public class SudokuEasyData : MonoBehaviour
{
    // public static List<SudokuData.SudokuBoardData> GetData()
    // {
    //     List<SudokuData.SudokuBoardData> data = new List<SudokuData.SudokuBoardData>();

    //     data.Add(new SudokuData.SudokuBoardData(
    //         new List<int>(81) {
    //             0, 1, 4, 0, 0, 0, 0, 3, 0,
    //             3, 0, 0, 5, 1, 0, 8, 0, 0,
    //             0, 8, 0, 0, 0, 9, 0, 0, 6,
    //             0, 0, 1, 8, 0, 0, 6, 0, 0,
    //             0, 0, 3, 2, 5, 6, 4, 0, 0,
    //             0, 0, 6, 0, 0, 7, 2, 0, 0,
    //             9, 0, 0, 7, 0, 0, 0, 4, 0,
    //             0, 0, 5, 0, 8, 4, 0, 0, 2,
    //             0, 4, 0, 0, 0, 0, 7, 1, 0
    //         },
    //         new List<int>(81) {
    //             0, 1, 4, 0, 0, 0, 0, 3, 0,
    //             3, 0, 0, 5, 1, 0, 8, 0, 0,
    //             0, 8, 0, 0, 0, 9, 0, 0, 6,
    //             0, 0, 1, 8, 0, 0, 6, 0, 0,
    //             0, 0, 3, 2, 5, 6, 4, 0, 0,
    //             0, 0, 6, 0, 0, 7, 2, 0, 0,
    //             9, 0, 0, 7, 0, 0, 0, 4, 0,
    //             0, 0, 5, 0, 8, 4, 0, 0, 2,
    //             0, 4, 0, 0, 0, 0, 7, 1, 0
    //         }
    //     ));

    //     return data;
    // }

    public static List<SudokuData.SudokuBoardData> GetData(int size, string difficulty)
    {
        List<SudokuData.SudokuBoardData> data = SudokuDataGenerator.GenerateSudokuData(size, difficulty);
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
        // SudokuGame.Add("Medium", SudokuMediumData.GetData());
        // SudokuGame.Add("Hard", SudokuHardData.GetData());
        // SudokuGame.Add("VeryHard", SudokuVeryHardData.GetData());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
