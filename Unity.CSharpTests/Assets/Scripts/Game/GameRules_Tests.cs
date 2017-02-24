using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceRace.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceRace.PlayerTools;
using SpaceRace.Game;
using SpaceRace.World.Buildings.Collection;
using UnityEngine;

namespace SpaceRace.Game.Tests
{
    [TestClass()]
    public class GameRules_Tests
    {
        private GameRules rules;

//        [TestInitialize ()]
//        public void TestInitialize ()
//        {
//            rules = new GameRules();
//        }

        [TestMethod()]
        public void GameRules_Test()
        {
            rules = new GameRules();
            Assert.AreEqual(true, GameRules.CONFIG_REPO.Count > 0);
        }

        [TestMethod()]
        public void FORCE_BUILD_ORDER_Test()
        {
            Player player = new Player("test", Color.blue);
            Type buildable = GameRules.LOOK_FOR_BUILDING("TownHall");
            Type notBuildable = GameRules.LOOK_FOR_BUILDING("House");

            Assert.AreEqual(true, GameRules.FORCE_BUILD_ORDER(buildable, player));
            Assert.AreEqual(false, GameRules.FORCE_BUILD_ORDER(notBuildable, player));
        }

        [TestMethod()]
        public void CHECK_BUILDING_TILE_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CHECK_PLAYER_BUILDING_LEVEL_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GET_BUILDING_UPGRADES_FOR_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SETUP_INVENTORY_Test()
        {
            Assert.Fail();
        }
    }
}