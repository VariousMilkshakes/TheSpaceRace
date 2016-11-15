namespace SpaceRace.World
{
	/// <summary>
	/// Used to easly apply states to the world, tiles and buildings
	/// </summary>
	[System.Serializable]
	public enum WorldStates
	{
		None,
		All,
		Roman,		// /
		Viking,		// |
		MiddleAges,	// |
		Tudor,		// \ Era Dependent
		CivilWar,	// / World States
		Empirial,	// |
		Modern,		// |
		Space,		// \

		Damaged,	// /
		Flooded,	// \ Natural Disaster Dependent
		BroughtOut, // / World States
		Burning		// \
	}
}
