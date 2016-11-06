using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using PlayerTools;

namespace World.Buildings
{
	abstract class Building<T>
		where T : Building<T>, new()
	{

		public static T BUILD(Player builder)
		{
			T newBuilding = new T();
			Inventory builderInv = builder.Inventory;

			// Check if player can afford to spend resources
			if (builderInv.SpendResource(newBuilding.BuildRequirements()))
			{
				return newBuilding;
			}

			// Throw alert to UI to be displayed to user
			throw new BuildingException("Not enough resources", typeof(T));
		}

		/*
		* The SpriteRenderer of building to replace tile sprite
		*/
		/*private*/public SpriteRenderer sr = new SpriteRenderer ();

		/// <summary>
		/// The number of resources contained within building
		/// </summary>
		public int ResourceCount
		{
			get; set;
		}

		/// <summary>
		/// Resources required for a player to build
		/// </summary>
		/// <returns>Required resources</returns>
		public abstract ResourceBox BuildRequirements();

		/// <summary>
		/// Called when the building is built
		/// </summary>
		/// <returns>Resources provided once the building is completed</returns>
		public ResourceBox OnBuild ()
		{
			return ResourceBox.EMPTY();
		}

		/// <summary>
		/// Called each game 'tick'
		/// </summary>
		/// <returns>Resource provided on tick</returns>
		public ResourceBox OnTick ()
		{
			return ResourceBox.EMPTY();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ResourceBox OnUpgrade ()
		{
			return ResourceBox.EMPTY();
		}

	}
}
