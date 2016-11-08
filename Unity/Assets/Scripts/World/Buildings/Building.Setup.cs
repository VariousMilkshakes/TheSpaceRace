using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceRace.PlayerTools;
using SpaceRace.Utils;

using UE = UnityEngine;

namespace SpaceRace.World.Buildings
{
	public partial class Building
	{
		protected Dictionary<WorldStates, UE.Sprite> _buildingSprites;
		protected WorldStates _buildingState;
		protected Resources _buildingResource = Resources.None;
		protected int _buildingResourceCost = 0;

		#region ResourceBoxes For Events

		protected Dictionary<string, ResourceBox> _eventResources = new Dictionary<string, ResourceBox>()
		{
			{ "BuildRequirements", ResourceBox.EMPTY() },
			{ "OnBuild", ResourceBox.EMPTY() },
			{ "OnTurn", ResourceBox.EMPTY() }
		};

		#endregion

		public Building(Type t)
		{
			useConfig(checkForConfig(t));
			_buildingState = WorldStates.All;
		}

		/*
		* The Sprite of building to replace tile sprite
		*/
		public UE.Sprite ActiveSprite
		{
			get { return _buildingSprites[_buildingState]; }
		}

		public WorldStates State
		{
			get { return _buildingState; }
			set { _buildingState = value; }
		}

		/// <summary>
		/// Assign config values to object fields
		/// </summary>
		/// <param name="c">Relevant config</param>
		private void useConfig(List<Config.Property> c)
		{
			if (c == null) return;

			foreach (Config.Property prop in c)
			{
				try
				{
					string[] keyParts = prop.Key.Split('.');
					domainSwitch(keyParts, prop.Value);
				}
				catch
				{
					Console.WriteLine("Could not read property: " + prop.ToString());
				}
			}
		}

		/// <summary>
		/// Handle properties using the "Res" domain
		/// </summary>
		/// <param name="resourceProp"></param>
		private void assignResource(string[] propKey, string propValue)
		{
			if (propKey.Length < 3) return;

			ResourceBox newBox;
			
			if (propKey[2] == "type")
			{
				Resources rType = findEnumValue<Resources>(propValue);
				newBox = new ResourceBox(rType);
			}
			else if (_eventResources.ContainsKey(propKey[1]))
			{
				newBox = _eventResources[propKey[1]];
				try
				{
					int volumeValue = Int32.Parse(propValue);
					newBox.IncreaseQuantity(volumeValue);
				}
				catch
				{
					throw new Exception("Invalid property value");
				}
			}
		}

		/// <summary>
		/// Gets the Enum value that matches the string or ID
		/// </summary>
		/// <typeparam name="E">Enum type to search</typeparam>
		/// <param name="matchValue">Value to find in Enum</param>
		/// <returns>Relevant Enum value</returns>
		private E findEnumValue<E> (string matchValue)
		{
			int eID;

			try
			{
				//Check if an ID is being used
				eID = Int16.Parse(matchValue);
			}
			catch (FormatException)
			{
				//Try using Enum value instead
				string[] eValues = Enum.GetNames(typeof(E));
				eID = eValues.ToList().IndexOf(matchValue);
			}

			if (eID == -1) eID = 0;

			return (E)Enum.ToObject(typeof(E), eID);
		}

		private void saveResourceBox (ResourceBox box, string target)
		{
			if (!_eventResources.ContainsKey(target))
			{
				_eventResources.Add(target, box);
				return;
			}

			_eventResources[target] = box;
		}

		/// <summary>
		/// Handle Properties using the "Sprite" domain
		/// </summary>
		/// <param name="spriteProp"></param>
		private void assignSprite(string[] propKey, string propValue)
		{
			WorldStates targetState = findEnumValue<WorldStates>(propKey[1]);

			UE.Sprite newSprite = UE.Resources.Load<UE.Sprite>(propValue);
			_buildingSprites.Add(targetState, newSprite);
		}

		/// <summary>
		/// Redirects property using domain
		/// </summary>
		/// <param name="propKey"></param>
		private void domainSwitch(string[] propKey, string propValue)
		{
			if (propKey.Length <= 1) throw new Exception("Invalid Property");

			switch (propKey[0])
			{
				case "Res":
					assignResource(propKey, propValue);
					break;
				case "Sprite":
					assignSprite(propKey, propValue);
					break;
				default:
					throw new Exception("Invalid property domain");
			}
		}

		/// <summary>
		/// Check if object has relevant config file
		/// </summary>
		/// <param name="t">Type of building to look for config of</param>
		/// <returns>Relevant config</returns>
		public List<Config.Property> checkForConfig(Type t)
		{
			string target = t.Name;
			Config parentConfig = GameRules.CONFIG_REPO["Buildings"];

			if (GameRules.CONFIG_REPO.ContainsKey(target))
			{
				return parentConfig.Properties[target];
			}

			return null;
		}
	}
}
