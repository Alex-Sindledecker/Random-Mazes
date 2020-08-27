using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private enum Direction
    {
        NONE = -1, UP = 0, DOWN = 180, LEFT = 90, RIGHT = 270
    }

    public int mazeSize = 50;
    public GameObject hallway;
    public GameObject corner;

    private bool[][] visitedCells;
    private System.Random r;

    // Start is called before the first frame update
    void Start()
    {
        r = new System.Random();
        BuildMaze();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BuildMaze()
    {
        visitedCells = new bool[mazeSize][];
        for (int row = 0; row < mazeSize; row++)
        {
            visitedCells[row] = new bool[mazeSize];
            for (int col = 0; col < mazeSize; col++)
                visitedCells[row][col] = false;
        }
        BuildMaze(0, 0, Direction.NONE);
    }

    void BuildMaze(int x, int z, Direction direction)
    {
        visitedCells[x][z] = true;
        foreach (int i in Enumerable.Range(0, 4).OrderBy(_x => r.Next()))
        {
            //Rotate hallway 90 degress on the y axis when facing in the z
            switch (i)
            {
                case 0:
                    if (ValidCell(x + 1, z))
                    {
                        BuildMaze(x + 1, z, Direction.RIGHT); 
                        Debug.DrawLine(new Vector3(x, 0, z), new Vector3(x + 1, 0, z), Color.red, 10000000f);
                        CreatePathway(x, z, direction, Direction.RIGHT);
                    }
                    break;
                case 1:
                    if (ValidCell(x - 1, z)) 
                    { 
                        BuildMaze(x - 1, z, Direction.LEFT);
                        Debug.DrawLine(new Vector3(x, 0, z), new Vector3(x - 1, 0, z), Color.red, 10000000f);
                        CreatePathway(x, z, direction, Direction.LEFT);
                    }
                    break;
                case 2:
                    if (ValidCell(x, z + 1))
                    {
                        BuildMaze(x, z + 1, Direction.UP);
                        Debug.DrawLine(new Vector3(x, 0, z), new Vector3(x, 0, z + 1), Color.red, 10000000f);
                        CreatePathway(x, z, direction, Direction.UP);
                    }
                    break;
                case 3:
                    if (ValidCell(x, z - 1))
                    { 
                        BuildMaze(x, z - 1, Direction.DOWN);
                        Debug.DrawLine(new Vector3(x, 0, z), new Vector3(x, 0, z - 1), Color.red, 10000000f);
                        CreatePathway(x, z, direction, Direction.DOWN);
                    }
                    break;
            }
        }
    }

    bool ValidCell(int row, int col)
    {
        if (row >= 0 && row < mazeSize && col >= 0 && col < mazeSize)
        {
            if (visitedCells[row][col] == false)
                return true;
        }
        return false;
    }

    void CreatePathway(int x, int z, Direction oldDir, Direction newDir)
    {
        if (oldDir == Direction.NONE)
        {
            Instantiate(hallway, new Vector3(x, 0, z), Quaternion.Euler(-90, (float)newDir, 0), transform);
        }
        else if (oldDir == newDir)
        {
            Instantiate(hallway, new Vector3(x, 0, z), Quaternion.Euler(-90, (float)oldDir, 0), transform);
        }
        else
        {
            if (oldDir == Direction.UP && newDir == Direction.LEFT)
                Instantiate(corner, new Vector3(x, 0, z), Quaternion.Euler(-90, 0, 0), transform);
            else if (oldDir == Direction.UP && newDir == Direction.RIGHT)
                Instantiate(corner, new Vector3(x, 0, z), Quaternion.Euler(-90, -90, 0), transform);
            else if (oldDir == Direction.RIGHT && newDir == Direction.UP)
                Instantiate(corner, new Vector3(x, 0, z), Quaternion.Euler(-90, 90, 0), transform);
            else if (oldDir == Direction.RIGHT && newDir == Direction.UP)
                Instantiate(corner, new Vector3(x, 0, z), Quaternion.Euler(-90, 0, 0), transform);
        }
    }
}
