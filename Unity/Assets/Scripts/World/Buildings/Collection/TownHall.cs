using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;
using System.Linq;

//TODO:
//once everything works, refactor so that Player.Properties looks after tiles that the player owns
//set max/min x/y when initial city tiles are set. Only change the max/min when border lists are all owned
//how are turns handled?

namespace SpaceRace.World.Buildings.Collection
{
	/// <summary>
	/// Town hall. Responsible for setting and expanding the city boundary.
	/// </summary>
	public class TownHall : Building
	{
		private MapGenerator mapGen;
		private List<Tile> cityTiles;
		private Player currentPlayer;
		private List<Tile> mapTiles;
		private int maxXCoord;
		private int maxYCoord;
		private int minXCoord;
		private int minYCoord;
		private List<Tile> topBorder;
		private List<Tile> bottomBorder;
		private List<Tile> rightBorder;
		private List<Tile> leftBorder;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceRace.World.Buildings.Collection.TownHall"/> class.
		/// </summary>
		public TownHall () : base (typeof(TownHall))
		{
			mapGen = GameObject.FindGameObjectWithTag ("PlaneManager")
				.GetComponent<MapGenerator> ();
			Sprite sprite = null;
			cityTiles = /*PlayerTools.Player.Properties.GetPlayerTiles ();*/ new List<Tile> ();
			currentPlayer = GameObject.Find ("GameManager").GetComponent<Game> ().GetActivePlayer ();
			mapTiles = mapGen.getTiles ();


			/// Set sprite for building
			try {
				Config buildingConfigs = GameRules.CONFIG_REPO ["Buildings"];
				string spritePath = buildingConfigs.LookForProperty ("TownHall", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load (spritePath, typeof(Sprite)) as Sprite;
			} catch (Exception e) {
				Debug.Log (e);
			}
			_buildingSprites.Add (WorldStates.All, sprite);

		}

		/// <summary>
		/// Resources required for a player to build a town hall
		/// </summary>
		/// <returns>Required resources</returns>
		public override SpaceRace.PlayerTools.ResourceBox BuildRequirements ()
		{
			return new SpaceRace.PlayerTools.ResourceBox (PlayerTools.Resources.Money, 0);
		}

		public override void OnTurn ()
		{
			int turn = 0;
			base.OnTurn ();
			if (turn != 1) {
				setCityTiles ();
				maxXCoord = findMaxX (cityTiles);
				maxYCoord = findMaxY (cityTiles);
				minXCoord = findMinX (cityTiles);
				minYCoord = findMinY (cityTiles);
				topBorder = new List<Tile> ();
				bottomBorder = new List<Tile> ();
				rightBorder = new List<Tile> ();
				leftBorder = new List<Tile> ();
			}
			///Trigger city expansion if population has increased by 5
			//	int population = currentPlayer.Inventory.CheckResource (SpaceRace.PlayerTools.Resources.Population);
			int population = 10; /*test*/
			if (population % 5 == 0) {
				expandCityBoundary ();
			}
			turn++;
		}

		/// <summary>
		/// Finds the position of this player's Town Hall
		/// </summary>
		/// <returns>The town hall.</returns>
		private Tile findTownHall ()
		{
			Tile toReturn = null;
			for (int i = 0; i < mapTiles.Count; i++) {
				if (mapTiles.ElementAt (i) != null && mapTiles.ElementAt (i).Building != null
				    && mapTiles.ElementAt (i).Building.GetType ().Name.Equals ("TownHall")) {	
					toReturn = mapTiles [i];
				}
			}
			return toReturn;

		}

		/// <summary>
		/// Sets the city tiles by changing these to this player's colour.
		/// </summary>
		private void setCityTiles ()
		{
			Tile townHallPos = findTownHall ();
			List<Tile> surrounding = new List<Tile> ();
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY () - 1));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY ()));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY () + 1));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY () - 1));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY () + 1));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY () - 1));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY ()));
			surrounding.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY () + 1));

			foreach (Tile tile in surrounding) {
				tile.ApplyPlayerColor (currentPlayer.Color);
				cityTiles.Add (tile);
			}
		}


		/// <summary>
		/// Expands the city boundary by randomly selecting one of the tiles bordering the city boundary 
		/// and setting this to be long to the player
		/// </summary>

		private void expandCityBoundary ()
		{
			System.Random rnd = new System.Random ();
			foreach (Tile tile in mapTiles) {
				//add all tiles surrounding the current city limit to borderTiles
				if (tile.GetY() == maxYCoord+1 && tile.GetX () >= minXCoord-1 && tile.GetX () <= maxXCoord+1) {	
					topBorder.Add (tile);
					tile.IsOwned = true;
				} else {
					if (tile.GetY() == minYCoord-1 && tile.GetX () >= minXCoord-1 && tile.GetX () <= maxXCoord+1) {
						bottomBorder.Add (tile);
						tile.IsOwned = true;
					} else {
						if (tile.GetX () == maxXCoord+1 && tile.GetY () > minYCoord && tile.GetY () < maxYCoord) {
							rightBorder.Add (tile);
							tile.IsOwned = true;
						} else {
							if (tile.GetX () == minXCoord-1 && tile.GetY () > minYCoord && tile.GetY () < maxYCoord) {
								leftBorder.Add (tile);
								tile.IsOwned = true;
							}
						}
					}
				}
			}

			//choose random tile from 'borderTiles' to expand to
			List<Tile> borderTiles = new List<Tile> ();
			borderTiles.AddRange (topBorder);
			borderTiles.AddRange (bottomBorder);
			borderTiles.AddRange (leftBorder);
			borderTiles.AddRange (rightBorder);

			int borderIndex = borderTiles.Count ();
			int expandToIndex = rnd.Next (borderIndex);
			Tile expandTo = borderTiles.ElementAt (expandToIndex);

			expandTo.ApplyPlayerColor (currentPlayer.Color);
			cityTiles.Add (expandTo);	
		}

		/// <summary>
		/// Check if all tiles in the specified list are owned
		/// </summary>
		/// <returns><c>true</c>, if border is owned, <c>false</c> otherwise.</returns>
		/// <param name="borderList">Border list, either topBorder, bottomBorder, leftBorder or rightBorder.</param>
		/// <see cref="expandCityBoundary()"/>
		private bool isBorderOwned(List<Tile> borderList){
			int ownedCount = 0;
			foreach (Tile tile in borderList) {
				if (tile.IsOwned) {
					ownedCount++;
				}
			}
			if (ownedCount == borderList.Count ()) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Finds the maximum x coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The maximum x coordinate int</returns>
		/// <param name="list">List.</param>
		private int findMaxX (List<Tile> list)
		{ 
			if (list.Count == 0) {
				throw new InvalidOperationException ("Error: empty list");
			}
			int max = int.MinValue;
			foreach (Tile tile in list) {
				if (tile.GetX () > max) {
					max = tile.GetX ();
				}
			}
			return max;
		}

		/// <summary>
		/// Finds the maximum y coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The maximum y coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMaxY (List<Tile> list)
		{ 
			if (list.Count == 0) {
				throw new InvalidOperationException ("Error: empty list");
			}
			int max = int.MinValue;
			foreach (Tile tile in list) {
				if (tile.GetY () > max) {
					max = tile.GetY ();
				}
			}
			return max;
		}

		/// <summary>
		/// Finds the minimum x coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The minimum x coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMinX (List<Tile> list)
		{
			if (list.Count == 0) {
				throw new InvalidOperationException ("Error: empty list");
			}
			int min = int.MaxValue;
			foreach (Tile tile in list) {
				if (tile.GetX () < min) {
					min = tile.GetX ();
				}
			}
			return min;
		}

		/// <summary>
		/// Finds the minimum y coordinate of a tile inside the city boundary.
		/// </summary>
		/// <returns>The minimum y coordinate int.</returns>
		/// <param name="list">List.</param>
		private int findMinY (List<Tile> list)
		{
			if (list.Count == 0) {
				throw new InvalidOperationException ("Error: empty list");
			}
			int min = int.MaxValue;
			foreach (Tile tile in list) {
				if (tile.GetY () < min) {
					min = tile.GetY ();
				}
			}
			return min;
		}

	}
}

				

