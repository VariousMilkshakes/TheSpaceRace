using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceRace.World.Buildings
{
	class BuildingException : Exception
	{
		public Type Building { get; set; }

		public BuildingException() { }

		public BuildingException(string message) : base(message) { }

		public BuildingException(string message, Exception inner) : base(message, inner) { }

		public BuildingException(string message, Type building)
		{
			Building = building;
		}
	}
}
