using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PlayerTools;

namespace World.Buildings.Collection
{
	class LumberYard : Building<LumberYard>
	{
		public LumberYard(Type t) : base(t)
		{
		}

		public ResourceBox BuildRequirements ()
		{
			return _eventResources["OnBuild"];
		}

		public override void OnTurn ()
		{
			if (Input.Type == _eventResources["input"].Type)
		}
	}
}
