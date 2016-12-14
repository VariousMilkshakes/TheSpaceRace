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

public class GameRules {

    private static Dictionary<string, Config> config_repo;

    public static Dictionary<string, Config> CONFIG_REPO
    {
        get { return config_repo; }
        private set { config_repo = value; }
    }

    public GameRules ()
    {
        CONFIG_REPO = Config.LOAD();
    }

	#region Building Validation

    public static bool FORCE_BUILD_ORDER (Type buildingType, Player builder)
    {
        if (builder.BuildingCount == 0 &&
            buildingType.Name != TownHall.BUILDING_NAME) return false;
        return true;
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
	    if (!acceptedTiles.Contains(tileType)) return false;

        // TODO: use generic to convert config to datatype
	    string boolString = buildingConfig.LookForProperty(buildingType.Name, Building.TOWNHALL_CONNECTION)
	                                      .Value;
	    bool needsTownHall = false;
	    if (!bool.TryParse(boolString, out needsTownHall) && !needsTownHall) return false;

	    if (tile.GetOwner() == null) {
	        if (!needsTownHall) return true;
	        return false;
	    }

	    if (tile.GetOwner() != builder.PlayerName) return false;

	    return true;
	}

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

		if ((int)playerLevel >= buildingLevel)
		{
			return true;
		}

		return false;
	}

	public static List<Type> GET_BUILDING_UPGRADES_FOR(Type upgradableBuilding)
	{
		Config buildingsConfig = CONFIG_REPO["Buildings"];
		string[] upgrades = buildingsConfig.LookForProperty(upgradableBuilding.Name, "Upgrade.To").Value.Split(',');

		return Game.BUILDING_REPO
			.Where(building => upgrades.Contains(building.Name))
			.ToList();
	}

    #endregion

    #region Starting Resources

    private const int wood = 10;
    private const int pop = 1;
    private const int money = 600;

    public static void SETUP_INVENTORY (Inventory newInventory)
    {
        newInventory.AddResource(Resource.Wood, wood);
        newInventory.AddResource(Resource.Population, pop);
        newInventory.AddResource(Resource.Money, money);
        newInventory.AddResource(Resource.Stone, 500);
    }

    #endregion

}
