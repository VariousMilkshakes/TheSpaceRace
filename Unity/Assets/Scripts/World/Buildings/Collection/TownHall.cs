using System;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using SpaceRace.Game;
using System.Collections.Generic;
using System.Linq;


namespace SpaceRace.World.Buildings.Collection
{
	/// <summary>
	/// Town hall. Responsible for setting and expanding the city boundary based on the city's population.
	/// </summary>
	public class TownHall : Building

	{
		private MapGenerator mapGen;
		private List<Tile> cityTiles;
		private Player currentPlayer;
        //All tiles on the map
		private List<Tile> mapTiles;
        //max X coordinate of the city on the map
		private int maxXCoord;
        //max Y coodinate of the city on the map
		private int maxYCoord;
        //min X coordinate of the city on the map
		private int minXCoord;
        //min Y coordinate of the city on the map
		private int minYCoord;
        //Border lists determine the outer border of tiles
		private List<Tile> topBorder;
		private List<Tile> bottomBorder;
		private List<Tile> rightBorder;
		private List<Tile> leftBorder;
        //The complete outer border of tiles which the city can expand to
		private List<Tile> borderTiles;
    //    int populationTarget;
		String playerName;
		UnityEngine.Color playerColour;
		public const string BUILDING_NAME = "TownHall";

		/// <summary>
		/// Load sprites as singletons associated with worldstates
		/// Means that a sprite is only loaded once
		/// </summary>
		private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();


		/// <summary>
		/// Initializes a new instance of the <see cref="SpaceRace.World.Buildings.Collection.TownHall"/> class.
		/// </summary>
		public TownHall (Player builder, Tile pos)
			: base (typeof(TownHall), builder, pos, loaded_sprites)
		{
			mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent<MapGenerator> ();
			playerName = _owner.Name;
			playerColour = _owner.Color;

			cityTiles = /*PlayerTools.Player.Properties.GetPlayerTiles ();*/ new List<Tile> ();
			mapTiles = mapGen.GetTiles ();

     //       populationTarget = 2;

			/// Each border of tiles surrounding the city boundary 
			topBorder = new List<Tile> ();
			bottomBorder = new List<Tile> ();
			rightBorder = new List<Tile> ();
			leftBorder = new List<Tile> ();
			/// Total tiles surrounding the city boundary, consists of topBorder, bottomBorder, rightBorder and leftBorder
			borderTiles = new List<Tile> ();

		}

		/// <summary>
		/// Gets the active sprite.
		/// </summary>
		/// <returns>The active sprite.</returns>
		public override Sprite GetActiveSprite(){
			return loaded_sprites [_buildingState];
		}

        /// <summary>
        /// The resources required to construct the building
        /// </summary>
        /// <returns>ResourceBox required for this building</returns>
        public override ResourceBox BuildRequirements ()
		{
			return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS, true);
        }

        /// <summary>
        /// When the town hall is first built, set the potision of the 
        /// building on the map and set the max/min x/y coordinates
        /// </summary>
        /// <returns></returns>
		public override ResourceBox OnBuild ()
		{
			setCityTiles ();
			Tile townHallPos = _position;
			maxXCoord = townHallPos.GetX () + 1;
			maxYCoord = townHallPos.GetY () + 1;
			minXCoord = townHallPos.GetX () - 1;
			minYCoord = townHallPos.GetY () - 1;
			return base.OnBuild ();
		}

		public override void OnTurn ()
		{
			base.OnTurn ();
			/// Initially, only tiles owned by the current user are those directly surrounding the town hall
			/// After first turn, the maximum X and Y coordinates are dependent on the current city tiles
			maxXCoord = findMaxX (cityTiles);
			maxYCoord = findMaxY (cityTiles);
			minXCoord = findMinX (cityTiles);
			minYCoord = findMinY (cityTiles);

			///Trigger city expansion if population has increased by 2
			int population = _owner.Inventory.CheckResource (Resource.Population);
			if (population % 2 == 0) {  //needs to be changed
				expandCityBoundary ();
			}

        //    populationTarget ++;
		}


		/// <summary>
		/// Sets the city tiles by setting the owner of that tile to the current player and adding the tile
        /// to the current player's list of city tiles.
		/// </summary>
		private void setCityTiles ()
		{
			Tile townHallPos = _position;
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
		/// Expands the city boundary by randomly selecting one of the tiles bordering the city boundary 
		/// and setting this to be long to the player
		/// </summary>
		private void expandCityBoundary ()
		{
            //Choice of tile to expand to from the list will be random
			System.Random rnd = new System.Random ();
           
            int i = 0;
           // Debug.Log("Tiles owned: " + cityTiles.Count() + " Border tiles: " + borderTiles.Count());
			if (cityTiles.Count () >= borderTiles.Count ()+cityTiles.Count() && i == 0) {
				foreach (Tile tile in mapTiles) {
					if (tile.GetOwner () == null) {
						//Add all tiles surrounding the current city limit to borderTiles
						if (tile.GetY () == maxYCoord + 1 && tile.GetX () >= minXCoord - 1 && tile.GetX () - 1 <= maxXCoord) {	
							topBorder.Add (tile);
						} 
						if (tile.GetY () == minYCoord - 1 && tile.GetX () >= minXCoord - 1 && tile.GetX ()-1 <= maxXCoord) {
                            bottomBorder.Add(tile);
                        } 
						if (tile.GetX () == maxXCoord + 1 && tile.GetY () > minYCoord - 1 && tile.GetY () < maxYCoord + 1) {
							rightBorder.Add (tile);
                        } 
						if (tile.GetX () == minXCoord - 1 && tile.GetY () > minYCoord - 1 && tile.GetY () < maxYCoord + 1) {
							leftBorder.Add (tile);
                        }
					}
			    }

            //Add tiles in each border to borderTiles
            borderTiles.AddRange(topBorder);
            borderTiles.AddRange(bottomBorder);
            borderTiles.AddRange(leftBorder);
            borderTiles.AddRange(rightBorder);
            i++;

			}

            //Remove tiles from the borderTiles list if it's inside the city boundary
            for (int count = 0; count < borderTiles.Count(); count++)
            {
                if (cityTiles.Contains(borderTiles[count]))
                {
                    borderTiles.Remove(borderTiles[count]);
                   
                }
            }

            if (borderTiles.Count() != 0)
            {
                //Select a tile to expand to
                int borderIndex = borderTiles.Count();
                int expandToIndex = rnd.Next(borderIndex);
                Tile expandTo = borderTiles.ElementAt(expandToIndex);

                //Expand the city boundary
                expandTo.SetOwner(playerName, playerColour);
                cityTiles.Add(expandTo);

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