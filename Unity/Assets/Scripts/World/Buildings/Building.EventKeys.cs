using System;

namespace SpaceRace.World.Buildings
{
    /// <summary>
    /// Set up the requirements for a building
    /// </summary>
	public partial class Building
	{
	    public const string CONFIG = "Buildings";
		public const string BUILDING_REQUIREMENTS = "Res.BuildRequirements";
		public const string OUTPUT_ON_BUILD = "Res.OnBuild";
        public const string INPUT_ON_TURN = "Res.OnTurnInput";
        public const string OUTPUT_ON_TURN = "Res.OnTurnOutput";
        public const string REQUIRES_WORKER = "Res.RequireWorker";
        public const string BUILDING_AGE = "Age";
        public const string VALID_TILES = "Tiles";
	    public const string TOWNHALL_CONNECTION = "RequireTownHall";


        public static string SPRITE (WorldStates state)
	    {
	        string stateName = Enum.GetName(typeof(WorldStates), state);

	        return "Sprite." + stateName;
	    }
    }
}
