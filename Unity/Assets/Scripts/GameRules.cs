using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

using SpaceRace.Utils;
using SpaceRace.World.Buildings;

public class GameRules : MonoBehaviour {

	public static readonly Dictionary<string, Config> CONFIG_REPO = Config.LOAD();

	public GameObject MapObject;

	// Use this for initialization
	void Start () {

		MapGenerator map = MapObject.GetComponent<MapGenerator>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
