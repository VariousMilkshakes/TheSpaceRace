using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tsunami : ANaturalDisaster {

	private Transform position;
	private Vector3 movement;
	private Vector3 distance;

	private GameObject targetTile;
	private Vector3 targetPos;

	private float speed = 0.5f;
	private bool moving = false;
	private int explodeTime = -1;
	private bool destoryable = false;
	private const float damageMod = 0.1f;

	private ResourceBox cost;

	void Awake(){
		cost = new ResourceBox (Resource.Faith, 10000);
	}

	override public ResourceBox Cost(){
		return cost;
	}

	override public void Target(GameObject target, bool destroyBuilding){
		destoryable = false;
		targetTile = target;
		targetPos = target.GetComponent<Transform>().position;

		position = GetComponent<Transform>();
		position.Translate(distance);

		moving = true;
	}
}
