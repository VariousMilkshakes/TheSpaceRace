using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceRace;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using SpaceRace.World;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using SpaceRace.World.Disasters;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
	/// <summary>
	/// Sits between player and UI
	/// Bound to UiHandler on relative players turn
	/// </summary>
	public class UIController
	{

		private Player owner;

		#region UpdateNotification

		private UpdateEvent propertyUpdateEvent;

		public UpdateEvent PropertyUpdateEvent
		{
			get { return propertyUpdateEvent; }
		}

		#endregion

		public UIController(Player currentPlayer)
		{
			owner = currentPlayer;
			propertyUpdateEvent = new UpdateEvent();
            owner.Inventory.AddListener(ResourceUpdate);
		}

		public Player Player
		{
			get { return owner; }
		}

		public void NotifyPropertyChange(int propertyFlag, object[] updateArgs)
   		{
			PropertyUpdateEvent.Invoke(propertyFlag, updateArgs);
		}

		/// <summary>
		/// Fetchs updated value then updates tracker
		/// </summary>
		/// <param name="resource">Tracker to update</param>
		public void ResourceUpdate(Resource resource)
		{
			int updatedValue = owner.Inventory.CheckResource(resource);
			NotifyPropertyChange(UiHack.PROPERTY_RESOURCE_TRACKERS, new object[]{resource, updatedValue});
		}

		/// <summary>
		/// Update all trackers
		/// </summary>
		public void ResourceUpdate()
		{
			var allResources =
				Enum.GetValues(typeof(Resource)).Cast<Resource>();

			foreach (Resource res in allResources)
			{
				ResourceUpdate(res);
			}
			;
		}

		/// <summary>
		/// Returns buildings which player can build on tile
		/// </summary>
		/// <param name="selectedTile">Tile to be built on</param>
		/// <returns>Valid buildings</returns>
		public List<Type> GetValidBuildings(Tile selectedTile)
		{
			//TODO: Change when will has added tile type

			return Game.BUILDING_REPO
				.Where(building =>
                    GameRules.FORCE_BUILD_ORDER(building, owner) &&
					GameRules.CHECK_BUILDING_TILE(building, selectedTile, owner) &&
					GameRules.CHECK_PLAYER_BUILDING_LEVEL(building, owner.Age))
				.ToList();
		}

		/// <summary>
		/// Convert building type into button
		/// </summary>
		/// <param name="building"></param>
		/// <param name="buttonSize"></param>
		/// <param name="delegateTile"></param>
		/// <returns></returns>
		public GameObject CreateBuildingButton(Type building, int buttonSize, Tile delegateTile)
		{
			GameObject newMenuItem = (GameObject)GameObject.Instantiate(
				UiHack.BUILDING_ITEM_TEMPLATE,
				new Vector3(0, 0),
				UiHack.BUILDING_ITEM_TEMPLATE.transform.rotation);

			Button menuButton = newMenuItem.GetComponent<Button>();

			// Store in local scope to set inside delegate
			Type localBuilding = building;
			menuButton.onClick.AddListener(delegate
			{
				var localOwner = owner;
			    var localTile = delegateTile;

				if (localTile.Build(localBuilding, localOwner))
					localTile.ApplyPlayerColor(localOwner.Color);

				NotifyPropertyChange(UiHack.PROPERTY_CLEAR_BUILDINGS, null);
			});

			// Set sprite for building
		    try {
		        Config buildingConfig = GameRules.CONFIG_REPO["Buildings"];
		        string spritePath = buildingConfig.LookForProperty(building.Name, "Sprite.All")
		                                          .Value;
		        Texture2D rawSprite = Resources.Load(spritePath, typeof(Texture2D)) as Texture2D;

		        menuButton.image.sprite = Sprite.Create(rawSprite,
		                                                new Rect(0, 0, buttonSize, buttonSize),
		                                                new Vector2());
		    } catch (Exception e) {
		        Debug.Log(e);
		    }

		    ButtonHover hover = menuButton.GetComponent<ButtonHover>();
		    hover.TargetBuilding = building;
            hover.UiHandler = GameObject.Find("TempUIHandler");

		    return newMenuItem;
		}

		public void AdvancePlayerAge()
		{
			int current = (int) owner.Age + 1;
			owner.Age = (WorldStates) Enum.GetValues(typeof(Resource)).GetValue(current);
		    owner.ReadyToAdvance = false;
		}

        //TODO: Make abstract disaster
		public void Cast (Tile selectedTile, string disasterName)
	    {
	        if (selectedTile == null) {
				UiHack.ERROR.Handle ("No tile selected!");
            }

			bool destroyBuilding = !(selectedTile.Building != null && selectedTile.Building.GetType ().Name == TownHall.BUILDING_NAME);

			Transform t = selectedTile.gameObject.GetComponent<Transform> ();
			GameObject cast = GameObject.Instantiate(Resources.Load ("Prefabs/Disasters/" + disasterName, typeof(GameObject))) as GameObject;

			ANaturalDisaster disaster = cast.GetComponent <ANaturalDisaster> ();

			ResourceBox cost = disaster.Cost();
			if (!Player.Inventory.SpendResource(cost)) {
				UiHack.ERROR.Handle("Not enough faith!");
				GameObject.Destroy (cast);
				return;
			}

			disaster.Target (selectedTile.gameObject, destroyBuilding);
        }

        public void FetchBuildingInfo(Type building)
        {
            string info = building.Name + "\n";

            Config config = GameRules.CONFIG_REPO[Building.CONFIG];
            ResourceBox requirements = config.GetPropertyResourceBox(building.Name, Building.BUILDING_REQUIREMENTS);

            info += requirements.Cap + " ";
            info += Enum.GetName(typeof(Resource), requirements.Type) + "\n";

            NotifyPropertyChange(UiHack.PROPERTY_BUILDING_TIP, new object[]{ info });
        }
    }
}
