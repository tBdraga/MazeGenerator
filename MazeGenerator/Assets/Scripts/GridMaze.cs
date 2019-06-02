using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Maze;

public class GridMaze : MonoBehaviour
{
    //cells
    public Cell[] cells;
    //references
    public Maze maze;
    public Selection selection;
    //variables
    public Vector3 startingPoint;
    public Vector3 endingPoint;
    //***************************
    public Node[,] grid;
    public List<Node> finalPath;

    public float nodeRadius;
    public float NodeDiameter;
    public int gridSizeX, gridSizeY;


    // Start is called before the first frame update
    void Start()
    {
        maze = new Maze();
        selection = new Selection();
        initializeGridSize();
        initializeCellArray();
        initializePositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializeCellArray() {
        //cell array initialized
        cells = maze.cells;
    }

    public void initializePositions() {
        //staring positions initialized
        startingPoint = selection.spawnPosition;
        endingPoint = selection.finishPosition;
    }

    public void initializeGridSize() {
        gridSizeX = maze.xSize;
        gridSizeY = maze.ySize;
    }
}
