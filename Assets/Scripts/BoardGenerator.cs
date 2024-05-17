using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab, tileParent;
    [SerializeField] private int height = 5, width = 5;
    [SerializeField] int bombAmount = 5;
    public int Rows
    {
        get => height;
        set => height = value;
    }
    
    public int Columns
    {
        get => width;
        set => width = value;
    }

    public int Bombs
    {
        get => bombAmount;
        set => bombAmount = value;
    }
    
    private Cell[,] gridArray = new Cell[0,0];

    public static readonly List<Cell> correctlyMarkedBombs = new();

    public static bool hasStarted;

    private DateTime timeStarted;
    
    //UI

    [SerializeField] private TextMeshProUGUI winOrLoseText, timeText;
    [SerializeField] private GameObject gameOverPanel;

    private void Start()
    {
        GenerateEmptyBoard();
    }

    void GenerateEmptyBoard()
    {
        RectTransform parentRt = tileParent.GetComponent<RectTransform>();

        parentRt.sizeDelta = new Vector2(width * 100, height * 100);
        
        gridArray = new Cell[width, height];

        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                gridArray[x, y] = new(x, y);

                GameObject tileInWorld = Instantiate(tilePrefab, tileParent.transform);
                //tileInWorld.transform.position = new Vector3(i, j, 0);
                tileInWorld.GetComponent<CellObject>().Setup(gridArray[x, y], RevealOtherTiles, GameOver, CheckWin, StartGame);
            }
        }
    }

    void StartGame(Cell starterCell)
    {
        hasStarted = true;
        timeStarted = DateTime.Now;
        
        AddBombs(starterCell);
    }

    public void ResetGame()
    {
        foreach(Transform t in tileParent.transform) Destroy(t.gameObject);

        hasStarted = false;
        
        correctlyMarkedBombs.Clear();
        
        gameOverPanel.SetActive(false);
        
        GenerateEmptyBoard();
    }
    

    void AddBombs(Cell starterCell)
    {
        Debug.Log("Starting game.");
        
        List<Cell> surroundingCells = GetSurroundingCells(starterCell).ToList();
        surroundingCells.Add(starterCell);
        
        List<Cell> tilesWithBombs = new();
        for (int i = 0; i < bombAmount; i++)
        {
            int randomX = Random.Range(0, width);
            int randomY = Random.Range(0, height);

            Cell chosenTile = gridArray[randomX, randomY];

            if (!tilesWithBombs.Contains(chosenTile) && !surroundingCells.Contains(chosenTile))
            {
                chosenTile.AddBomb();
                tilesWithBombs.Add(chosenTile);
            }
            else i--;
        }
        
        SetNumbers();
        
        RevealOtherTiles(starterCell);
    }

    void SetNumbers()
    {
        foreach (Cell cell in gridArray)
        {
            cell.SetNumber(NumberOfSurroundingBombs(cell));
        }
    }

    void RevealOtherTiles(Cell emptyCell)
    {
        foreach (Cell cell in GetSurroundingCells(emptyCell))
        {
            cell.holderObject.GetComponent<CellObject>().Click();
        }
    }

    void GameOver()
    {
        Debug.Log("Game over!");
        gameOverPanel.SetActive(true);

        winOrLoseText.text = "Game over!";
        timeText.text = "";
    }

    void CheckWin()
    {
        if (correctlyMarkedBombs.Count == bombAmount)
        {
            Debug.Log("Win!");
            gameOverPanel.SetActive(true);

            winOrLoseText.text = "You win!";

            TimeSpan duration = DateTime.Now - timeStarted;

            timeText.text = duration.Minutes + "m" + duration.Seconds + "s";
        }
    }

    private int NumberOfSurroundingBombs(Cell cellToCheck)
    {
        int numberOfSurroundingBombs = 0;

        foreach (Cell cell in GetSurroundingCells(cellToCheck))
        {
            if (cell.HasBomb) numberOfSurroundingBombs++;
        }

        return numberOfSurroundingBombs;
    }

    private Cell[] GetSurroundingCells(Cell middleCell)
    {
        //We need to look in the perimeter of the cell if there is a bomb
        //The perimeter is:
        //  (x--, y--)     (x, y--)    (x++, y--)
        //   (x--, y)       (x, y)      (x++, y)
        //  (x--, y++)     (x, y++)    (x++, y++)

        List<Cell> surroundingCells = new(8);

        for (int x = -1; x < 2; x++)
        {
            //check cell x, y

            for (int y = -1; y < 2; y++)
            {
                if (middleCell.posX + x < 0 || middleCell.posY + y < 0) continue;
                if (middleCell.posX + x >= width || middleCell.posY + y >= height) continue;
                if (x == 0 && y == 0) continue;
                
                surroundingCells.Add(gridArray[middleCell.posX + x, middleCell.posY + y]);
            }
        }

        return surroundingCells.ToArray();
    }

    public void ChangeRows(int value)
    {
        height = value;
    }
    
    public void ChangeColumns(int value)
    {
        width = value;
    }
}

