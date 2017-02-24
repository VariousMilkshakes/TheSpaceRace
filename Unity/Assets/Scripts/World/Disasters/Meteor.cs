using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : MonoBehaviour
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

	// Use this for initialization
	void Awake ()
	{
	    position = GetComponent<Transform>();
        movement = new Vector3(-1f * speed, -1f * speed);
        distance = new Vector3(100f, 100f);
	}
	
	// Update is called once per frame
	void Update () {
	    if (moving) {
            // Continue moving
            position.Translate(movement);

            // Check  colission with tile
	        if (position.position.x <= targetPos.x || position.position.y <= targetPos.y) {
	            moving = false;
                destroy();
	        }
        }

	    switch (explodeTime) {
            case -1:
                // Not exploding
	            break;
            case 0:
                // Clear disaster
                Destroy(gameObject);
	            break;
            default:
	            explodeTime--;
	            break;
	    }
	}

    /// <summary>
    /// Runs on target on tile
    /// </summary>
    /// <param name="target">Target tile</param>
    /// <param name="destroyBuilding"></param>
    public void Target (GameObject target, bool destroyBuilding = true)
    {
        destoryable = destroyBuilding;
        targetTile = target;
        targetPos = target.GetComponent<Transform>().position;

        position = GetComponent<Transform>();
        position.Translate(distance);

        moving = true;
    }

    private void destroy ()
    {
        Animator ani = GetComponent<Animator>();

        ani.runtimeAnimatorController = ExplosionAnimation;
        explodeTime = 60;

        if (destoryable)
            // TODO: Disable building instead of destroying
            targetTile.GetComponent<Tile>().DestoryBuilding();
        else
            damage();
    }

    // Pick random resource to destory
    private void damage ()
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
