using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab, tileParent;

    [SerializeField]
    private int height = 5, width = 5, amountOfBombs = 5;

    private Tile[,] gridArray = new Tile[0,0];
    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTiles()
    {
        gridArray = new Tile[height, width];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = new Tile() { posX = i, posY = j };

                GameObject tileInWorld = Instantiate(tilePrefab, tileParent.transform);
                tileInWorld.transform.position = new Vector3(i, j, 0);

                gridArray[i, j].tile = tileInWorld;
            }
        }

    }

    private void GenerateBombs()
    {        
        List<Tile> tilesWithoutBombs = new List<Tile>();

        foreach(Tile tile in gridArray)
        {
            tilesWithoutBombs.Add(tile);
        }


        int randomX, randomY;

        

        for (int i = 0; i < amountOfBombs; i++)
        {
            
        }
        
    }

    private Vector2 RandomTile()
    {
        return new Vector2(Random.Range(0, gridArray.GetLength(0)), Random.Range(0, gridArray.GetLength(1)));
    }
}


[System.Serializable]
public class Tile
{
    public int posX, posY;

    public GameObject tile;

    public bool hasBomb = false;
}
