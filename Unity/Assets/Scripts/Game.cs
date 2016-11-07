using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using SpaceRace.World.Buildings;

using UnityEngine;

namespace SpaceRace
{
	class Game : MonoBehaviour
	{
		/// <summary>
		/// List of all building types in Building Collection
		/// </summary>
		public static readonly List<Type> BUILDING_REPO = LOAD_BUILDINGS();

		/// <summary>
		/// Finds all buildings inheriting from Building<> abstract type
		/// </summary>
		/// <returns>All buildings</returns>
		public static List<Type> LOAD_BUILDINGS()
		{
			List<Type> buildingTypes = new List<Type>(Assembly.GetAssembly(typeof(Building<>))
				.GetTypes()
				.Where(b => b.IsClass &&
				!b.IsAbstract &&
				b.IsSubclassOf(typeof(Building<>))));

			return buildingTypes;
		}

		void Start ()
		{
			
		}
	}
}
