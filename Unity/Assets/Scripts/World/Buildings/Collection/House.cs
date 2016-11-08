﻿using System;

using SpaceRace.World.Buildings;

using PT = SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;

namespace SpaceRace.World.Buildings.Collection
{
	class House : Building
	{	

		public House () : base (typeof(House))
		{
			Sprite sprite = null;

			/// Set sprite for building
			try
			{
				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
				string spritePath = buildingConfigs.LookForProperty("House", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
			_buildingSprites.Add(WorldStates.All, sprite);
		}

		public override PT.ResourceBox BuildRequirements()
		{
			return new PT.ResourceBox(PT.Resources.Wood, 10);
		}

		public override void OnTurn()
		{
			return;
		}
	}
}
