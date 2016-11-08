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
			List<Type> buildingTypes = Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => t.BaseType != null &&
				t.BaseType.IsGenericType &&
				t.BaseType.GetGenericTypeDefinition() == typeof(Building<>)).ToList<Type>();

			return buildingTypes;
		}

		public static Type LOOK_FOR_BUILDING (string buildingName)
		{
			foreach (Type t in BUILDING_REPO)
			{
				if (t.Name == buildingName) return t;
			}

			return null;
		}

		void Start ()
		{
			
		}
	}
}
