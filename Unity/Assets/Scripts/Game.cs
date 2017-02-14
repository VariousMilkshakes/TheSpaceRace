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

		private static readonly string new_turn_handler = "newTurn";

		public GameObject UiHandlerObject;

		private List<Player> activePlayers;
        private List<AI> activeAIs;
		private UiHack uiHandler;
		private Player activePlayer;
		private bool running;

		void Start()
		{
		    new GameRules();
			running = true;

			Player player1 = new Player();
            Player player2 = new Player();
            AI computerPlayer = new AI(new Player());
			player1.PlayerName = "1";
			player1.Color = Color.gray;
			player2.PlayerName = "2";
			player2.Color = Color.magenta;
            computerPlayer.PlayerName = "Computer";
            computerPlayer.Color = Color.blue;

			uiHandler = UiHandlerObject.GetComponent<UiHack>();
			uiHandler.BindTo(player1.PlayerUI);

			activePlayers = new List<Player>()
			{
				player1,
				player2
			};

            activeAIs = new List<AI>()
            {
                computerPlayer
            };

			StartCoroutine(new_turn_handler);
		}

		/// <summary>
		/// Async waits for both players to complete
		/// phase before moving to next turn
		/// turn cons
		/// </summary>
		/// <returns>Coroutine Enum</returns>
		private System.Collections.IEnumerator newTurn()
		{
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
                //Allow AI to take its turn
                foreach (AI p in activeAIs)
                {
                    p.OnTurn();
                }
                    StartCoroutine(new_turn_handler);
			}

		}

		/// <summary>
		/// Start players phase and bind UI to player
		/// </summary>
		private void newPhase ()
		{
			if (activePlayer.ReadyToAdvance) uiHandler.DisplayAdvanceButton();

			uiHandler.BindTo(activePlayer.PlayerUI);
			activePlayer.OnTurn();
            UiHack.ERROR.Handle("NEXT PLAYER");
		}

		/// <summary>
		/// Notifiy Game that current player phase
		/// has been completed
		/// </summary>
		public void CompleteTurn ()
		{
			activePlayer.TurnComplete = true;
			uiHandler.UnbindFrom();
		}

		public Player GetActivePlayer(){
			return activePlayer;
		}

		public String GetActivePlayerName() {
			return activePlayer.PlayerName;
		}

		public List<Player> GetActivePlayers(){
			return activePlayers;
		}
			
	}
}
