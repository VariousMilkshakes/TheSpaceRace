﻿using System.Collections.Generic;
using SpaceRace.PlayerTools;
using SpaceRace.World;
using SpaceRace.World.Buildings;
using UnityEngine;

namespace Assets.Scripts.World.Buildings.Collection
{
	class Colosseum : Building
	{
        public const string BUILDING_NAME = "Colosseum";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public Colosseum(Player builder, Tile pos)
            : base (typeof(Colosseum), builder, pos, loaded_sprites){ }

        public override Sprite GetActiveSprite()
        {
            return loaded_sprites[_buildingState];
        }

        /// <summary>
        /// The resources required to construct the building
        /// </summary>
        /// <returns>Resources required for this building</returns>
        public override ResourceBox BuildRequirements()
        {
            return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS, true);
        }
    }
}