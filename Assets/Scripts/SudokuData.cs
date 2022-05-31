using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static System.DateTime;
using static System.Int32;

public class SudokuDataGenerator : MonoBehaviour
{
    private const float _easyPercentage = 0.25f, _mediumPercentage = 0.45f, _hardPercentage = 0.65f, _veryHardPercentage = 0.75f;


    private struct CheckFullValidBoardJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<int> board;

        [NativeDisableParallelForRestriction] public NativeArray<short> isValid;
        public int size;

        //Constructor
        public CheckFullValidBoardJob(int size, NativeArray<short> isValid, NativeArray<int> board)
        {
            this.size = size;
            this.isValid = isValid;
            this.board = board;
        }


        public void Execute(int idx)
        {
            var len = size * size;
            int row = idx / len, col = idx % len;
            var number = board[idx];
            // If empty cell or isValid is already false, return false
            if (board[idx] == 0 || isValid[0] == 0)
            {
                isValid[0] = 0;
                return;
            }
            // Check row
            for (int j = 0; j < len; j++)
                if (row * len + j != idx && board[row * len + j] == number)
                {
                    isValid[0] = 0;
                    return;
                }
            // Check column
            for (int i = 0; i < len; i++)
                if (i * len + col != idx && board[i * len + col] == number)
                {
                    isValid[0] = 0;
                    return;
                }
            // Check square
            for (int i = size * (int)(row / size); i < size * ((int)(row / size) + 1); i++)
                for (int j = size * (int)(col / size); j < size * ((int)(col / size) + 1); j++)
                    if (i * len + j != idx && board[i * len + j] == number)
                    {
                        isValid[0] = 0;
                        return;
                    }
            isValid[0] = 1;
        }
    }


    private struct CheckPossibleRowJob : IJob
    {
        [ReadOnly] public NativeArray<int> board;

        public NativeArray<short> isValid;
        public int size, idx, number;

        //Constructor
        public CheckPossibleRowJob(int size, int idx, int number, NativeArray<short> isValid, NativeArray<int> board)
        {
            this.size = size;
            this.idx = idx;
            this.number = number;
            this.isValid = isValid;
            this.board = board;
        }


        public void Execute()
        {
            var len = size * size;
            int row = idx / len, col = idx % len;
            // Check row
            for (int j = 0; j < len; j++)
                if (row * len + j != idx && board[row * len + j] == number)
                    isValid[0] = 0;
        }
    }


    private struct CheckPossibleColumnJob : IJob
    {
        [ReadOnly] public NativeArray<int> board;

        public NativeArray<short> isValid;
        public int size, idx, number;

        //Constructor
        public CheckPossibleColumnJob(int size, int idx, int number, NativeArray<short> isValid, NativeArray<int> board)
        {
            this.size = size;
            this.idx = idx;
            this.number = number;
            this.isValid = isValid;
            this.board = board;
        }


        public void Execute()
        {
            var len = size * size;
            int row = idx / len, col = idx % len;
            // Check column
            for (int i = 0; i < len; i++)
                if (i * len + col != idx && board[i * len + col] == number)
                    isValid[0] = 0;
        }
    }


    private struct CheckPossibleSquareJob : IJob
    {
        [ReadOnly] public NativeArray<int> board;

        public NativeArray<short> isValid;
        public int size, idx, number;

        //Constructor
        public CheckPossibleSquareJob(int size, int idx, int number, NativeArray<short> isValid, NativeArray<int> board)
        {
            this.size = size;
            this.idx = idx;
            this.number = number;
            this.isValid = isValid;
            this.board = board;
        }


        public void Execute()
        {
            var len = size * size;
            int row = idx / len, col = idx % len;
            // Check square
            for (int i = size * (int)(row / size); i < size * ((int)(row / size) + 1); i++)
                for (int j = size * (int)(col / size); j < size * ((int)(col / size) + 1); j++)
                    if (i * len + j != idx && board[i * len + j] == number)
                        isValid[0] = 0; ;
        }
    }


    public static SudokuData.SudokuBoardData GenerateSudokuData(in ushort size, in string difficulty, int randomState = -1)
    {

        if (randomState == -1)
        {
            var dateTime = System.DateTime.Now;
            randomState = (int)dateTime.TimeOfDay.TotalMilliseconds;
        }
        Random.InitState(randomState);
        // SudokuData.SudokuBoardData data = new SudokuData.SudokuBoardData();

        var solution = GenerateSudokuBoard(size, Random.Range(0, int.MaxValue));
        var board = RemoveRandomValues(solution, size, GetDifficultyPercentage(difficulty), Random.Range(0, int.MaxValue));
        return new SudokuData.SudokuBoardData(board, solution);
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
        var nextStates = GenerateRandomStates(size, Random.Range(0, int.MaxValue));
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
            nextStates = GenerateRandomStates(size, Random.Range(0, int.MaxValue));
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
            case "VeryHard":
                return _veryHardPercentage;
            default:
                return 0.0f;
        }
    }


    private static List<int> RemoveRandomValues(in List<int> solution, in int size, in float removePercentage, int randomState = -1)
    {
        int len = size * size, removedValues = 0, removeCount = (int)(removePercentage * solution.Count), remainingAttempts = solution.Count * solution.Count;
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

                var solutionCount = SolveBoard(board, size, Random.Range(0, int.MaxValue));
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


    private static int SolveBoard(in List<int> board, in int size, int randomState = -1)
    {
        int solutionCount = 0;
        int len = size * size;
        Stack<(List<int>, HashSet<int>)> stack = new Stack<(List<int>, HashSet<int>)>();
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
        for (var i = 0; i < board.Count; i++)
        {
            // Find empty cells
            if (board[i] == 0)
                emptyCells.Add(i);
        }

        // Solve board
        // Push initial state to stack and start backtracking
        foreach (var idx in emptyCells)
        {
            var nextStates = GenerateRandomStates(size, Random.Range(0, int.MaxValue));
            foreach (var state in nextStates)
                if (CheckPossible(board, size, idx, state))
                {
                    var tmpBoard = new List<int>(board);
                    var tmpEmptyCells = new HashSet<int>(emptyCells);
                    tmpBoard[idx] = state;
                    tmpEmptyCells.Remove(idx);
                    stack.Push((tmpBoard, tmpEmptyCells));
                }
        }

        // HashSet<JobHandle> jobs = new HashSet<JobHandle>();
        NativeArray<int> sharedBoard = new NativeArray<int>(board.ToArray(), Allocator.TempJob);
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
                if (currentEmptyCells.Count == 0)
                {
                    // Start of paralell section
                    NativeArray<short> isValid = new NativeArray<short>(new short[] { -1 }, Allocator.TempJob);
                    var job = new CheckFullValidBoardJob(size, isValid, sharedBoard);
                    var jobHandle = job.Schedule(sharedBoard.Length, size * size);
                    jobHandle.Complete();
                    if (isValid[0] == 1)
                    {
                        solutionCount++;
                        // We're only interested in cases with:
                        //  "no solution" -> solutionCount == 0,
                        //  "one solution" -> solutionCount == 1, 
                        //  "multiple solutions" -> solutionCount == 2
                        if (solutionCount > 1)
                            break;
                    }
                    isValid.Dispose();
                    // End of parallel section
                }
            }

            // Get all adjacent vertices of current vertex
            // If an adjacent vertex is not visited, push it to stack
            foreach (var idx in currentEmptyCells)
            {
                var nextStates = GenerateRandomStates(size, Random.Range(0, int.MaxValue));
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
        sharedBoard.Dispose();

        return solutionCount;
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

    private string _difficulty;
    private ushort _size;

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
        _difficulty = "Easy";
        _size = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public SudokuData.SudokuBoardData GenerateGame()
    {
        return SudokuDataGenerator.GenerateSudokuData(_size, _difficulty);
    }


    public string GetDifficulty()
    {
        return _difficulty;
    }


    public ushort GetSize()
    {
        return _size;
    }


    public void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
            case "Medium":
            case "Hard":
            case "VeryHard":
                _difficulty = difficulty;
                break;
            default:
                throw new System.ArgumentException("Invalid difficulty. Must be one of: Easy, Medium, Hard, VeryHard");
        }
        _difficulty = difficulty;
    }


    public void SetSize(ushort size)
    {
        if (size < 1 || size > 9)

            throw new System.ArgumentOutOfRangeException("size", "Size must be between 1 and 9");
        _size = size;
    }
}
