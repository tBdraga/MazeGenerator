using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{

    //clasa celule
    [System.Serializable]
    public class Cell {
        public bool visited;
        public GameObject north;     //1
        public GameObject east;     //2
        public GameObject west;      //3
        public GameObject south;      //4
    }

    public GameObject wall;
    public GameObject floor;
    public float wallLength = 1.0f;
    public int xSize = 5;
    public int ySize = 5;

    private Vector3 initialPos;

    private GameObject wallHolder;

    public Cell[] cells;
    private int currentCell=0;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;


    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();    
    }


    public void CreateWalls() {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        initialPos = new Vector3((-xSize / 2) + wallLength/2, 0.0f , (-ySize/2) + wallLength/2);
        Vector3 myPos = initialPos;
        GameObject tempWall;
        GameObject tempFloor;

        //x Axis
        for (int i = 0; i < ySize; i++) {
            for (int j = 0; j <= xSize; j++) {
                myPos = new Vector3(initialPos.x +(j*wallLength)-wallLength/2, 0.0f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall,myPos,Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //y Axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), 0.0f, initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //floor
        for (int i = 0; i <= ySize - 1; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), -0.5f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempFloor = Instantiate(floor, myPos, Quaternion.identity) as GameObject;
                tempFloor.transform.parent = wallHolder.transform;
            }
        }

        CreateCells();
    }

    public void CreateCells() {
        totalCells = xSize * ySize;
        //array of gameobjects
        GameObject[] allWalls;
        //how many children the wallHolder gameobject holds
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];

        cells = new Cell[xSize*ySize];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;


        //Get All Wall children
        for (int i = 0; i < children; i++) {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assign walls to cells
        for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++) {
            cells[cellprocess] = new Cell();
            cells[cellprocess].east = allWalls[eastWestProcess];
            //first horizontal wall is generated after the last vertical wall, that's why(xSize+1)*ySize
            cells[cellprocess].south = allWalls[childProcess +(xSize+1)*ySize];

            if (termCount == xSize)
            {
                eastWestProcess += 2;
                termCount = 0;
            }
            else {
                eastWestProcess++;
            }

            termCount++;
            childProcess++;
            cells[cellprocess].west = allWalls[eastWestProcess];
            cells[cellprocess].north = allWalls[(childProcess + (xSize + 1) * ySize) +  xSize-1];
                
        }
        CreateMaze();
    }

    public void CreateMaze() { 
        lastCells = new List<int>();
        lastCells.Clear();

        while (visitedCells < totalCells) {
            if (startedBuilding)
            {
                GiveNeighbour();
                if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true) {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;

                    if (lastCells.Count > 0) {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else {
                currentCell = Random.Range(0,totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }

        }

        Debug.Log("Finished");
    }

    public void BreakWall() {
        switch (wallToBreak)
        {
            case 1: Destroy(cells[currentCell].north);break;
            case 2: Destroy(cells[currentCell].east); break;
            case 3: Destroy(cells[currentCell].west); break;
            case 4: Destroy(cells[currentCell].south); break;
        }
    }

    public void GiveNeighbour() {
        int length = 0;
        int[] neigbours = new int[4];
        int[] connectingWall = new int[4];  
        //cornering the cell
        int check = 0;
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if (currentCell + 1 < totalCells && (currentCell + 1) != check) {
            if (cells[currentCell + 1].visited == false) {
                neigbours[length] = currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }
        
        //east  
        if (currentCell - 1 >= 0 && currentCell != check) {
            if (cells[currentCell - 1].visited == false) {
                neigbours[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }

        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neigbours[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //south
        if (currentCell - xSize >=0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neigbours[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neigbours[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }
        else {
            if (backingUp > 0) {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }

    }
}
