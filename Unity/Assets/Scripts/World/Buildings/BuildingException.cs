using System;

namespace SpaceRace.World.Buildings
{
    /// <summary>
    /// Exception methods for constructing buildings, thrown in case of error
    /// </summary>
    public class BuildingException : Exception
	{
		public Type Building { get; set; }

		public BuildingException() { }

		public BuildingException(string message) : base(message) { }

		public BuildingException(string message, Exception inner) : base(message, inner) { }

		public BuildingException(string message, Type building) : base(message)
		{
			Building = building;
		}

	    public override string ToString()
	    {
	        return Message;
	    }
	}
}
