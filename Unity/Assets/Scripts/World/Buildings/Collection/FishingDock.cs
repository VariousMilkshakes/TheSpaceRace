﻿using SpaceRace.PlayerTools;
using UnityEngine;
using System.Collections.Generic;


namespace SpaceRace.World.Buildings.Collection
{
	/// <summary>
	/// Town hall. Responsible for setting and expanding the city boundary.
	/// </summary>
	public class FishingDock : Building
	{
        public const string BUILDING_NAME = "FishingDock";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public FishingDock(Player builder, Tile pos)
            : base (typeof(FishingDock), builder, pos, loaded_sprites){ }

        public override Sprite GetActiveSprite()
        {
            return loaded_sprites[_buildingState];
        }

        /// <summary>
        /// The resources required to construct the building
        /// </summary>
        /// <returns>ResourceBox required for this building</returns>
        public override ResourceBox BuildRequirements()
        {
            return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS, true);
        }
    }
}

