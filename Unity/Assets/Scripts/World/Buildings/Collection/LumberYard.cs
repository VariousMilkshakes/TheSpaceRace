using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PT = SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;

namespace SpaceRace.World.Buildings.Collection
{
	class LumberYard : Building
	{
		public LumberYard() : base(typeof(LumberYard))
		{
			Sprite sprite = null;

			/// Set sprite for building
			try
			{
				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
				string spritePath = buildingConfigs.LookForProperty("LumberYard", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
			_buildingSprites.Add(WorldStates.All, sprite);

			Input = new PT.ResourceBox(PT.Resources.Money, 0, 10);
			Output = new PT.ResourceBox(PT.Resources.Wood, 0, 25);
		}

		public override PT.ResourceBox BuildRequirements ()
		{
			return new PT.ResourceBox(PT.Resources.Money, 50);
		}

		public override void OnTurn ()
		{
			base.OnTurn();
		}
	}
}
