<<<<<<< HEAD:Unity/Assets/Scripts/World/Buildings/Building.cs
﻿using System;
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
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using PlayerTools;
using SpaceRace;

namespace World.Buildings
{
	abstract partial class Building<T> : TurnObject
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

		/// <summary>
		/// Resources required for a player to build
		/// </summary>
		/// <returns>Required resources</returns>
		public abstract ResourceBox BuildRequirements();

		/// <summary>
		/// Input and output resources from building
		/// </summary>
		public ResourceBox Input;
		public ResourceBox Output;

		/// <summary>
		/// Called when the building is built
		/// </summary>
		/// <returns>Resources provided once the building is completed</returns>
		public ResourceBox OnBuild ()
		{
			return ResourceBox.EMPTY();
		}

		/// <summary>
		/// Called each game turn
		/// </summary>
		/// <returns>Resource provided on tick</returns>
		public abstract void OnTurn();

		/// <summary>
		/// Called each time building is upgraded
		/// </summary>
		/// <returns>Resources provided on upgrade</returns>
		public ResourceBox OnUpgrade ()
		{
			return ResourceBox.EMPTY();
		}

	}
}
>>>>>>> refs/remotes/origin/dev:Unity/Assets/Scripts/World/Buildings/Building.Events.cs
