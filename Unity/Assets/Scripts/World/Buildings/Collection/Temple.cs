using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using SpaceRace.World;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using UnityEngine;

namespace Assets.Scripts.World.Buildings.Collection
{
	class Temple : Building
	{
        public const string BUILDING_NAME = "Temple";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

	    public Temple (Player builder, Tile pos)
	        : base(typeof(Temple), builder, pos, loaded_sprites)
	    {
	        Upgradeable = true;
	    }

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
