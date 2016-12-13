//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SpaceRace.PlayerTools;
//using SpaceRace.Utils;
//using SpaceRace.World;
//using SpaceRace.World.Buildings;
//using SpaceRace.World.Buildings.Collection;
//using UnityEngine;
//
//namespace Assets.Scripts.World.Buildings.Collection
//{
//    internal class Church : Building
//    {
//        public Church (Player builder) : base(typeof(Church), builder)
//        {
//            Sprite sprite = null;
//
//            /// Set sprite for building
//            try {
//                Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
//                string spritePath = buildingConfigs.LookForProperty("Church", "Sprite.All")
//                                                   .Value;
//                sprite = Resources.Load(spritePath, typeof(Sprite)) as Sprite;
//            } catch (Exception e) {
//                Debug.Log(e);
//            }
//            _buildingSprites.Add(WorldStates.All, sprite);
//
//            Input = new ResourceBox(Resource.Population, 0, 1);
//            Output = new ResourceBox(Resource.Stone, 0, 10);
//        }
//
//        public override ResourceBox BuildRequirements ()
//        {
//            return new ResourceBox(Resource.Wood, 30);
//        }
//
//        public override void OnTurn ()
//        {
//            base.OnTurn();
//        }
//    }
//}