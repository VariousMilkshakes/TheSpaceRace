using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;

public abstract class ANaturalDisaster : MonoBehaviour {

	public ResourceBox Cost(){
		return ResourceBox.EMPTY ();
	}

	public GameObject GetPrefab(){
		return null;
	}

	public void Target(GameObject target, bool destroyBuilding){
		
	}
}
