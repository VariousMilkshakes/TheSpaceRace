using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utils;
using UnityEngine;

using SpaceRace.Game;
using SpaceRace.World;
using SpaceRace.World.Buildings;

namespace SpaceRace.PlayerTools
{
	[System.Serializable]
	public partial class Player : ITurnObject
	{

		private Inventory inventory;
		private List<Building> playerBuildings;
		private UIController playerUI;
	    private int playerTurn = 1;
		private List<Tile> playerTiles;

		public readonly string Name;
		public readonly Color Color;

		public bool ReadyToAdvance = false;
		public bool TurnComplete = false;
	    public WorldStates Age = WorldStates.Roman;

		public Inventory Inventory
		{
			get { return inventory; }
		}

	    public int BuildingCount
	    {
	        get { return playerBuildings.Count; }
	    }

		public UIController PlayerUI
		{
			get { return playerUI; }
		}

	    public int Turn
	    {
	        get { return playerTurn; }
	    }

		public Player (string name, Color colour)
		{
		    Name = name;
		    Color = colour;

			playerBuildings = new List<Building>();
			playerTiles = new List<Tile> ();

		    inventory = GameRules.DEF_INVENTORY();

            playerUI = new UIController(this);
        }

		public void OnTurn()
		{
            AdvanceTurn();

			if (inventory.CheckResource(Resource.Faith) == 100)
			{
				ReadyToAdvance = true;
			}

			foreach (Building building in playerBuildings)
			{
				Resource requiredRes = building.Input.Type;

				if (requiredRes != Resource.None && requiredRes != Resource.Free)
				{
					int resInInv = inventory.CheckResource(requiredRes);
					int resDeposit = building.Input.Fill(resInInv);

				    if (inventory.SpendResource(new ResourceBox(requiredRes, resDeposit))) {
                        building.OnTurn();
                    }
				} else {
				    building.OnTurn();
				}

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

	    public void RemoveBuilding (Building oldBuilding)
	    {
	        playerBuildings.Remove(oldBuilding);
	    }

	    public void AdvanceTurn ()
	    {
	        playerTurn++;
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
		public List<Building> GetPlayerBuildings(){
			return playerBuildings;
		}

		public List<Tile> GetPlayerTiles(){
			return playerTiles;
		}
	}
}
