using System.Collections.Generic;
using SpaceRace.PlayerTools;
using UnityEngine;
using System;

namespace SpaceRace.World.Buildings.Collection
{
    class SpaceCenter : Building
    {
        private System.Random rnd;
        public const string BUILDING_NAME = "SpaceCenter";
        private int hydrogen;

        /// <summary>
        /// Load sprites as singletons associated with worldstates
        /// Means that a sprite is only loaded once
        /// </summary>
	    private static Dictionary<WorldStates, Sprite> loaded_sprites = new Dictionary<WorldStates, Sprite>();

        public SpaceCenter(Player builder, Tile pos)
            : base(typeof(SpaceCenter), builder, pos, loaded_sprites) { }

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

        public override void OnTurn()
        {
            base.OnTurn();
            //check amount of hydrogen this player owns
            hydrogen = _owner.Inventory.CheckResource(Resource.Hydrogen);
            Debug.Log("Hydrogen: " + hydrogen);
    
            if(landShip() == true)
            {
                //end the game
                Debug.Log("Congratulations");
            } else
            {
                Debug.Log("Launch unsuccessful");
            }
        }

        /// <summary>
        /// Attempt to land the space ship on the moon
        /// </summary>
        /// <returns>Returns true if landing was successful</returns>
        private Boolean landShip()
        {
            int landProb = calculateLandProb(hydrogen);
            Debug.Log("Land prob: " + landProb);
            if (landProb <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculate the probability of landing the space ship
        /// </summary>
        /// <param name="hydrogen"></param>
        /// <returns></returns>
        private int calculateLandProb(int hydrogen)
        {
            rnd = new System.Random();
            int prob;
            if (hydrogen < 350)
            {
                Debug.Log("Lowest");
                prob = rnd.Next(100);
            }
            else if (hydrogen < 500)
            {
                Debug.Log("Low");
                prob = rnd.Next(80);
            }
            else if (hydrogen < 650)
            {
                Debug.Log("Middle");
                prob = rnd.Next(60);
            }
            else if (hydrogen < 800)
            {
                Debug.Log("Good");
                prob = rnd.Next(40);
            }
            else if (hydrogen >= 800)
            {
                Debug.Log("Best");
                prob = rnd.Next(20);
            }
            else
            {
                prob = 0;
            }

            /* int result = (int)Math.Floor((double)(Math.Abs(prob - prob) * (1 + 100 - 20) + 20));
             return result;*/
            return prob;
        }

       
    }
}
