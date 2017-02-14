using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;

namespace SpaceRace.World.Disasters{
	public interface INaturalDisaster{

		ResourceBox Cost();

		GameObject GetPrefab ();

		void Target (GameObject target, bool destrayable);

		void Damage ();

	}
}
