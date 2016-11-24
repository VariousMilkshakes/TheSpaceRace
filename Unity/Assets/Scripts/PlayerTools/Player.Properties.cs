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
	}
}
