using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SpaceRace.PlayerTools;
using SpaceRace;

namespace SpaceRace.World.Buildings
{
	public abstract partial class Building : TurnObject
	{
		/// <summary>
		/// Attempt for player to create instance of building
		/// </summary>
		/// <typeparam name="T">Type of building to create</typeparam>
		/// <param name="builder">Player building building</param>
		/// <returns>New instance of building</returns>
		/// <throws>Buidling Exception if player has insufficient resources</throws>
		public static T BUILD<T>(Player builder) where T : Building, new()
		{
			UnityEngine.Debug.Log(typeof(T).Name);
			T newBuilding = (T) Activator.CreateInstance(typeof(T), new object[] {builder});
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
		public virtual ResourceBox OnBuild ()
		{
			return ResourceBox.EMPTY();
		}

		/// <summary>
		/// Called each game turn
		/// </summary>
		/// <returns>Resource provided on tick</returns>
		public virtual void OnTurn()
		{
			Output.Empty();

			/// If the building gets insufficient input resources
			/// then the output is left empty
			if (Input.IsFull())
			{
				Output.Fill(Output.Cap);
				Input.Empty();
			}
		}

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
