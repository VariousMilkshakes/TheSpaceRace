using UnityEngine;
using System.Collections;
using World.Buildings;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class GameRules : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private List<Type> LoadBuildings ()
	{
		List<Type> buildingTypes = new List<Type>(Assembly.GetAssembly(typeof(Building<>))
			.GetTypes()
			.Where(b => b.IsClass &&
			!b.IsAbstract &&
			b.IsSubclassOf(typeof(Building<>))));

		return buildingTypes;
	}
}
