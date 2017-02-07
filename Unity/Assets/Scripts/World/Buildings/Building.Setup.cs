using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpaceRace.PlayerTools;
using SpaceRace.Utils;

using UnityEngine;

namespace SpaceRace.World.Buildings
{
    public partial class Building
    {
        public bool Upgradeable = false;

        protected WorldStates _buildingState;
        protected Resource _buildingResource = Resource.None;
        protected int _buildingResourceCost = 0;
        protected Player _owner;
        protected Tile _position;

        #region ResourceBoxes For Events

        protected Dictionary<string, ResourceBox> _eventResources = new Dictionary<string, ResourceBox>(){
                                                                        { "BuildRequirements", ResourceBox.EMPTY() },
                                                                        { "OnBuild", ResourceBox.EMPTY() },
                                                                        { "OnTurn", ResourceBox.EMPTY() }
                                                                    };

        #endregion

        protected Building (Type t, Player builder, Tile pos, Dictionary<WorldStates, Sprite> spriteContainer)
        {
            _buildingState = WorldStates.All;
            _owner = builder;
            _position = pos;

            // Try to load building sprite from path in config
            try {
                Config buildingsConfig = GameRules.CONFIG_REPO[CONFIG];
                string spritePath = buildingsConfig.LookForProperty(t.Name, SPRITE(WorldStates.All)).Value;
                _setSprite(spriteContainer, Resources.Load(spritePath, typeof(Sprite)) as Sprite);
            } catch (Exception) {
                throw new BuildingException("Could not load building");
            }

            Input = GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(t.Name, INPUT_ON_TURN);
            Output = GameRules.CONFIG_REPO[CONFIG].GetPropertyResourceBox(t.Name, OUTPUT_ON_TURN);
        }

		public WorldStates State
		{
			get { return _buildingState; }
			set { _buildingState = value; }
		}

        public Player Owner
        {
            get { return _owner; }
        }

        /*
		* The Sprite of building to replace tile sprite
		*/
        public abstract Sprite GetActiveSprite();

	    protected void _setSprite (Dictionary<WorldStates, Sprite> sprites, Sprite newSprite)
	    {
	        if (!sprites.ContainsKey(_buildingState))
                sprites.Add(_buildingState, newSprite);
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
				Resource rType = findEnumValue<Resource>(propValue);
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
//					assignSprite(propKey, propValue);
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
