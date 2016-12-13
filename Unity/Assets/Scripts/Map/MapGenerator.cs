using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SpaceRace.PlayerTools;

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
	/// The resource sprites.
	/// </summary>
	public Sprite[] resourceSprites;

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

	private System.Random rnd = new System.Random();

	/// <summary>
	/// The tiles.
	/// </summary>
	[HideInInspector]
	public List<Tile> tiles;

	/// <summary>
	/// The two dimensional integer array of grid positions used to refer to each position on the grid.
	/// </summary>
	int[,] gridPos;

	public enum TileTypes
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

	public Sprite GetResourceSprite(int index){
		return resourceSprites [index];
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

	public List<Tile> GetTilesWithResource(SpaceRace.PlayerTools.Resources resource){
		List<Tile> result = new List<Tile> ();
		foreach (Tile tile in tiles) {
			if (tile.GetResource().Equals(resource)) {
				result.Add (tile);
			}
		}
		return result;
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
		SetUpResources ();
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
	/*
		I plan on changing this to make modular 'zones' so that a map contains start zones for players that will have an ensured amount of randomly placed resources 
		to make it fair for each player and so one player cannot monopolise the market of a basic resource. This would limit the player to place a townHall inside the
		zone so that they are ensured to have a certain number of starting resources surronding them. after MVP.
	*/
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
	/// This method removes any regions deemed to be too small as well as creating coasts of sand around water regions and mountains within grass regions.
	/// </description>
	void ProcessGrid(){
		int grassThreshold = 20;
		int waterThreshold = 20;
		int mountainThreshold = 5;

		//remove regions of water tiles smaller than 20 tiles in area.
		RemoveRegionsOfSize ((int)TileTypes.WATER, (int)TileTypes.GRASS, grassThreshold);

		//remove regions of grass tiles smaller than 20 tiles in area (islands smaller than 20 tiles).
		RemoveRegionsOfSize ((int)TileTypes.GRASS, (int)TileTypes.WATER, waterThreshold);

		CreateCoast ();

		CreateMountains ();

		RemoveRegionsOfSize ((int)TileTypes.MOUNTIAN, (int)TileTypes.GRASS, mountainThreshold);

		int mountainsTooLargeThreshold = 50;
		List<List<Coord>> regions = GetRegions ((int)TileTypes.MOUNTIAN);
		foreach (List<Coord> region in regions) {
			if (region.Count > mountainsTooLargeThreshold) {
				foreach (Coord tile in region) {
					gridPos [tile.tileX, tile.tileY] = (int)TileTypes.GRASS;
				}
			}
		}
	}

	/// <summary>
	/// Removes the regions (of type) of the given size.
	/// </summary>
	/// <description>
	/// Removes the region of the specified type of tile if it contains fewer than the given threshold tiles by replacing them with the given type of replacement type of tile.
	/// </description>
	/// <param name="type">Type.</param>
	/// <param name="replaceType">Replace type.</param>
	/// <param name="threshold">Threshold.</param>
	void RemoveRegionsOfSize(int type, int replaceType, int threshold){
		List<List<Coord>> regions = GetRegions (type);
		foreach (List<Coord> region in regions) {
			if (region.Count < threshold) {
				foreach (Coord tile in region) {
					gridPos [tile.tileX, tile.tileY] = replaceType;
				}
			}
		}
	}

	/// <summary>
	/// Creates the coast.
	/// </summary>
	void CreateCoast(){
		//Change all the tiles around water to sand tiles.
		List<List<Coord>> grassRegions = GetRegions((int) TileTypes.GRASS);

		List<Coord> sandTiles = new List<Coord> ();
		foreach (List<Coord> grassregion in grassRegions) {
			foreach (Coord tile in grassregion){
				if (GetAdjacentTilesOfType(tile.tileX, tile.tileY, (int)TileTypes.WATER) >= 1){
					sandTiles.Add (tile);
					gridPos [tile.tileX, tile.tileY] = (int)TileTypes.SAND;
				}
			}
		}
	}

	/// <summary>
	/// Creates the mountains.
	/// </summary>
	void CreateMountains(){
		List<List<Coord>> grassRegions = GetRegions((int) TileTypes.GRASS);

		//Change a portion of grass tiles into mountain/rocky tiles

		List<Coord> mountainTiles = new List<Coord> ();
		foreach (List<Coord> grassRegion in grassRegions) {
			foreach (Coord tile in grassRegion) {
				if(rnd.Next(0,100) < 20){
					mountainTiles.Add (tile);
				}
			}
		}
		foreach (Coord tile in mountainTiles) {
			gridPos [tile.tileX, tile.tileY] = (int)TileTypes.MOUNTIAN;
		}
		for (int i = 0; i < 5; i++) {
			foreach (List<Coord> grassRegion in grassRegions) {
				foreach (Coord tile in grassRegion) {
					if (GetAdjacentTilesOfType (tile.tileX, tile.tileY, (int)TileTypes.MOUNTIAN) > 3) {
						gridPos [tile.tileX, tile.tileY] = (int)TileTypes.MOUNTIAN;
					}
					if (GetAdjacentTilesOfType (tile.tileX, tile.tileY, (int)TileTypes.MOUNTIAN) < 2) {
						gridPos [tile.tileX, tile.tileY] = (int)TileTypes.GRASS;
					}
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
	/// Sets up resources.
	/// </summary>
	void SetUpResources(){
		SetUpResource (SpaceRace.PlayerTools.Resources.Wood, (int)TileTypes.GRASS, 20);
		SetUpResource (SpaceRace.PlayerTools.Resources.Stone, (int)TileTypes.MOUNTIAN, 5);
		SetUpResource (SpaceRace.PlayerTools.Resources.Iron, (int)TileTypes.MOUNTIAN, 5);

		//Sets any remaining grass Tiles to have straw on them for farms to gather (and convert to food?)
		List<List<Coord>> grassRegions = GetRegions ((int)TileTypes.GRASS);
		foreach (List<Coord> region in grassRegions) {
			foreach (Coord tile in region) {
				Tile t = GetTile (tile.tileX, tile.tileY);
				if (t.GetResource () == SpaceRace.PlayerTools.Resources.None) {
					t.addResource (SpaceRace.PlayerTools.Resources.Straw);
				}
			}
		}
	}

	/// <summary>
	/// Sets up a resource.
	/// </summary>
	/// <param name="resource">Resource.</param>
	/// <param name="baseTileType">Base tile type.</param>
	/// <param name="minimum">Minimum.</param>
	void SetUpResource(SpaceRace.PlayerTools.Resources resource, int baseTileType, int minimum){
		List<List<Coord>> grassRegions = GetRegions (baseTileType);

		foreach (List<Coord> grassRegion in grassRegions) {
			foreach (Coord tile in grassRegion) {
				if(rnd.Next(0, 100) < minimum){
					GetTile(tile.tileX, tile.tileY).addResource(resource);
				}
			}
		}
		EnsureResourceExists (resource, minimum, baseTileType);
	}

	/// <summary>
	/// Ensures at least the minimum amount of the resource exists.
	/// </summary>
	/// <param name="resource">Resource.</param>
	/// <param name="amount">Amount.</param>
	/// <param name="baseTileType">Base tile type.</param>
	void EnsureResourceExists(SpaceRace.PlayerTools.Resources resource, int amount, int baseTileType){

		if (GetTilesWithResource (resource).Count < amount) {
			List<List<Coord>> regions = GetRegions (baseTileType);
			foreach (List<Coord> region in regions) {
				for (int i = amount/regions.Count; i > 0; i--) {
					int rndIndex = rnd.Next (0, region.Count);
					GetTile (region [rndIndex].tileX, region [rndIndex].tileY).addResource (resource);
					region.Remove (region [rndIndex]);
					region.TrimExcess ();
				}
			}
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
