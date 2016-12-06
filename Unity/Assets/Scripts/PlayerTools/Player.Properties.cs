using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UE = UnityEngine;

using SpaceRace.World;
using SpaceRace.World.Buildings;

namespace SpaceRace.PlayerTools
{
	[System.Serializable]
	public partial class Player : TurnObject
	{

		private Inventory inventory;

		private List<Building> playerBuildings;
		private List<Tile> playerTiles;

		public string PlayerName;
		public UE.Color Color;

		public bool ReadyToAdvance = false;
		public bool TurnComplete = false;
		public WorldStates Age = WorldStates.Roman;

		public Inventory Inventory
		{
			get { return inventory; }
		}

		public Player ()
		{
			playerBuildings = new List<Building>();
			playerTiles = new List<Tile> ();

			inventory = new Inventory();
			inventory.AddResource(Resources.Wood, 10);
			inventory.AddResource(Resources.Population, 0);
			inventory.AddResource(Resources.Money, 100);
		}

		public void OnTurn()
		{
			if (inventory.CheckResource(Resources.Faith) == 100)
			{
				ReadyToAdvance = true;
			}

			foreach (Building building in playerBuildings)
			{
				Resources requiredRes = building.Input.Type;

				if (requiredRes != Resources.None && requiredRes != Resources.Free)
				{
					int resInInv = inventory.CheckResource(requiredRes);
					int resDeposit = building.Input.Fill(resInInv);
					inventory.SpendResource(new ResourceBox(requiredRes, resDeposit));
				}

				building.OnTurn();

				inventory.AddResource(building.Output);
			}
		}

		/// <summary>
		/// Keep track of newly constructed building
		/// </summary>
		/// <param name="newBuilding"></param>
		public void TrackBuilding (Building newBuilding)
		{
			playerBuildings.Add(newBuilding);
		}

		/// <summary>
		/// Tracks the tiles owned by this player
		/// </summary>
		/// <param name="newTile">New tile.</param>
		public void TrackTile(Tile newTile){
			playerTiles.Add (newTile);
		}

		/// <summary>
		/// Gets the buildings owned by this player
		/// </summary>
		/// <returns>The player's buildings.</returns>
		public List<SpaceRace.World.Buildings.Building> GetPlayerBuildings(){
			return playerBuildings;
		}

		public List<Tile> GetPlayerTiles(){
			return playerTiles;
		}
	}
}
