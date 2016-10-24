using UnityEngine;
using System.Collections;
using World.Buildings;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

using Utils;

public class GameRules : MonoBehaviour {

	public static readonly Dictionary<string, Config> CONFIG_REPO = Config.LOAD();

	GameObject MapObject { get; set; }

	// Use this for initialization
	void Start () {

		MapGenerator map = MapObject.GetComponent<MapGenerator>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
