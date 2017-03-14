using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpaceRace.PlayerTools;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tsunami : ANaturalDisaster {

	private Transform position;
	private Vector3 movement;
	private Vector3 distance;

	private GameObject targetTile;
	private Vector3 targetPos;

	public RuntimeAnimatorController TsunamiAnimation;

	private List<GameObject> children;

	private int lifeTime = -1;
	private const float damageMod = 0.2f;

	private ResourceBox cost;

	void Awake(){
		cost = new ResourceBox (Resource.Faith, 1000);
		children = new List<GameObject> ();
	}

	void Update(){
		CheckLifeTime ();
	}

	override public ResourceBox Cost(){
		return cost;
	}

	override public void Target(GameObject target, bool destroyBuilding){
		targetTile = target;
		targetPos = target.GetComponent<Transform>().position;

		int min = 0;
		int max = Enum.GetValues(typeof(Resource)).Length - 1;

		Animator ani = gameObject.GetComponent<Animator> ();
		ani.runtimeAnimatorController = TsunamiAnimation;

		List<Tile> surroundingTiles = GetSurroundingTiles (target.GetComponent<Tile> ());

		if (!surroundingTiles.Any ()) {
			throw new Exception ("The List of surrounding tiles is empty!");
		}

		foreach (Tile t in surroundingTiles) {
			Debug.Log (t.GetX() + ", " + t.GetY());
			GameObject tsunamiChild = Instantiate (Resources.Load ("Prefabs/Disasters/TsunamiChild"), t.GetComponentInParent<Transform>().position, Quaternion.identity) as GameObject;
			children.Add (tsunamiChild);

			tsunamiChild.GetComponent<Animator> ().runtimeAnimatorController = TsunamiAnimation;

			if (t.Building != null) {
				Resource targetResource = (Resource)Random.Range (min, max);
				t.Building.Owner.Inventory.ModifyResource (targetResource, 1f - damageMod);
			}
		}

		lifeTime = 120;

	}

	List<Tile> GetSurroundingTiles(Tile target){
		MapGenerator mapgen = (GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent<MapGenerator> ()) as MapGenerator;

		List<Tile> allTiles = mapgen.GetTiles ();
		List<Tile> returnTiles = new List<Tile> ();

		int tarX = target.GetX ();
		int tarY = target.GetY ();

		foreach (Tile t in allTiles) {
			int x = t.GetX ();
			int y = t.GetY ();

			if (x >= tarX - 1 && x <= tarX + 1) {
				if (y >= tarY - 1 && y <= tarY + 1) {
					returnTiles.Add (t);
				}
			}
		}

		return returnTiles;
	}

	void CheckLifeTime(){
		switch (lifeTime) {
		case -1:
			break;
		case 0:
			foreach (GameObject child in children) {
				Destroy (child);
			}
			Destroy (gameObject);
			break;
		default:
			lifeTime--;
			break;
		}
	}
}
