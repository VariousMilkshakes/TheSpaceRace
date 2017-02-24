using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceRace.Game
{
    class GameManager
    {
        public static GameManager MANAGER
        {
            get { return manager; }
        }

        private static readonly GameManager manager = new GameManager();

        public List<Player> players;
        private Game currentGame;

        private GameManager()
        {
            players = new List<Player>();
        }

        public List<Player> GetActivePlayers ()
        {
            players.Add(new Player("Player 1", new Color(0.7f, 0.7f, 0.4f)));
            players.Add(new Player("Player 2", Color.red));
            return players;
        }
    }
}
