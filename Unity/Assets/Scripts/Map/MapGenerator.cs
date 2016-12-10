using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
	/// <summary>
	/// The number of columns (x).
	/// </summary>
	[Range (1, 100)]
	public int columns;

	/// <summary>
	/// The number of rows (y).
	/// </summary>
	[Range (1, 100)]
	public int rows;

	/// <summary>
	/// The seed to pass to the random number generator in the RandomiseGrid method.
	/// </summary>
	public string seed;

	/// <summary>
	/// The boolean decides whether to use the given seed or generate a rondom float between -1000 and 1000.
	/// </summary>
	public bool randomSeed;


	/// <summary>
	/// The percentage of the map covered in water before smoothing and processing.
	/// </summary>
	/// <see cref="https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx"/>
	[Range(0, 100)]
	public float waterPercentage;

	/// <summary>
	/// The sprites of the map tiles.
	/// </summary>
	public Sprite[] sprites;

	/// <summary>
	/// The hover sprite.
	/// </summary>
	public Sprite hoverSprite;

	/// <summary>
	/// The selected sprite.
	/// </summary>
	public Sprite selectedSprite;

	/// <summary>
	/// The building sprite.
	/// </summary>
	public Sprite buildingSprite;

	/// <summary>
	/// The static sprites.
	/// </summary>
	private Sprite[] staticSprites;

	/// <summary>
	/// The position of the bottom left hand corner of the grid.
	/// </summary>
	private Transform mapHolder;

	/// <summary>
	/// The tiles.
	/// </summary>
	[HideInInspector]
	public List<Tile> tiles;

	/// <summary>
	/// The two dimensional integer array of grid positions used to refer to each position on the grid.
	/// </summary>
	int[,] gridPos;

	public enum Types
	{
		GRASS, WATER, SAND, MOUNTIAN
	}

	/// <summary>
	/// Gets the sprite.
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="type">Type.</param>
	public Sprite GetSprite(int type){
		return sprites [type];
	}

	/// <summary>
	/// Gets the static sprites.
	/// </summary>
	/// <returns>The static sprites.</returns>
	public Sprite[] GetStaticSprites(){
		return staticSprites;
	}

	/*
	* Setters
	*/
	/// <summary>
	/// Sets the width of the map.
	/// </summary>
	/// <param name="width">Width.</param>
	public void SetMapWidth(float width){
		columns = (int)width;
	}

	/// <summary>
	/// Sets the height of the map.
	/// </summary>
	/// <param name="height">Height.</param>
	public void SetMapHeight(float height){
		rows = (int)height;
	}

	/// <summary>
	/// Sets the map water percentage.
	/// </summary>
	/// <param name="water">Water.</param>
	public void SetMapWaterPercentage(float water){
		waterPercentage = water;
	}

	/// <summary>
	/// Sets the map seed.
	/// </summary>
	/// <param name="seed">Seed.</param>
	public void SetMapSeed(string seed){
		randomSeed = false;
		this.seed = seed;
	}

	/// <summary>
	/// Gets the grid position.
	/// </summary>
	/// <returns>The grid position.</returns>
	public int[,] GetGridPos() {
		return gridPos;
	}

	/// <summary>
	/// Gets the tiles.
	/// </summary>
	/// <returns>The tiles.</returns>
	public List<Tile> GetTiles() {
		return tiles;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start(){
		staticSprites = new Sprite[3];
		staticSprites [0] = hoverSprite;
		staticSprites [1] = selectedSprite;
		staticSprites [2] = buildingSprite;
		tiles = new List<Tile> ();
		GenerateMap ();
		SetUpMap ();
	}

	/// <summary>
	/// Gets the tile.
	/// </summary>
	/// <returns>The tile.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile GetTile(int x, int y){
		foreach (Tile t in tiles) {
			if (t.transform.position.x  == x && t.transform.position.y == y) {
				return t;
			}
		} throw new NoTileException ("No such Tile");
	}

	/// <summary>
	/// Generates the map.
	/// </summary>
	/// <description>
	/// This method is an amalgam of other methods.
	/// It is also responsible for instanciateing gridPos.
	/// Calls Smooth 5 times to ensure smoothest map.
	/// </description>
	void GenerateMap(){
		gridPos = new int[columns, rows];
		RandomiseGrid ();
		for (int i = 0; i < 5; i++) {
			Smooth ();
		}

		ProcessGrid ();
	}

	/// <summary>
	/// Processes the grid.
	/// </summary>
	/// <description>
	/// This method uses foreach loops to iterate over each region (see GetRegionTiles) and if that region is smaller thatn the thershold size,
	/// sets the type of that tile to the opposite type.
	/// </description>
	void ProcessGrid(){
		List<List<Coord>> waterRegions = GetRegions ((int)Types.WATER);
		int waterThresholdSize = 20;

		//remove regions of water tiles smaller than 20 tiles in area.
		foreach (List<Coord> waterRegion in waterRegions) {
			if(waterRegion.Count < waterThresholdSize){
				foreach (Coord tile in waterRegion){
					gridPos [tile.tileX, tile.tileY] = (int)Types.GRASS;
				}
			}
		}

		List<List<Coord>> grassRegions = GetRegions ((int)Types.GRASS);
		int grassThresholdSize = 20;

		//remove regions of grass tiles smaller than 20 tiles in area (islands smaller than 20 tiles).
		foreach (List<Coord> grassRegion in grassRegions) {
			if(grassRegion.Count < grassThresholdSize){
				foreach(Coord tile in grassRegion){
					gridPos [tile.tileX, tile.tileY] = (int)Types.WATER;
				}
			}
		}

		//Change all the tiles around water to sand tiles.
		List<Coord> sandTiles = new List<Coord> ();
		foreach (List<Coord> grassregion in grassRegions) {
			foreach (Coord tile in grassregion){
				if (GetAdjacentWaterCount(tile.tileX, tile.tileY) >= 1){
					sandTiles.Add (tile);
				}
			}
		}
		foreach (Coord tile in sandTiles) {
			gridPos [tile.tileX, tile.tileY] = (int)Types.SAND;
			foreach (List<Coord> grassRegion in grassRegions) {
				if (grassRegion.Contains (tile)) {
					grassRegion.Remove (tile);
				}
			}
		}

		//Change a portion of grass tiles into mountain/rocky tiles
		System.Random r = new System.Random();
		List<Coord> mountainTiles = new List<Coord> ();
		foreach (List<Coord> grassRegion in grassRegions) {
			foreach (Coord tile in grassRegion) {
				if(r.Next(0,100) < 20){
					mountainTiles.Add (tile);
				}
			}
		}
		foreach (Coord tile in mountainTiles) {
			gridPos [tile.tileX, tile.tileY] = (int)Types.MOUNTIAN;
			/*foreach (List<Coord> grassRegion in grassRegions) {
				if (grassRegion.Contains (tile)) {
					grassRegion.Remove (tile);
				}
			}*/
		}
		for (int i = 0; i < 5; i++) {
			foreach (List<Coord> grassRegion in grassRegions) {
				foreach (Coord tile in grassRegion) {
					if (GetAdjacentTilesOfType (tile.tileX, tile.tileY, (int)Types.MOUNTIAN) > 4) {
						gridPos [tile.tileX, tile.tileY] = (int)Types.MOUNTIAN;
					}
					if (GetAdjacentTilesOfType (tile.tileX, tile.tileY, (int)Types.MOUNTIAN) < 2) {
						gridPos [tile.tileX, tile.tileY] = (int)Types.GRASS;
					}
				}
			}
		}

		List<List<Coord>> mountainRegions = GetRegions ((int)Types.MOUNTIAN);
		int mountainThresholdSize = 5;

		//remove regions of mountain tiles smaller than 5 tiles in area.
		foreach (List<Coord> mountainRegion in mountainRegions) {
			if(mountainRegion.Count < grassThresholdSize){
				foreach(Coord tile in mountainRegion){
					gridPos [tile.tileX, tile.tileY] = (int)Types.GRASS;
				}
			}
		}
		
	}

	/// <summary>
	/// Gets the regions.
	/// </summary>
	/// <description>
	/// This method creates a List of regions (see GetRegionTiles) of the specified type and returns this list.
	/// </description>
	/// <returns>The regions.</returns>
	/// <param name="tileType">Tile type.</param>
	List<List<Coord>> GetRegions(int tileType){
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] gridFlags = new int[columns, rows];

		for (int x = 0; x < columns; x++){
			for (int y = 0; y < rows; y++){
				if (gridFlags [x, y] == 0 && gridPos [x, y] == tileType) {
					List<Coord> newRegion = GetRegionTiles (x, y);
					regions.Add (newRegion);

					foreach (Coord tile in newRegion) {
						gridFlags [tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}

	/// <summary>
	/// Gets the region tiles.
	/// </summary>
	/// <description>
	/// This method takes two parameters a starting x value and a starting y value.
	/// It creates a List of all the tiles of the same type in a region. (Where a region is a continuous area of tiles of the same type.)
	/// It then returns this list.
	/// </description>
	/// <returns>The region tiles.</returns>
	/// <param name="startX">Start x.</param>
	/// <param name="startY">Start y.</param>
	List<Coord> GetRegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord>(); 
		int[,] gridFlags = new int[columns, rows];
		int tileType = gridPos [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord(startX, startY));
		gridFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);
			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if(IsInRange(x,y) && (y == tile.tileY || x == tile.tileX)){
						if(gridFlags[x,y] == 0 && gridPos[x,y] == tileType){
							gridFlags [x, y] = 1;
							queue.Enqueue (new Coord (x, y));
						}
					}
				}
			}
		}
		return tiles;
	}

	/// <summary>
	/// Determines whether this instance is in range of the specified x y.
	/// </summary>
	/// <returns><c>true</c> if this instance is in range of the specified x y; otherwise, <c>false</c>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	bool IsInRange(int x, int y){
		return x >= 0 && x < columns && y >= 0 && y < rows;
	}

	/// <summary>
	/// Randomises the grid.
	/// </summary>
	/// <description>
	/// This method Randomises all of the values in gridPos to be either 0 (a grass tile) or 1 (a water tile).
	/// This is achieved by iterating over each element in the array and using Random.Next with a range of 0 - 100 and comparing the value with the waterPercentage int.
	/// If the result is less than waterPercentage the type of the tile is set 1, otherwise 0.
	/// </description>
	void RandomiseGrid(){
		if(randomSeed){
			seed = UnityEngine.Random.Range (-1000f, 1000f).ToString ();
		}
		System.Random rnd = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				gridPos [x, y] = (rnd.Next (0, 100) < waterPercentage) ? 1 : 0;
			}
		}
	}

	/// <summary>
	/// Smooth this instance.
	/// </summary>
	/// <description>
	/// This method iterates over all grid positions and changes their value based on the eight surrounding grid positions.
	/// This method calls the GetAdjacentWaterCount method.
	/// If there are more than four water tiles adjacent the tile becomes a water tile.
	/// If there are fewer than four water tiles adjacent the tile becomes a grass tile.
	/// If there are four water tiles adjacent the tile stays as it was.
	/// </description>
	void Smooth(){
		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				int adjacentTiles = GetAdjacentTilesOfType (x, y, 1);
				if (adjacentTiles > 4){
					gridPos [x, y] = 1;
				}else if (adjacentTiles < 4){
					gridPos [x, y] = 0;
				}
			}
		}
	}

	/// <summary>
	/// Gets the adjacent water count.
	/// </summary>
	/// <returns>The adjacent water count.</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posY">Position y.</param>
	/// <description>
	/// This method iterates over the eight positions adjacent to the given grid position and evaluates its type.
	/// As water tiles are of type 1, simply returning the sum of all adjacent tiles will return the number of adjacent water tiles.
	/// 
	/// </description>
	int GetAdjacentWaterCount(int posX, int posY){
		int adjacentTiles = 0;
		for (int adjX = posX -1; adjX <= posX +1; adjX++) {
			for (int adjY = posY -1; adjY <= posY +1; adjY++) {
				if(IsInRange(adjX, adjY)){
					if (adjX != posX || adjY != posY) {
						adjacentTiles += gridPos [adjX, adjY];
					}
				}
			}
		}
		return adjacentTiles;
	}

	/// <summary>
	/// Gets the number of the adjacent tiles of type.
	/// </summary>
	/// <returns>the number of the adjacent tiles of type.</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posY">Position y.</param>
	/// <param name="type">Type.</param>
	int GetAdjacentTilesOfType(int posX, int posY, int type){
		int adjacentTiles = 0;
		for (int adjX = posX - 1; adjX <= posX + 1; adjX++) {
			for (int adjY = posY - 1; adjY <= posY + 1; adjY++) {
				if (IsInRange(adjX, adjY)){
					if(adjX != posX || adjY != posY){
						if(gridPos[adjX, adjY] == type){
							adjacentTiles += 1;
						}
					}
				}
			}
		}
					return adjacentTiles;
	}

	/// <summary>
	/// Sets up map.
	/// </summary>
	/// <description>
	/// This method is responsable for creating the Tile of the type as set in gridPos by the RandomiseGrid method.
	/// </description>
	void SetUpMap(){
		mapHolder = new GameObject ("Map").transform;
		mapHolder.tag = "Map";

		if (gridPos != null) {
			for(int x = 0; x < columns; x++){
				for (int y = 0; y < rows; y++){
					GameObject tile = new GameObject ("Tile");
					tile.transform.SetParent (GameObject.Find("PlaneManager").transform);
					tile.AddComponent<Tile>();
					Tile script = (Tile) tile.GetComponent ("Tile");
					script.NewTile (gridPos [x, y]);
					GameObject instance = Instantiate (tile, new Vector3 (x, y, 0.0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (mapHolder);
				}
			}
		}

		Component[] actualTiles = mapHolder.GetComponentsInChildren<Tile> ();
		foreach (Component tile in actualTiles) {
			tiles.Add ((Tile)tile);
		}

		GameObject planeManager = GameObject.FindGameObjectWithTag ("PlaneManager");
		Component[] oldTiles = planeManager.GetComponentsInChildren<Tile> ();
		foreach(Tile t in oldTiles){
			Destroy (t.gameObject);
		}
	}

	/// <summary>
	/// Coordinate.
	/// </summary>
	/// <description>
	/// This struct defines a type called Coord that is a pair of integer coordinates representing one of the tiles in the map.
	/// </description>
	/// <see cref="https://msdn.microsoft.com/en-us/library/aa288471(v=vs.71).aspx"/>
	struct Coord{
		public int tileX;
		public int tileY;

		public Coord (int x, int y){
			tileX = x;
			tileY = y;
		}
	}
}
