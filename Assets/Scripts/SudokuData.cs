using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.DateTime;
using static System.Int32;

public class SudokuDataGenerator : MonoBehaviour
{
    private const float _easyPercentage = 0.16f, _mediumPercentage = 0.37f, _hardPercentage = 0.58f, _veryHardPercentage = 0.79f;


    public static List<SudokuData.SudokuBoardData> GenerateSudokuData(in int size, in string difficulty, int randomState = -1)
    {
        List<SudokuData.SudokuBoardData> sudokuData = new List<SudokuData.SudokuBoardData>();
        SudokuData.SudokuBoardData data = new SudokuData.SudokuBoardData();

        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);

        data.solution = GenerateSudokuBoard(size, randomState);
        data.board = RemoveRandomValues(data.solution, size, GetDifficultyPercentage(difficulty), randomState);
        sudokuData.Add(data);
        return sudokuData;
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
            for (int j = size * (int)(col / size); j < size * ((int)(col / size) + 1); j++)
                if (i * len + j != idx && board[i * len + j] == number)
                    return false;

        return true;
    }


    private static bool CheckValidBoard(in List<int> board, in int size, in bool checkFull)
    {
        var len = size * size;

        for (int i = 0; i < len; i++)
            for (int j = 0; j < len; j++)
                if ((checkFull && board[i * len + j] == 0) || !CheckPossible(board, size, i * len + j, board[i * len + j]))
                    return false;

        return true;
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


    private static List<int> GenerateSudokuBoard(in int size, int randomState = -1)
    {
        int len = size * size, lastIdx = 0, maxIdx = 0;
        Stack<(int, int)> stack = new Stack<(int, int)>();
        List<int> board = new List<int>();
        List<HashSet<int>> visited = new List<HashSet<int>>();

        // Set Random Initial States
        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);

        // Initialize collections
        for (var i = 0; i < len * len; i++)
        {
            board.Add(0);
            visited.Add(new HashSet<int>());
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
                if (!visited[idx + 1].Contains(state) && CheckPossible(board, size, idx + 1, state))
                    stack.Push((idx + 1, state));
        }

        return board;
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


    private static List<int> RemoveRandomValues(in List<int> solution, in int size, in float removePercentage, int randomState = -1)
    {
        int len = size * size, removedValues = 0, removeCount = (int)(removePercentage * solution.Count), remainingAttempts = 1000;
        List<int> board = new List<int>(solution);
        List<int> filledCells = new List<int>();
        HashSet<int> filledCellsSet = new HashSet<int>(), removedCells = new HashSet<int>();

        // Set Random Initial States
        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);

        // Set filledCells
        for (int i = 0; i < len * len; i++)
            if (board[i] != 0)
            {
                filledCells.Add(i);
                filledCellsSet.Add(i);
            }

        // Second stage: remove random numbers from board while ensuring only one solution
        while (removedValues < removeCount && remainingAttempts-- > 0)
        {
            var filledCellsIdx = Random.Range(0, filledCells.Count - 1);
            var removeIdx = filledCells[filledCellsIdx];

            if (!removedCells.Contains(removeIdx))
            {
                var tmpVal = board[removeIdx];
                board[removeIdx] = 0;
                filledCells.RemoveAt(filledCellsIdx);
                filledCellsSet.Remove(removeIdx);
                removedCells.Add(removeIdx);

                var (solutionCount, tmp) = SolveBoard(board, size, randomState);
                if (solutionCount == 1)
                    removedValues++;
                else
                {
                    board[removeIdx] = tmpVal;
                    filledCells.Add(removeIdx);
                    filledCellsSet.Add(removeIdx);
                    removedCells.Remove(removeIdx);
                }
            }
        }
        return board;
    }


    private static (int, List<int>) SolveBoard(in List<int> board, in int size, int randomState = -1)
    {
        int solutionCount = 0, len = size * size;
        Stack<(List<int>, HashSet<int>)> stack = new Stack<(List<int>, HashSet<int>)>();
        List<int> solution = new List<int>(board);
        HashSet<string> visited = new HashSet<string>();
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
                if (CheckPossible(solution, size, idx, state))
                {
                    var tmpBoard = new List<int>(solution);
                    var tmpEmptyCells = new HashSet<int>(emptyCells);
                    tmpBoard[idx] = state;
                    tmpEmptyCells.Remove(idx);
                    stack.Push((tmpBoard, tmpEmptyCells));
                }
        }

        // While stack is not empty, search for solution
        while (stack.Count > 0)
        {
            // Get top of stack and check if visited
            var (currentState, currentEmptyCells) = stack.Pop();
            if (!visited.Contains(string.Concat(currentState.ToArray())))
            {
                // If not visited, mark as visited and "make a move"
                visited.Add(string.Concat(currentState.ToArray()));
                // Check for solution (exit condition)
                if (currentEmptyCells.Count == 0 && CheckValidBoard(currentState, size, true))
                {
                    solution = currentState;
                    solutionCount++;
                    // We're only interested in cases with:
                    //  "no solution" -> solutionCount == 0,
                    //  "one solution" -> solutionCount == 1, 
                    //  "multiple solutions" -> solutionCount == 2
                    if (solutionCount > 1)
                        break;
                }
            }

            // Get all adjacent vertices of current vertex
            // If an adjacent vertex is not visited, push it to stack
            foreach (var idx in currentEmptyCells)
            {
                var nextStates = GenerateRandomStates(size, randomState);
                foreach (var state in nextStates)
                {
                    if (CheckPossible(currentState, size, idx, state))
                    {
                        var tmpBoard = new List<int>(currentState);
                        var tmpEmptyCells = new HashSet<int>(currentEmptyCells);
                        tmpBoard[idx] = state;
                        tmpEmptyCells.Remove(idx);
                        if (!visited.Contains(string.Concat(tmpBoard.ToArray())))
                            stack.Push((tmpBoard, tmpEmptyCells));
                    }
                }
            }
        }

        return (solutionCount, solution);
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
        SudokuGame.Add("Easy", SudokuDataGenerator.GenerateSudokuData(3, "Easy"));
        SudokuGame.Add("Medium", SudokuDataGenerator.GenerateSudokuData(3, "Medium"));
        SudokuGame.Add("Hard", SudokuDataGenerator.GenerateSudokuData(3, "Hard"));
        SudokuGame.Add("VeryHard", SudokuDataGenerator.GenerateSudokuData(3, "VeryHard"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
