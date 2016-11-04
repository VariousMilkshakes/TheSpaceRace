using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
	/*
	* The number of columns (x) and rows (y). The dimensions of the grid.
	*/
	public int columns;
	public int rows;

	/*
	* The seed to pass to the random number generator in the RandomiseGrid method.
	* The boolean decides whether to use the given seed or generate a rondom float between -1000 and 1000.
	*/
	public string seed;
	public bool randomSeed;

	/*
	* The square brackets here are whats known as an attribute in C# and in this instance they and their content define the range that waterPercentage can take.
	* see https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx
	* 
	* The range is between 0 and 100 as it is percentage.
	* The percentage of the map covered in water before smoothing and processing.
	*/
	[Range(0, 100)]
	public int waterPercentage;

	/*
	* The array of possible sprites that a Tile can be passed to each Tile in the SetUpMap method.
	* It's values are set in the editor.
	*/
	public Sprite[] sprites;

	/*
	* The position of the bottom left hand corner of the grid.
	*/
	private Transform mapHolder;

	/*
	* The two dimensional integer array of grid positions used to refer to each position on the grid.
	*/
	int[,] gridPos;

	/*
	* Called once at the start of the program.
	*/
	void Start()
	{
		GenerateMap();
		SetUpMap();
	}

	/*
	* This method is an amalgam of other methods.
	* It is also responsible for instanciateing gridPos.
	* Calls Smooth 5 times to ensure smoothest map.
	*/
	void GenerateMap()
	{
		gridPos = new int[columns, rows];
		RandomiseGrid();
		for (int i = 0; i < 5; i++)
		{
			Smooth();
		}

		ProcessGrid();
	}

	/*
	* This method uses foreach loops to iterate over each region (see GetRegionTiles) and if that region is smaller thatn the thershold size,
	* 	sets the type of that tile to the opposite type.
	*/
	void ProcessGrid()
	{
		List<List<Coord>> waterRegions = GetRegions(1);
		int waterThresholdSize = 20;

		foreach (List<Coord> waterRegion in waterRegions)
		{
			if (waterRegion.Count < waterThresholdSize)
			{
				foreach (Coord tile in waterRegion)
				{
					gridPos[tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> grassRegions = GetRegions(0);
		int grassThresholdSize = 20;

		foreach (List<Coord> grassRegion in grassRegions)
		{
			if (grassRegion.Count < grassThresholdSize)
			{
				foreach (Coord tile in grassRegion)
				{
					gridPos[tile.tileX, tile.tileY] = 1;
				}
			}
		}
	}

	/*
	* This method creates a List of regions (see GetRegionTiles) of the specified type and returns this list.
	*/
	List<List<Coord>> GetRegions(int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>>();
		int[,] gridFlags = new int[columns, rows];

		for (int x = 0; x < columns; x++)
		{
			for (int y = 0; y < rows; y++)
			{
				if (gridFlags[x, y] == 0 && gridPos[x, y] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(x, y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion)
					{
						gridFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}


	/*
	* This method takes two parameters a starting x value and a starting y value.
	* It creates a List of all the tiles of the same type in a region. (Where a region is a continuous area of tiles of the same type.)
	* It then returns this list.
	*/
	List<Coord> GetRegionTiles(int startX, int startY)
	{
		List<Coord> tiles = new List<Coord>();
		int[,] gridFlags = new int[columns, rows];
		int tileType = gridPos[startX, startY];

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		gridFlags[startX, startY] = 1;

		while (queue.Count > 0)
		{
			Coord tile = queue.Dequeue();
			tiles.Add(tile);
			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
			{
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
				{
					if (IsInRange(x, y) && (y == tile.tileY || x == tile.tileX))
					{
						if (gridFlags[x, y] == 0 && gridPos[x, y] == tileType)
						{
							gridFlags[x, y] = 1;
							queue.Enqueue(new Coord(x, y));
						}
					}
				}
			}
		}
		return tiles;
	}

	/*
	* Returns true if the grid position given is within the grid. i.e. is not beyond the edges of the grid.
	*/
	bool IsInRange(int x, int y)
	{
		return x >= 0 && x < columns && y >= 0 && y < rows;
	}

	/*
	 * This method Randomises all of the values in gridPos to be either 0 (a grass tile) or 1 (a water tile).
	 * This is achieved by iterating over each element in the array and using Random.Next with a range of 0 - 100 and comparing the value with the waterPercentage int.
	 * If the result is less than waterPercentage the type of the tile is set 1, otherwise 0.
	*/
	void RandomiseGrid()
	{
		if (randomSeed)
		{
			seed = Random.Range(-1000f, 1000f).ToString();
		}
		System.Random rnd = new System.Random(seed.GetHashCode());

		for (int x = 0; x < columns; x++)
		{
			for (int y = 0; y < rows; y++)
			{
				gridPos[x, y] = (rnd.Next(0, 100) < waterPercentage) ? 1 : 0;
			}
		}
	}

	/*
	* This method iterates over all grid positions and changes their value based on the eight surrounding grid positions.
	* This method calls the GetAdjacentWaterCount method.
	* If there are more than four water tiles adjacent the tile becomes a water tile.
	* If there are fewer than four water tiles adjacent the tile becomes a grass tile.
	* If there are four water tiles adjacent the tile stays as it was.
	*/
	void Smooth()
	{
		for (int x = 0; x < columns; x++)
		{
			for (int y = 0; y < rows; y++)
			{
				int adjacentTiles = GetAdjacentWaterCount(x, y);
				if (adjacentTiles > 4)
				{
					gridPos[x, y] = 1;
				}
				else if (adjacentTiles < 4)
				{
					gridPos[x, y] = 0;
				}
			}
		}
	}


	/*
	* This method iterates over the eight positions adjacent to the given grid position and evaluates its type. 
	* As water tiles are of type 1, simply returning the sum of all adjacent tiles will return the number of adjacent water tiles.
	*/
	int GetAdjacentWaterCount(int posX, int posY)
	{
		int adjacentTiles = 0;
		for (int adjX = posX - 1; adjX <= posX + 1; adjX++)
		{
			for (int adjY = posY - 1; adjY <= posY + 1; adjY++)
			{
				if (adjX >= 0 && adjX < columns && adjY >= 0 && adjY < rows)
				{
					if (adjX != posX || adjY != posY)
					{
						adjacentTiles += gridPos[adjX, adjY];
					}
				}
			}
		}
		return adjacentTiles;
	}

	/*
	* This method is responsable for creating the Tile of the type as set in gridPos by the RandomiseGrid method.
	*/
	void SetUpMap()
	{
		mapHolder = new GameObject("Map").transform;

		if (gridPos != null)
		{
			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					Tile tile = new Tile(gridPos[x, y], sprites);
					GameObject toInstantiate = tile.tile;
					GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent(mapHolder);
				}
			}
		}
	}

	/*
	* A struct is a data type similar to a class but is a value type rather than a refernce type.
	* see: https://msdn.microsoft.com/en-us/library/aa288471(v=vs.71).aspx
	* 
	* This struct defines a type called Coord that is a pair of integer coordinates representing one of the tiles in the map.
	*/
	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}
}
