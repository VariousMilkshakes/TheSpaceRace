using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using UnityEngine;
using Random = UnityEngine.Random;
namespace SpaceRace.World.Disasters{
	public class Meteor : ANaturalDisaster
	{

	    private Transform position;
	    private Vector3 movement;
	    private Vector3 distance;

	    private GameObject targetTile;
	    private Vector3 targetPos;

	    public RuntimeAnimatorController ExplosionAnimation;

	    private float speed = 0.5f;
	    private bool moving = false;
	    private int explodeTime = -1;
	    private bool destoryable = false;
	    private const float damageMod = 0.1f;

		private ResourceBox cost;

		// Use this for initialization
		void Awake ()
		{
		    position = GetComponent<Transform>();
	        movement = new Vector3(-1f * speed, -1f * speed);
	        distance = new Vector3(100f, 100f);

			cost = new ResourceBox(Resource.Faith, 100);
		}
		
		// Update is called once per frame
		void Update () {
			Move ();

		    switch (explodeTime) {
	            case -1:
		            break;
	            case 0:
	                Destroy(gameObject);
		            break;
	            default:
		            explodeTime--;
		            break;
		    }
		}

		void Move(){
			if (moving) {
				position.Translate(movement);

				if (position.position.x <= targetPos.x || position.position.y <= targetPos.y) {
					moving = false;
					DestroyTarget();
				}
			}
		}

		override public ResourceBox Cost(){
			return cost;
		}

		override public void Target (GameObject target, bool destroyBuilding)
	    {
	        destoryable = destroyBuilding;
	        targetTile = target;
	        targetPos = target.GetComponent<Transform>().position;

	        position = GetComponent<Transform>();
	        position.Translate(distance);

	        moving = true;
	    }

		public void DestroyTarget ()
	    {
	        Animator ani = GetComponent<Animator>();

	        ani.runtimeAnimatorController = ExplosionAnimation;
	        explodeTime = 60;

	        if (destoryable)
	            targetTile.GetComponent<Tile>().DestoryBuilding();
	        else
	            Damage();
	    }

	    public void Damage ()
	    {
	        int min = 0;
	        int max = Enum.GetValues(typeof(Resource))
	                      .Length - 1;

	        Resource targetResource = (Resource)Random.Range(min, max);
	        targetTile.GetComponent<Tile>()
	                  .Building
	                  .Owner
	                  .Inventory
	                  .ModifyResource(targetResource, 1f - damageMod);
	    }
	}
}
