using System.Collections.Generic;
using SpaceRace.PlayerTools;
using SpaceRace.World;
using SpaceRace.World.Buildings;
using UnityEngine;

namespace Assets.Scripts.World.Buildings.Collection
{
    internal class Church : Building
    {
        public const string BUILDING_NAME = "Church";

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public Church(Player builder, Tile pos)
            : base (typeof(Church), builder, pos, loaded_sprites){ }

        public override ResourceBox OnBuild ()
        {
            if ((int)_owner.Age < (int)WorldStates.MiddleAges) {
                _owner.ReadyToAdvance = true;
            }

            return base.OnBuild();
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