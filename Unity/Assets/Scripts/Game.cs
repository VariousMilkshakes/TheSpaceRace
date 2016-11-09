using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SpaceRace.World.Buildings;

using UnityEngine;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;

namespace SpaceRace
{
	class Game : MonoBehaviour
	{
		/// <summary>
		/// List of all building types in Building Collection
		/// </summary>
		public static readonly List<Type> BUILDING_REPO = LOAD_BUILDINGS();

		/// <summary>
		/// Finds all buildings inheriting from Building<> abstract type
		/// </summary>
		/// <returns>All buildings</returns>
		public static List<Type> LOAD_BUILDINGS()
		{
			List<Type> buildingTypes = typeof(Building)
				.Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(Building)) && !t.IsAbstract).ToList<Type>();

			return buildingTypes;
		}

		public static Type LOOK_FOR_BUILDING (string buildingName)
		{
			foreach (Type t in BUILDING_REPO)
			{
				if (t.Name == buildingName) return t;
			}

			return null;
		}

		public GameObject UIHandlerObject;

		private List<TurnObject> activePlayers;
		private UiHack uiHandler;

		void Start()
		{
			Player player1 = new Player();

			activePlayers = new List<TurnObject>()
			{
				player1,
				new AI(new Player())
			};

			uiHandler = UIHandlerObject.GetComponent<UiHack>();
			uiHandler.BindPlayer(player1);
			uiHandler.ResourceUpdate();

			NewTurn();
		}

		public void NewTurn ()
		{
			foreach (TurnObject p in activePlayers)
			{
				if (p.GetType().Name == "Player")
				{
					Player player = p as Player;
					if (player.ReadyToAdvance) uiHandler.DisplayAdvanceButton();
				}

				p.OnTurn();
			}
		}
	}
}
