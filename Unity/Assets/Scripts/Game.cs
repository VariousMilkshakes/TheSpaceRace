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

		public GameObject UiHandlerObject;

		private List<Player> activePlayers;
		private UiHack uiHandler;
		private Player activePlayer;
		private bool running;

		private int turn = 0;



		void Start()
		{
			running = true;

			Player player1 = new Player();
			Player player2 = new Player();
			player1.PlayerName = "Jim";
			player1.Color = Color.magenta;
			player2.PlayerName = "Jack";
			player2.Color = Color.cyan;

			activePlayers = new List<Player>()
			{
				player1,
				player2
			};

			uiHandler = UiHandlerObject.GetComponent<UiHack>();
			uiHandler.BindPlayer(player1);
			uiHandler.ResourceUpdate();

			StartCoroutine("NewTurn");
		}

		public System.Collections.IEnumerator NewTurn()
		{
			turn++;

			foreach (Player p in activePlayers)
			{
				activePlayer = p;
				newPhase();

				while (!activePlayer.TurnComplete)
				{
					yield return new WaitForSeconds(.1f);
				}

				activePlayer.TurnComplete = false;
			}

			if (running)
			{
				StartCoroutine("NewTurn");
			}
		}

		private void newPhase ()
		{
			uiHandler.SetTurn(turn, activePlayer);
			if (activePlayer.ReadyToAdvance) uiHandler.DisplayAdvanceButton();

			activePlayer.OnTurn();
		}

		public void CompleteTurn ()
		{
			activePlayer.TurnComplete = true;
		}

		public Player GetActivePlayer(){
			return activePlayer;
		}

		public String GetActivePlayerName() {
			return activePlayer.PlayerName;
		}
			
	}
}
