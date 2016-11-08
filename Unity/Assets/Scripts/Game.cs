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
			List<Type> buildingTypes = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => t.BaseType != null &&
				t.BaseType.IsGenericType &&
				t.BaseType.GetGenericTypeDefinition() == typeof(Building<>)).ToList<Type>();

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
				new AI()
			};

			uiHandler = UIHandlerObject.GetComponent<UiHack>();
			uiHandler.currentPlayer = player1;
		}

		private void newTurn ()
		{
			foreach (TurnObject p in activePlayers)
			{
				p.OnTurn();
			}
		}
	}
}
