﻿using System.Collections.Generic;
using SpaceRace.PlayerTools;
using UnityEngine;

namespace SpaceRace.World.Buildings.Collection
{
    class Hotel : Building
    {
        public const string BUILDING_NAME = "Hotel";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public Hotel(Player builder, Tile pos)
            : base(typeof(Hotel), builder, pos, loaded_sprites) { }

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
