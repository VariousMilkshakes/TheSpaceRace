using SpaceRace.World.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRace.PlayerTools
{
	[System.Serializable]
	public partial class Player : TurnObject
	{

		private Inventory inventory;

		private List<TurnObject> playerBuildings;

		public Inventory Inventory
		{
			get { return inventory; }
		}

		public Player ()
		{
			playerBuildings = new List<TurnObject>();

			inventory = new Inventory();
			inventory.AddResource(Resources.Wood, 10);
			inventory.AddResource(Resources.Population, 0);
			inventory.AddResource(Resources.Money, 100);
		}

		public void OnTurn()
		{
			foreach (TurnObject buildingObject in playerBuildings)
			{
				Building<> building = buildingObject as Building<>;
			}
		}
	}
}
