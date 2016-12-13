using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
		private Dictionary<Resource, int> resources;

		private Action<Resource> resourceUpdateEvent;

		public Inventory()
		{
			resources = new Dictionary<Resource, int>();
		}

		/// <summary>
		/// Find out how much of 'x' resource player has
		/// </summary>
		/// <param name="targetResource">Target resource to look up</param>
		/// <returns>Quantity of resource</returns>
		public int CheckResource(Resource targetResource)
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
				resourceUpdateEvent.Invoke(requirements.Type);
				return true;
			}

			return false;
		}

		public void AddResource(ResourceBox input)
		{
			AddResource(input.Type, input.Quantity);
		}

		/// <summary>
		/// Increase player's resource
		/// </summary>
		/// <param name="targetResource">Resource to increase</param>
		/// <param name="quantity">Amount to increase resource by</param>
		public void AddResource(Resource targetResource, int quantity)
		{
			if (resources.ContainsKey(targetResource))
			{
				resources[targetResource] += quantity;
			}
			else
			{
				// If resource does not yet exist create new key
				resources.Add(targetResource, quantity);
			}

			if (resourceUpdateEvent != null) resourceUpdateEvent.Invoke(targetResource);
		}

		/// <summary>
		/// Add listener to call event each time an inventory value is changed
		/// </summary>
		/// <param name="Listener">Event to call</param>
		public void AddListener (Action<Resource> Listener)
		{
			resourceUpdateEvent = Listener;
		}

		public void ClearListeners ()
		{
			resourceUpdateEvent = null;
		}
	}
}