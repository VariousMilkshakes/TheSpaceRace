using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceRace.PlayerTools
{
	/// <summary>
	/// Keeps track of players resources
	/// </summary>
	[System.Serializable]
	public class Inventory
	{

		/// <summary>
		/// Contains the players resource count
		/// </summary>
		private Dictionary<Resources, int> resources;

		public Inventory()
		{
			resources = new Dictionary<Resources, int>();
		}

		/// <summary>
		/// Find out how much of 'x' resource player has
		/// </summary>
		/// <param name="targetResource">Target resource to look up</param>
		/// <returns>Quantity of resource</returns>
		public int CheckResource(Resources targetResource)
		{
			if (!resources.ContainsKey(targetResource))
			{
				return 0;
			}

			return resources[targetResource];
		}

		/// <summary>
		/// Attempt to spend players resource
		/// </summary>
		/// <param name="requirements">Resource to spend</param>
		/// <returns>Returns true if the player had sufficient resources</returns>
		public bool SpendResource(ResourceBox requirements)
		{
			if (CheckResource(requirements.Type) >= requirements.Quantity)
			{
				resources[requirements.Type] -= requirements.Quantity;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Increase player's resource
		/// </summary>
		/// <param name="targetResource">Resource to increase</param>
		/// <param name="quantity">Amount to increase resource by</param>
		public void AddResource(Resources targetResource, int quantity)
		{
			if (resources.ContainsKey(targetResource))
			{
				resources[targetResource] += quantity;
			}

			// If resource does not yet exist create new key
			resources.Add(targetResource, quantity);
		}
	}
}