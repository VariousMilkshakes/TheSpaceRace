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
//	class Field : Building
//	{
//		public Field(Player builder) : base (typeof(Field), builder)
//		{
//			Sprite sprite = null;
//
//			/// Set sprite for building
//			try
//			{
//				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
//				string spritePath = buildingConfigs.LookForProperty("Field", "Sprite.All").Value;
//				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
//			}
//			catch (Exception e)
//			{
//				Debug.Log(e);
//			}
//			_buildingSprites.Add(WorldStates.All, sprite);
//
//			Input = new SpaceRace.PlayerTools.ResourceBox(SpaceRace.PlayerTools.Resource.Population, 0, 1);
//			Output = new SpaceRace.PlayerTools.ResourceBox(SpaceRace.PlayerTools.Resource.Stone, 0, 10);
//		}
//
//		public override SpaceRace.PlayerTools.ResourceBox BuildRequirements()
//		{
//			return new SpaceRace.PlayerTools.ResourceBox(SpaceRace.PlayerTools.Resource.Wood, 30);
//		}
//
//		public override void OnTurn()
//		{
//			base.OnTurn();
//		}
//	}
//}
