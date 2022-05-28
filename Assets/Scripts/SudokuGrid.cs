using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public int GridSize = 0;
    public float EverySquareOffset = 0.0f;
    public float GroupOffset = 0.0f;
    public GameObject GridSquare;
    public Vector2 StartPosition = new Vector2(0.0f, 0.0f);
    public float SquareScale = 1.0f;
    private int _columns = 0;
    private int _rows = 0;
    private List<GameObject> _gridSquares = new List<GameObject>();
    private int _selectedGridData = -1;

    void Start()
    {
        if (GridSquare.GetComponent<GridSquare>() == null)
            Debug.LogError("gridSquare must have a GridSquare script attached to it");
        _columns = _rows = GridSize * GridSize;
        CreateGrid();
        SetGridNumbers("Easy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        // 0, 1, 3, 4, 5, 6
        // 7, 8, 9, 10, ...
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                _gridSquares.Add(Instantiate(GridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform); // instantiate this game object as a child of the grid
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(SquareScale, SquareScale, SquareScale);
            }
        }
    }

    private void SetSquaresPosition()
    {
        var squareRect = _gridSquares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + EverySquareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + EverySquareOffset;

        int rowNumber = 0, columnNumber = 0;

        foreach (var square in _gridSquares)
        {
            if (columnNumber + 1 > _columns)
            {
                rowNumber++;
                columnNumber = 0;
            }
            var posXOffset = offset.x * columnNumber + (int)(columnNumber / GridSize) * GroupOffset;
            var posYOffset = offset.y * rowNumber + (int)(rowNumber / GridSize) * GroupOffset;
            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(StartPosition.x + posXOffset, StartPosition.y - posYOffset);
            columnNumber++;
        }
    }

    private void SetGridNumbers(string level)
    {
        _selectedGridData = Random.Range(0, SudokuData.Instance.SudokuGame[level].Count - 1);
        var data = SudokuData.Instance.SudokuGame[level][_selectedGridData];

        SetGridSquareData(data);
        // foreach (var square in _gridSquares)
        // {
        //     square.GetComponent<GridSquare>().SetNumber(Random.Range(0, (GridSize * GridSize) + 1));
        // }
    }

    private void SetGridSquareData(SudokuData.SudokuBoardData data)
    {
        for (int i = 0; i < _gridSquares.Count; i++)
        {
            _gridSquares[i].GetComponent<GridSquare>().SetNumber(data.board[i]);
        }
    }
}
