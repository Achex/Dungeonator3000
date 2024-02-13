using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{

    public static System.Random random;

    public static int[][] NewMaze()
    {
        print("Generating new maze");
        random = new();

        int width = 15;
        int height = 15;

        int[][] gridList = GenerateMaze(width, height);

        GenerateMazeImage(gridList, "Assets/Scripts/blackSquare.png", "Assets/Scripts/whiteSquare.png", width, height);

        //PrintMaze(gridList);

        return gridList;
    }

    static int[][] GenerateMaze(int width, int height)
    {
        //width and height must be above 2
        if (width < 2)
            width = 3;
        if (height < 2)
            height = 3;

        //round up width and height to nearest odd number
        if (width % 2 == 0)
            width++;
        if (height % 2 == 0)
            height++;

        //grid of cells with all walls
        int[][] grid = new int[height][];
        for (int i = 0; i < height; i++)
        {
            grid[i] = new int[width];
            for (int j = 0; j < width; j++)
            {
                grid[i][j] = 1;
            }
        }

        //frontier cells
        List<(int, int)> frontier = new List<(int, int)>();

        (int, int) initialCell = (1, 1);
        frontier.Add(initialCell);

        grid[initialCell.Item1][initialCell.Item2] = 0;

        //Bad prim's: maze were favouring horizontal corridors
        // List<(int, int)> Getneighbours1((int, int) cell)
        // {
        //     int row = cell.Item1;
        //     int col = cell.Item2;
        //     List<(int, int)> neighbours = new List<(int, int)>
        //     {
        //         (row - 2, col),
        //         (row + 2, col),
        //         (row, col - 2),
        //         (row, col + 2)
        //     };
            
        //     neighbours.Sort((a, b) => random.Next(-1, 2));
        //     print(random.Next(-1, 2).ToString());
        //     return neighbours.FindAll(neighbour => neighbour.Item1 >= 0 && neighbour.Item1 < height && neighbour.Item2 >= 0 && neighbour.Item2 < width);
        // }


        //HELPER FUNCTION V2
        List<(int, int)> Getneighbours((int, int) cell)
        {
            int row = cell.Item1;
            int col = cell.Item2;

            List<(int, int)> neighbours = new List<(int, int)>
            {
                (row - 2, col),
                (row + 2, col),
                (row, col - 2),
                (row, col + 2)
            };

            // Fisher-Yates shuffle for randomness
            int n = neighbours.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (int, int) value = neighbours[k];
                neighbours[k] = neighbours[n];
                neighbours[n] = value;
            }

            return neighbours.FindAll(neighbour => neighbour.Item1 >= 0 && neighbour.Item1 < height && neighbour.Item2 >= 0 && neighbour.Item2 < width);
        }

        while (frontier.Count > 0)
        {
            (int, int) currentCell = frontier[frontier.Count - 1];
            frontier.RemoveAt(frontier.Count - 1);

            List<(int, int)> neighbours = Getneighbours(currentCell);

            foreach ((int, int) neighbour in neighbours)
            {
                int row = neighbour.Item1;
                int col = neighbour.Item2;
                if (grid[row][col] == 1)
                {
                    grid[row][col] = 0;  //mark the cell as part of the maze
                    frontier.Add(neighbour);
                    //remove the wall between the current cell and the neighbour
                    int wallRow = (row + currentCell.Item1) / 2;
                    int wallCol = (col + currentCell.Item2) / 2;
                    grid[wallRow][wallCol] = 0;
                }
            }
        }

        //ensure the outer border is always blocked
        for (int i = 0; i < height; i++)
        {
            grid[i][0] = 1;
            grid[i][width - 1] = 1;
        }
        for (int j = 0; j < width; j++)
        {
            grid[0][j] = 1;
            grid[height - 1][j] = 1;
        }

        //insert entrance and exit
        int midHeight = height / 2;
        grid[midHeight][0] = 0;
        grid[midHeight][1] = 0;
        grid[midHeight][width - 1] = 0;
        grid[midHeight][width - 2] = 0;

        return grid;
    }

    public static void PrintMaze(int[][] maze)
    {
        string tmp = "\n";
        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze[i].Length; j++)
            {
                if (maze[i][j] == 1)
                    tmp += "# ";
                else
                    tmp += "X ";
            }
            tmp += "\n";
        }
        print(tmp);
    }

    static Texture2D readImage(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return texture;
    }

    static void SaveTextureToFile(Texture2D texture, string filePath)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
    }

    static void GenerateMazeImage(int[][] gridList, string blackSquarePath, string whiteSquarePath, int width, int height)
    {
        Texture2D a = readImage(blackSquarePath);
        Texture2D b = readImage(whiteSquarePath);

        System.Collections.Generic.List<Texture2D> imageConstruction = new System.Collections.Generic.List<Texture2D>();

        //use assets to make maze
        for (int row = 0; row < gridList.Length; row++)
        {
            for (int col = 0; col < gridList[row].Length; col++)
            {
                if (gridList[row][col] == 0)
                {
                    imageConstruction.Add(b);
                }
                else
                {
                    imageConstruction.Add(a);
                }
            }
        }

        Texture2D grid = MakeGrid(imageConstruction, width, height, 0);

        SaveTextureToFile(grid, "Assets/Resources/mazeImage.png");
    }

    static Texture2D MakeGrid(System.Collections.Generic.List<Texture2D> imageConstruction, int width, int height, int padding)
    {
        int ncol = width;
        int nrow = height;
        int cellWidth = imageConstruction[0].width;
        int cellHeight = imageConstruction[0].height;

        Texture2D grid = new Texture2D(ncol * (cellWidth + padding), nrow * (cellHeight + padding));

        for (int row = 0; row < nrow; row++)
        {
            for (int col = 0; col < ncol; col++)
            {
                Texture2D cell = imageConstruction[row * ncol + col];
                grid.SetPixels32(col * (cellWidth + padding), row * (cellHeight + padding), cellWidth, cellHeight, cell.GetPixels32());
            }
        }

        grid.Apply();
        return grid;
    }
}