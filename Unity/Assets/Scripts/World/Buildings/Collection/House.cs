using System;
using System.Collections.Generic;
using SpaceRace.World.Buildings;

using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;

namespace SpaceRace.World.Buildings.Collection
{
    internal class House : Building
	{
	    public const string BUILDING_NAME = "House";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

		public House (Player builder, Tile pos)
            : base (typeof(House), builder, pos, loaded_sprites){}

	    public override Sprite GetActiveSprite ()
	    {
	        return loaded_sprites[_buildingState];
	    }

	    public override ResourceBox BuildRequirements()
		{
            return GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(BUILDING_NAME, BUILDING_REQUIREMENTS, true);
        }
	}
}
