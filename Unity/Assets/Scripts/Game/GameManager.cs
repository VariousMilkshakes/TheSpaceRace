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
    static class GameManager
    {

        public static List<Player> PLAYERS = new List<Player>();
        public static int MAP_WIDTH = 64;
        public static int MAP_HEIGHT = 64;
        public static int MAP_WATER = 20;

        public static Player Winner;

        private static Game CURRENT_GAME;

    }
}
