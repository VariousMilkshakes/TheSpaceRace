using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PlayerTools;

namespace World.Buildings.Collection
{
	class LumberYard : Building<LumberYard>
	{
		public LumberYard() : base(typeof(LumberYard))
		{
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
