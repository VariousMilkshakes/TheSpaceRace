﻿using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;

using System.Linq;

//TODO:
//once everything works, refactor so that Player.Properties looks after tiles that the player owns
//set max/min x/y when initial city tiles are set. Only change the max/min when border lists are all owned

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
		private List<Tile> borderTiles;
		int tilesOwned;
		String playerName;
		UnityEngine.Color playerColour;
		int turn;/*delete me*/

		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceRace.World.Buildings.Collection.TownHall"/> class.
		/// </summary>
		public TownHall (Player builder) : base (typeof(TownHall), builder)
		{
			Sprite sprite = null;
			mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent<MapGenerator> ();
			currentPlayer = GameObject.Find ("GameManager").GetComponent<Game> ().GetActivePlayer();
			playerName = GameObject.Find ("GameManager").GetComponent<Game> ().GetActivePlayerName ();
			playerColour = currentPlayer.Color;

			cityTiles = /*PlayerTools.Player.Properties.GetPlayerTiles ();*/ new List<Tile> ();
			mapTiles = mapGen.getTiles ();
			turn = 1;/*delete me*/

			/// Each border of tiles surrounding the city boundary 
			topBorder = new List<Tile> ();
			bottomBorder = new List<Tile> ();
			rightBorder = new List<Tile> ();
			leftBorder = new List<Tile> ();
			/// Total tiles surrounding the city boundary, consists of topBorder, bottomBorder, rightBorder and leftBorder
			borderTiles = new List<Tile> ();

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

		public override ResourceBox OnBuild ()
		{
			setCityTiles ();
			Tile townHallPos = findTownHall ();
			maxXCoord = townHallPos.GetX () + 1;
			maxYCoord = townHallPos.GetY () + 1;
			minXCoord = townHallPos.GetX () - 1;
			minYCoord = townHallPos.GetY () - 1;
			return base.OnBuild ();
		}

		public override void OnTurn ()
		{

			turn = 1;
			base.OnTurn ();
			/// Initially, only tiles owned by the current user are those directly surrounding the town hall

				/// After first turn, the maximum X and Y coordinates are dependent on the current city tiles
				setCityTiles ();
				maxXCoord = findMaxX (cityTiles);
				maxYCoord = findMaxY (cityTiles);
				minXCoord = findMinX (cityTiles);
				minYCoord = findMinY (cityTiles);


			///Trigger city expansion if population has increased by 2
			//	use me int population = currentPlayer.Inventory.CheckResource (SpaceRace.PlayerTools.Resources.Population);
			int population = 10; /*test, delete me*/
			if (population % 2 == 0) {
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
				Tile target = (Tile)mapTiles [i];
				if (target != null && target.Building != null && target.GetOwner() != null
					&& target.Building.GetType ().Name.Equals ("TownHall")
					&& target.GetOwner ().Equals(playerName)) {	
					toReturn = target;
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
			List<Tile> surroundingTH = new List<Tile> ();
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY () - 1));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY ()));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY () + 1));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY () - 1));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY () + 1));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY () - 1));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY ()));
			surroundingTH.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY () + 1));
			surroundingTH.Add(mapGen.GetTile(townHallPos.GetX(), townHallPos.GetY()));
			foreach (Tile tile in surroundingTH) {
				tile.SetOwner (playerName, playerColour);
				cityTiles.Add (tile);
			}
		}

		/// <summary>
		/// Calculates the number of tiles on the map owned by the current player
		/// </summary>
		/// <returns>The number of tiles owned.</returns>
		private int setTilesOwned ()
		{
			tilesOwned = 0;
			foreach (Tile tile in cityTiles) {
				if (tile.GetOwner ()!= null) {
					if (tile.GetOwner ().Equals(playerName)) {
						tilesOwned++;
					}
				}
			}
			Debug.Log ("Tiles owned: " + tilesOwned + ", " + "borderTiles.Count(): " + borderTiles.Count ());
			return tilesOwned;
		}

		/// <summary>
		/// Expands the city boundary by randomly selecting one of the tiles bordering the city boundary 
		/// and setting this to be long to the player
		/// </summary>

		private void expandCityBoundary ()
		{
			System.Random rnd = new System.Random ();
			setTilesOwned ();

			int i = 0;
			if (tilesOwned >= borderTiles.Count () && i == 0) {
				foreach (Tile tile in mapTiles) {
					if (tile.GetOwner () == null) {
						//add all tiles surrounding the current city limit to borderTiles
						if (tile.GetY () == maxYCoord + 1 && tile.GetX () >= minXCoord && tile.GetX () - 1 <= maxXCoord + 1) {	
							topBorder.Add (tile);
						} else {
							if (tile.GetY () == minYCoord - 1 && tile.GetX () >= minXCoord - 1 && tile.GetX () <= maxXCoord + 1) {
								bottomBorder.Add (tile);

							} else {
								if (tile.GetX () == maxXCoord + 1 && tile.GetY () > minYCoord - 1 && tile.GetY () < maxYCoord + 1) {
									rightBorder.Add (tile);

								} else {
									if (tile.GetX () == minXCoord - 1 && tile.GetY () > minYCoord - 1 && tile.GetY () < maxYCoord + 1) {
										leftBorder.Add (tile);

									}
								}
							}
						}
					}
				}
				//Add tiles in each border to borderTiles
				borderTiles.AddRange (topBorder);
				borderTiles.AddRange (bottomBorder);
				borderTiles.AddRange (leftBorder);
				borderTiles.AddRange (rightBorder);
				i++;
			}


			int borderIndex = borderTiles.Count ();
			int expandToIndex = rnd.Next (borderIndex);
			Tile expandTo = borderTiles.ElementAt (expandToIndex);
			expandTo.SetOwner (playerName, playerColour);
			cityTiles.Add (expandTo);
			tilesOwned++;
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