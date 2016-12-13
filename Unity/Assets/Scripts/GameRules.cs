using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using SpaceRace;
using SpaceRace.Utils;
using SpaceRace.World;
using SpaceRace.World.Buildings;

public class GameRules : MonoBehaviour {

	public static readonly Dictionary<string, Config> CONFIG_REPO = Config.LOAD();

	#region Building Validation

	public static bool CHECK_BUILDING_TILE(Type buildingType, String tileType)
	{
		Config buildingConfig = GameRules.CONFIG_REPO["Buildings"];
		string[] acceptedTiles = buildingConfig.LookForProperty(buildingType.Name, "Tiles").Value.Split(',');

		return acceptedTiles.Contains(tileType);
	}

	public static bool CHECK_PLAYER_BUILDING_LEVEL(Type buildingType, WorldStates playerLevel)
	{
		int buildingLevel;

		try
		{
			Config buildingConfig = GameRules.CONFIG_REPO["Buildings"];
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
		Config buildingsConfig = GameRules.CONFIG_REPO["Buildings"];
		string[] upgrades = buildingsConfig.LookForProperty(upgradableBuilding.Name, "Upgrade.To").Value.Split(',');

		return Game.BUILDING_REPO
			.Where(building => upgrades.Contains(building.Name))
			.ToList();
	}

	#endregion

}
