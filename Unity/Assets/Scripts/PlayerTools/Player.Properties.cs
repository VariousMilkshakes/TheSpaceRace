using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utils;
using UnityEngine;

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

		public string PlayerName;
		public Color Color;

		public bool ReadyToAdvance = false;
		public bool TurnComplete = false;
		public WorldStates Age = WorldStates.Roman;

		public Inventory Inventory
		{
			get { return inventory; }
		}

		public UIController PlayerUI
		{
			get { return playerUI; }
		}

	    public int Turn
	    {
	        get { return playerTurn; }
	    }

		public Player ()
		{
			playerBuildings = new List<Building>();

            inventory = new Inventory();
			inventory.AddResource(Resource.Wood, 10);
			inventory.AddResource(Resource.Population, 0);
			inventory.AddResource(Resource.Money, 100);

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

	    public void AdvanceTurn ()
	    {
	        playerTurn++;
	    }
	}
}
