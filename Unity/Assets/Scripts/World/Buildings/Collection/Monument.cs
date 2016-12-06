using System;

using SpaceRace.World.Buildings;

using PT = SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;

namespace SpaceRace.World.Buildings.Collection
{
	class Monument : Building
	{

		public Monument (PT.Player builder) : base (typeof(House), builder)
		{
			Sprite sprite = null;

			/// Set sprite for building
			try
			{
				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
				string spritePath = buildingConfigs.LookForProperty("Monument", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
			_buildingSprites.Add(WorldStates.All, sprite);

			Input = new PT.ResourceBox(PT.Resources.Free, 0, 0);
			Output = new PT.ResourceBox(PT.Resources.Faith, 0, 0);
		}

		public override PT.ResourceBox BuildRequirements()
		{
			return new PT.ResourceBox(PT.Resources.Wood, 400);
		}

		public override void OnTurn() { }

		public override PT.ResourceBox OnBuild()
		{
			return new PT.ResourceBox(PT.Resources.Faith, 100);
		}
	}
}
