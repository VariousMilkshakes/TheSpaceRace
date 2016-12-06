using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;
using System.Linq;


namespace SpaceRace.World.Buildings.Collection
{
	/// <summary>
	/// Town hall. Responsible for setting and expanding the city boundary.
	/// </summary>
	public class TownHall : Building
	{
		private MapGenerator mapGen;
		private Player currentPlayer;
		private List<Tile> cityTiles;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceRace.World.Buildings.Collection.TownHall"/> class.
		/// </summary>
		public TownHall () : base (typeof(TownHall))
		{
			mapGen = GameObject.FindGameObjectWithTag("PlaneManager")
				.GetComponent<MapGenerator>();
			Sprite sprite = null;
			cityTiles = new List<Tile>();

			setCityTiles ();			
			/// Set sprite for building
			try
			{
				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
				string spritePath = buildingConfigs.LookForProperty("TownHall", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
			_buildingSprites.Add(WorldStates.All, sprite);

		}
		/// <summary>
		/// Resources required for a player to build a town hall
		/// </summary>
		/// <returns>Required resources</returns>
		public override SpaceRace.PlayerTools.ResourceBox BuildRequirements()
		{
			return new SpaceRace.PlayerTools.ResourceBox(PlayerTools.Resources.Money, 10);
		}

		public override void OnTurn()
		{
			base.OnTurn ();
			setCityTiles ();
			///Trigger city expansion if population has increased by 5
//**fix**  	int population = PlayerTools.Inventory.CheckResource (PlayerTools.Resources.Population);
		//	int population = 10; /*test*/
		/*	if (population % 5 == 0) {
				expandCityBoundary ();
			}*/
		}
			
		/// <summary>
		/// Finds the position of this player's Town Hall
		/// </summary>
		/// <returns>The town hall.</returns>
		private Tile findTownHall(){
	/*		List<Tile> toSearch = mapGen.getTiles();
			Tile toReturn = null;
			for(int i = 0; i<toSearch.Count; i++){
				if(toSearch[i] != null && toSearch[i].Building.Equals("TownHall") && toSearch[i].getTileColour().Equals(currentPlayer.Color)){
					toReturn = toSearch [i];
				}
			}

			return toReturn;*/
			return null;
		}
			
		/// <summary>
		/// Sets the city tiles by changing these to this player's colour.
		/// </summary>
		private void setCityTiles(){
			Tile townHallPos = findTownHall ();
			if (townHallPos == null) {
				return;
			}
		/*	cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY()));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY() + 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY() + 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY()));
			cityTiles.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY() + 1));
			foreach(Tile tile in cityTiles){
				tile.ApplyPlayerColor (player.Color);
			}*/
			Tile tile = mapGen.GetTile (1, 1);
			cityTiles.Add (tile);
			tile.ApplyPlayerColor (currentPlayer.Color);
			setUnselectable ();

		}

		/// <summary>
		/// Expands the city boundary by randomly selecting one of the tiles bordering the city boundary 
		/// and setting this to be long to the player
		/// </summary>

		//change to expand to one OUTSIDE the boundary, currently selects tile the player already owns
		private void expandCityBoundary(){
			System.Random rnd = new System.Random ();
//**fix
			//find border tiles
			List<Tile> borderTiles = null;
			int maxXCoord = findMaxX (cityTiles);
			int maxYCoord = findMaxY (cityTiles);
			int minXCoord = findMinX (cityTiles);
			int minYCoord = findMinY (cityTiles);
			foreach (Tile tile in cityTiles) {
				//add all tiles with these values to expandX/expandY
				if (tile.GetX () == maxXCoord || tile.GetX () == minXCoord || tile.GetY () == maxYCoord || tile.GetY () == minYCoord) {
					borderTiles.Add (tile);
				}
			}

			//choose random tile from 'borderTiles' to expand to
			int expandToIndex = rnd.Next (borderTiles.Count);
			Tile expandTo = borderTiles.ElementAt (expandToIndex);
			expandTo.ApplyPlayerColor (currentPlayer.Color);
			Tile expandToMap = mapGen.GetTile (expandTo.GetX (), expandTo.GetY ());	
		}
			
		/// <summary>
		/// Finds the maximum x coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The maximum x coordinate int</returns>
		/// <param name="list">List.</param>
		private int findMaxX(List<Tile> list){ 
			if (list.Count == 0){
				throw new InvalidOperationException("Error: empty list");
			}
			int max = int.MinValue;
			foreach (Tile tile in list){
				if (tile.GetX() > max){
					max = tile.GetX();
				}
			}
			return max;
		}

		/// <summary>
		/// Finds the maximum y coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The maximum y coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMaxY(List<Tile> list){ 
			if (list.Count == 0){
				throw new InvalidOperationException("Error: empty list");
			}
			int max = int.MinValue;
			foreach (Tile tile in list){
				if (tile.GetY() > max){
					max = tile.GetY();
				}
			}
			return max;
		}

		/// <summary>
		/// Finds the minimum x coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The minimum x coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMinX(List<Tile> list){
			if (list.Count == 0){
				throw new InvalidOperationException("Error: empty list");
			}
			int min = int.MaxValue;
			foreach (Tile tile in list){
				if (tile.GetX() < min){
					min = tile.GetX();
				}
			}
			return min;
		}

		/// <summary>
		/// Finds the minimum y coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The minimum y coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMinY(List<Tile> list){
			if (list.Count == 0){
				throw new InvalidOperationException("Error: empty list");
			}
			int min = int.MaxValue;
			foreach (Tile tile in list){
				if (tile.GetY() < min){
					min = tile.GetY();
				}
			}
			return min;
		}
			
		/// <summary>
		/// Sets tiles outside of the city boundary to be unselectable.
		/// </summary>
		private void setUnselectable(){
			foreach (Tile tile in mapGen.tiles) {
				if (!cityTiles.Contains (tile)) {
					tile.enabled = false;
				}
			}
		}


	}
}

				

