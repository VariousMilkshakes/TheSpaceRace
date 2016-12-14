﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;

namespace SpaceRace.World.Buildings.Collection
{
	class LumberYard : Building
	{
        public const string BUILDING_NAME = "LumberYard";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public LumberYard(Player builder, Tile pos)
            : base (typeof(LumberYard), builder, pos, loaded_sprites){ }

        public override Sprite GetActiveSprite()
        {
            return loaded_sprites[_buildingState];
        }

        public override ResourceBox BuildRequirements()
        {
            return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS, true);
        }
    }
}
