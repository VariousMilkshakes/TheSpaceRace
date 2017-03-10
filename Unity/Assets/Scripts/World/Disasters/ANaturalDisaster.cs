using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;

public abstract class ANaturalDisaster : MonoBehaviour {

	public abstract ResourceBox Cost ();

	public abstract void Target (GameObject target, bool destroyBuilding);
}