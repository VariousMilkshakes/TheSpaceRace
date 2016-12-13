using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceRace;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	/// <summary>
	/// Sits between player and UI
	/// Bound to UiHandler on relative players turn
	/// </summary>
	class UIController
	{

		private Player owner;
		// TODO: Change to Iva's UI when ready
		private UiHack uiHandler;

		public UIController(Player currentPlayer)
		{
			owner = currentPlayer;
		}

		/// <summary>
		/// Start using this controller
		/// at the start of players turn
		/// </summary>
		/// <param name="handler">Current ui handler</param>
		public void AttachToHandler(UiHack handler)
		{
			uiHandler = handler;
		}

		/// <summary>
		/// Detach from handler at the end of turn
		/// </summary>
		public void DetachFromHandler()
		{
			uiHandler = null;
		}

		/// <summary>
		/// Tell UI to update resource trackers
		/// </summary>
		public void ResourceUpdate()
		{
			uiHandler.ResourceUpdate(owner.Inventory);
		}

		public List<Type> GetValidBuildings()
		{
			foreach (Type building in Game.BUILDING_REPO)
			{
				
			}
		}

		private bool checkBuildingRequirements(Type buildingType)
		{
			
		}

		private void a(){}

	}
}
