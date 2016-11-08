using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceRace.PlayerTools;
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
		}

		public override ResourceBox BuildRequirements ()
		{
			return _eventResources["OnBuild"];
		}

		public override void OnTurn ()
		{
			Output = ResourceBox.EMPTY();

			if (Input.Type == _eventResources["input"].Type)
			{
				/// If the building gets insufficient input resources
				/// then the output is left empty
				if (Input.Spend(_eventResources["input"].Quantity))
				{
					Output = _eventResources["output"];
				}
			}
		}
	}
}
