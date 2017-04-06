using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpaceRace.PlayerTools;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.Scripts.Utils;
using SpaceRace.Utils;

public class Volcano : ANaturalDisaster {

	private GameObject targetTile;
	private Vector3 targetPos;

	public RuntimeAnimatorController VolcanoAnimation;

	private List<GameObject> children;

	private int lifeTime = -1;
	private const float damageMod = 0.5f;

	private ResourceBox cost;

	void Awake(){
		cost = new ResourceBox (Resource.Faith, 5000);
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
		targetPos = target.GetComponent<Transform> ().position;


		int min = 0;
		int max = Enum.GetValues(typeof(Resource)).Length - 1;

		List<Tile> volcanoTiles = GetSurroundingTiles (targetTile.GetComponent<Tile> (), 1);
		List<Tile> meteorTiles = GetSurroundingTiles (targetTile.GetComponent<Tile> (), 4);

		foreach(Tile t in volcanoTiles){
			if (t.Building != null) {
				Resource targetResource = (Resource)Random.Range (min, max);
				t.Building.Owner.Inventory.ModifyResource (targetResource, 1f - damageMod);
			}
		}

		UIController controller = GameObject.Find ("TempUIHandler").GetComponent<UiHack> ().GetController () as UIController;
		foreach (Tile t in meteorTiles) {
			if(Random.value > 0.8){
				controller.Player.Inventory.AddResource (Resource.Faith, 100);
				controller.Cast (t, "Meteor");
			}

		}

		lifeTime = 320;
	}

	List<Tile> GetSurroundingTiles(Tile target, int radius){
		MapGenerator mapgen = (GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent<MapGenerator> ()) as MapGenerator;

		List<Tile> allTiles = mapgen.GetTiles ();
		List<Tile> returnTiles = new List<Tile> ();

		int tarX = target.GetX ();
		int tarY = target.GetY ();

		foreach (Tile t in allTiles) {
			int x = t.GetX ();
			int y = t.GetY ();
			if (radius <= 1) {
				if (x >= tarX - 1 && x <= tarX + 1) {
					if (y >= tarY - 1 && y <= tarY + 1) {
						returnTiles.Add (t);
					}
				}
			} else {
				if(IsCoordinateWithinCircle(tarX, tarY, radius, x, y)){
					returnTiles.Add (t);
				}
			}
		}

		return returnTiles;
	}

	private bool IsCoordinateWithinCircle(int midPointX, int midPointY, int radius, int x, int y){
		if(Mathf.Pow((float) (x - midPointX), 2) + Mathf.Pow((float) (y - midPointY), 2) <= Mathf.Pow((float) radius - 1, 2)){
			return true;
		}
		return false;
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
