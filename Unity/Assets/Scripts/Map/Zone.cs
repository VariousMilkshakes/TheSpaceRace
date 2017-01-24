using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone{

	private int size;
	private int[,] gridPos;
	private float seed;
	private System.Random rnd;
	private int waterPercentage;

	public Zone(int size, float seed, int waterPercentage){
		this.size = size;
		this.seed = seed;
		this.waterPercentage = waterPercentage;
		gridPos = new int[size, size];
		rnd = new System.Random (seed.GetHashCode ());
		GenerateZone ();
	}

	public int[,] getZone(){
		return gridPos;
	}

	/// <summary>
	/// Generates the zone.
	/// </summary>
	private void GenerateZone(){
		RandomiseZone (waterPercentage);
		Smooth ();
		ProcessGrid ();
	}

	/// <summary>
	/// Randomises the zone.
	/// </summary>
	/// <description>
	/// This method Randomises all of the values in gridPos to be either 0 (a grass tile) or 1 (a water tile).
	/// This is achieved by iterating over each element in the array and using Random.Next with a range of 0 - 100 and comparing the value with the waterPercentage int.
	/// If the result is less than waterPercentage the type of the tile is set 1, otherwise 0.
	/// </description>
	private void RandomiseZone(int waterPercentage){
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				gridPos [x, y] = (rnd.Next (0, 100) < waterPercentage) ? (int)TileTypes.GRASS : (int)TileTypes.WATER;
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
	private void Smooth(){
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				int adjacentTiles = GetAdjacentTilesOfType (x, y, (int)TileTypes.GRASS);
				if (adjacentTiles > 4){
					gridPos [x, y] = 1;
				}else if (adjacentTiles < 4){
					gridPos [x, y] = 0;
				}
			}
		}
	}

	/// <summary>
	/// Gets the number of the adjacent tiles of type.
	/// </summary>
	/// <returns>the number of the adjacent tiles of type.</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posY">Position y.</param>
	/// <param name="type">Type.</param>
	private int GetAdjacentTilesOfType(int posX, int posY, int type){
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

	private bool IsInRange(int x, int y){
		return x >= 0 && x < size && y >= 0 && y < size;
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

		//remove regions of mountains smaller than 5 tiles in area.
		RemoveRegionsOfSize ((int)TileTypes.MOUNTIAN, (int)TileTypes.GRASS, mountainThreshold);

		//remove regions of mountains larger than 50 tiles in area.
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
	/// Gets the regions.
	/// </summary>
	/// <description>
	/// This method creates a List of regions (see GetRegionTiles) of the specified type and returns this list.
	/// </description>
	/// <returns>The regions.</returns>
	/// <param name="tileType">Tile type.</param>
	List<List<Coord>> GetRegions(int tileType){
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] gridFlags = new int[size, size];

		for (int x = 0; x < size; x++){
			for (int y = 0; y < size; y++){
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
		int[,] gridFlags = new int[size, size];
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
	/// Creates the coast.
	/// </summary>
	void CreateCoast(){
		//Change all the tiles around water to sand tiles.
		List<List<Coord>> grassRegions = GetRegions((int) TileTypes.GRASS);

		foreach (List<Coord> grassregion in grassRegions) {
			foreach (Coord tile in grassregion){
				if (GetAdjacentTilesOfType(tile.tileX, tile.tileY, (int)TileTypes.WATER) >= 1){
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
		//smooth mountains
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
