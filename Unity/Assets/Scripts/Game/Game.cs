using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;

namespace SpaceRace.Game
{
    /// <summary>
    /// Handles running game turns
    /// </summary>
    public class Game : MonoBehaviour
    {
        private const string new_turn_handler = "newTurn";

        public GameObject UiHandlerObject; // UI GameObject

        public Player CurrentPlayer { get; private set; }

        private bool running = false; // Is game running?
        private List<Player> activePlayers;
        private UiHack uiHandler;

        /// <summary>
        /// Unity Start Method
        /// </summary>
        private void Start ()
        {
            running = true;
            activePlayers = GameManager.PLAYERS;
            if (activePlayers.Count == 0) {
                activePlayers.Add(new Player("P1", Color.cyan));
                activePlayers.Add(new Player("P2", Color.red));
            }

            // Bind UI to first player
            uiHandler = UiHandlerObject.GetComponent<UiHack>();
            uiHandler.BindTo(activePlayers[0].PlayerUI);

            // Begin first turn
            StartCoroutine(new_turn_handler);
        }

        /// <summary>
        /// Async turn, waits for both players to complete
        /// phase before moving to next turn
        /// </summary>
        /// <returns>Coroutine Enum</returns>
        private IEnumerator newTurn ()
        {
            foreach (var p in activePlayers) {
                newPhase(p);

                while (!p.TurnComplete) {
                    // Delay next check
                    yield return new WaitForFixedUpdate();
                }

                uiHandler.UnbindFrom();
                // Reset player tracker
                p.TurnComplete = false;
            }

            // Continue to next turn
            if (running) {
                StartCoroutine(new_turn_handler);
            }
        }

        /// <summary>
        /// Start new ACTION PHASE for player
        /// </summary>
        /// <param name="player">current player</param>
        private void newPhase (Player player)
        {
            if (player.ReadyToAdvance) uiHandler.DisplayAdvanceButton();
            else uiHandler.HideAdvanceButton();

            // Switch UI to player
            uiHandler.BindTo(player.PlayerUI);
            player.OnTurn();

            CurrentPlayer = player;
            UiHack.ERROR.Handle("NEXT PLAYER");
        }
    }
}