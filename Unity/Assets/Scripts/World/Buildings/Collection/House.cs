﻿using System;

using SpaceRace.World.Buildings;

using PT = SpaceRace.PlayerTools;
using UnityEngine;

namespace SpaceRace.World.Buildings.Collection
{
	class House : Building<House>
	{

		public House () : base (typeof(House))
		{

		}

		public override PT.ResourceBox BuildRequirements()
		{
			return _eventResources["OnBuild"];
		}

		public override void OnTurn()
		{
			throw new NotImplementedException();
		}
	}
}
