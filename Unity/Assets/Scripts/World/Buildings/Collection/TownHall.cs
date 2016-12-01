﻿using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;

namespace SpaceRace.World.Buildings.Collection
{
	public class TownHall : Building
	{
		MapGenerator mapGen;
		Player player;
		List<Tile> cityTiles;


		public TownHall () : base (typeof(TownHall))
		{
			Sprite sprite = null;
			player = new Player ();
			cityTiles = null;
			setCityTiles ();
			mapGen = GameObject.FindGameObjectWithTag("PlaneManager")
				.GetComponent<MapGenerator>();
			
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

			//Input = new PT.ResourceBox(PT.Resources.Free, 0, 0);
			//Output = new PT.ResourceBox(PT.Resources.Money, 0, 10);
		}

		public override SpaceRace.PlayerTools.ResourceBox BuildRequirements()
		{
			return new SpaceRace.PlayerTools.ResourceBox(SpaceRace.PlayerTools.Resources.None);
		}

		public override void OnTurn()
		{
			base.OnTurn();
		}

		/*find position of this player's town hall*/
		private Tile findTownHall(){
			List<Tile> toSearch = mapGen.tiles;
			Tile toReturn = null;
			for(int i = 0; i<toSearch.Count; i++){
				if(toSearch[i].Building.Equals("TownHall") && toSearch[i].getTileColour().Equals(player.Color)){
					toReturn = toSearch [i];
				}
			}
			return toReturn;
		}

		/*set tiles surrounding town hall to the player's colour*/
		private void setCityTiles(){
			Tile townHallPos = findTownHall ();
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY()));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () - 1, townHallPos.GetY() + 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX (), townHallPos.GetY() + 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY() - 1));
			cityTiles.Add(mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY()));
			cityTiles.Add (mapGen.GetTile (townHallPos.GetX () + 1, townHallPos.GetY() + 1));
			foreach(Tile tile in cityTiles){
				tile.ApplyPlayerColor (player.Color);
			}
		}

		private void expandCityBoundary(){
			System.Random rnd = new System.Random ();
			/*TODO: make list of tiles surrounding the city area*/

			//lists of x coordinates and a list of y coordinates of border tiles
			List<int> expandX = null;
			List<int> expandY = null;

			//find border tiles
			List<Tile> borderTiles = null;

			int maxXCoord = findMaxXY (cityTiles, "tile.GetX()");
			int maxYCoord = findMaxXY (cityTiles, "tile.GetY()");
			int minXCoord = findMinXY (cityTiles, "tile.GetX()");
			int minYCoord = findMinXY (cityTiles, "tile.GetX()");
			foreach (Tile tile in cityTiles) {
				//add all tiles with these values to expandX/expandY
				if (tile.GetX () == maxXCoord || tile.GetX () == minXCoord || tile.GetY () == maxYCoord || tile.GetY () == minYCoord) {
					cityTiles.Add (tile);
				}
			}

			int expandToX = rnd.Next (expandX.Count);
			int expandToY = rnd.Next (expandY.Count);

			Tile expandTo = mapGen.GetTile(expandToX, expandToY);
			expandTo.ApplyPlayerColor (player.Color);
		}

		//Find maximum X/Y coordinate of a tile
		private int findMaxXY(List<Tile> list, String XYCoord){ /*change to get rid of findMaxY*/
			if (list.Count == 0)
			{
				throw new InvalidOperationException("Error: empty list");
			}
			int max = int.MinValue;
			foreach (Tile tile in list)
			{
				if (tile.GetX() /*XYCoord*/ > max)
				{
					max = tile.GetX();
				}
			}
			return max;
		}

		//find minimum X/Y coordinate of a tile
		private int findMinXY(List<Tile> list, String XYCoord){
			if (list.Count == 0)
			{
				throw new InvalidOperationException("Error: empty list");
			}
			int min = int.MaxValue;
			foreach (Tile tile in list)
			{
				if (tile.GetY() /*XYCoord*/ < min)
				{
					min = tile.GetY();
				}
			}
			return min;
		}


		/*TODO: make tiles outside of city boundary unselectable*/

	}
}

				

