using UnityEngine;
using System.Collections;
using World.Buildings;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class GameRules : MonoBehaviour {

	/// <summary>
	/// List of all building types in Building Collection
	/// </summary>
	public static readonly List<Type> BUILDING_REPO = _LOAD_BUILDINGS();

	/// <summary>
	/// Finds all buildings inheriting from Building<> abstract type
	/// </summary>
	/// <returns>All buildings</returns>
	private static List<Type> _LOAD_BUILDINGS ()
	{
		List<Type> buildingTypes = new List<Type>(Assembly.GetAssembly(typeof(Building<>))
			.GetTypes()
			.Where(b => b.IsClass &&
			!b.IsAbstract &&
			b.IsSubclassOf(typeof(Building<>))));

		return buildingTypes;
	}

	GameObject MapObject { get; set; }

	// Use this for initialization
	void Start () {

		MapGenerator map = MapObject.GetComponent<MapGenerator>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
