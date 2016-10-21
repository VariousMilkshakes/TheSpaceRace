using System;

using World.Buildings;

using PT = PlayerTools;
using UnityEngine;

namespace World.Buildings.Collection
{
	class House : Building<House>
	{
		private PT.Resources buildingResource = PT.Resources.Wood;
		private int buildingResourceCost = 5;

		public override PT.ResourceBox BuildRequirements()
		{
			return new PT.ResourceBox(buildingResource, buildingResourceCost);
		}
	}
}
