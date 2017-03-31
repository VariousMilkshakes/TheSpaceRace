using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using SpaceRace;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using SpaceRace.World;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;

namespace SpaceRace.Game
{
    public class GameRules
    {
        #region Static Members
        /// <summary>
        /// Settings loaded from config files
        /// </summary>
        public static Dictionary<string, Config> CONFIG_REPO
        {
            get
            {
                if (config_repo == null) {
                    throw new Exception("Config files have not yet been loaded.");
                }

                return config_repo;
            }
        }

        /// <summary>
        /// Methods for validation and rules
        /// </summary>
        public static GameRules RULES
        {
            get { return game_rules; }
            set
            {
                if (game_rules == null) {
                    RULES = value;
                }
            }
        }

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

        /// <summary>
        /// Find building from repo, using string name of building
        /// </summary>
        /// <param name="buildingName">Name of building</param>
        /// <returns>Building type</returns>
        public static Type LOOK_FOR_BUILDING(string buildingName)
        {
            foreach (Type t in BUILDING_REPO)
            {
                if (t.Name == buildingName) return t;
            }

            return null;
        }

        private static Dictionary<string, Config> config_repo;
        private static readonly GameRules game_rules = new GameRules();
        #endregion

        public GameRules ()
        {
            config_repo = Config.LOAD();
        }

        #region Building Validation

        /// <summary>
        /// Only allow the first building to be built,
        /// to be a townhall
        /// </summary>
        /// <param name="buildingType">Attempted building type</param>
        /// <param name="builder">Player who is building</param>
        /// <returns>Whether or not building is valid</returns>
        public static bool FORCE_BUILD_ORDER(Type buildingType, Player builder)
        {
            return builder.BuildingCount != 0 || buildingType.Name == TownHall.BUILDING_NAME;
        }

        /// <summary>
        /// Checks that the tile being built on is valid for that building.
        /// Checking:
        ///     terrain
        ///     ownership
        ///     if it is connected to townhall
        /// </summary>
        /// <param name="buildingType">Building to build</param>
        /// <param name="tile">Tile selected by user</param>
        /// <param name="builder">Player building</param>
        /// <returns>Valid = True/False</returns>
        public static bool CHECK_BUILDING_TILE(Type buildingType, Tile tile, Player builder)
        {
            Config buildingConfig = CONFIG_REPO["Buildings"];
            string[] acceptedTiles = buildingConfig.LookForProperty(buildingType.Name, Building.VALID_TILES)
                                                    .Value
                                                    .Split(',');

            string tileType = Enum.GetName(typeof(Tile.TileType), tile.Type);
            string tileResource = Enum.GetName(typeof(Resource), tile.GetResource());
            if (!acceptedTiles.Contains(tileType) &&
                !acceptedTiles.Contains(tileResource)) return false;

            // TODO: use generic to convert config to datatype
            string boolString = buildingConfig.LookForProperty(buildingType.Name, Building.TOWNHALL_CONNECTION)
                                                .Value;

            // Main validation sequence
            bool needsTownHall = false;
            if (!bool.TryParse(boolString, out needsTownHall) && !needsTownHall) return false;

            if (tile.GetOwner() == null)
                return !needsTownHall;

            return tile.GetOwner() == builder.Name && tile.Building == null;
        }

        /// <summary>
        /// Check if player has access to building,
        /// dependent on level
        /// </summary>
        /// <param name="buildingType">Building to check</param>
        /// <param name="playerLevel">Current era of player</param>
        /// <returns>Whether or not the building is valid</returns>
        public static bool CHECK_PLAYER_BUILDING_LEVEL(Type buildingType, WorldStates playerLevel)
        {
            int buildingLevel;

            try
            {
                Config buildingConfig = CONFIG_REPO["Buildings"];
                var property = buildingConfig.LookForProperty(buildingType.Name, "Age");

                buildingLevel = (int)Enum.Parse(typeof(WorldStates), property.Value);
            }
            catch (Exception)
            {
                return false;
            }

            return (int)playerLevel >= buildingLevel;
        }

        #endregion

        /// <summary>
        /// Get buildings which upgrade from current building
        /// </summary>
        /// <param name="upgradableBuilding">Current building</param>
        /// <returns>Possible upgrades</returns>
        public static List<Type> GET_BUILDING_UPGRADES_FOR(Type upgradableBuilding)
        {
            Config buildingsConfig = CONFIG_REPO["Buildings"];
            string[] upgrades = buildingsConfig.LookForProperty(upgradableBuilding.Name, "Upgrade.To").Value.Split(',');

            return BUILDING_REPO
                .Where(building => upgrades.Contains(building.Name))
                .ToList();
        }

        #region Defaults

        private const int wood = 10;
        private const int pop = 1;
        private const int money = 6000;
        private const int stone = 1000;

        /// <summary>
        /// Default resource values for inventory
        /// </summary>
        /// <returns>Filled inventory</returns>
        public static Inventory DEF_INVENTORY ()
        {
            Inventory inv = new Inventory();
            inv.AddResource(Resource.Wood, wood);
            inv.AddResource(Resource.Population, pop);
            inv.AddResource(Resource.Money, money);
            inv.AddResource(Resource.Stone, stone);

            return inv;
        }

        #endregion
    }
}