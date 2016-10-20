using System;

using World.Buildings;

using PlayerTools;

namespace World.Buildings.Collection
{
	class House : Building<House>
	{
		private Resources buildingResource = Resources.Wood;
		private int buildingResourceCost = 5;

		public override ResourceBox BuildRequirements()
		{
			return new ResourceBox(buildingResource, buildingResourceCost);
		}
	}
}
