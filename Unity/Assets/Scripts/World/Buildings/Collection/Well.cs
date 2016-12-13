using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;

using System.Linq;


namespace SpaceRace.World.Buildings.Collection
{
    public class Well : Building
    {
        public const string BUILDING_NAME = "Well";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public Well(Player builder, Tile pos) 
            : base (typeof(Well), builder, pos, loaded_sprites)
		{
            Input = GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, INPUT_ON_TURN);
            Output = GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, OUTPUT_ON_TURN);
        }

        public override Sprite GetActiveSprite()
        {
            return loaded_sprites[_buildingState];
        }

        public override ResourceBox BuildRequirements()
        {
            return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS);
        }
    }
}

