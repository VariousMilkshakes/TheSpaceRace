using System;

using World.Buildings;

using PT = PlayerTools;
using UnityEngine;

namespace World.Buildings.Collection
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


	}
}
