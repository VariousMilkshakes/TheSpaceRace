using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Utils;
using SpaceRace;
using SpaceRace.PlayerTools;

using UnityEngine.UI;
using SpaceRace.World.Buildings;
using UnityEngine.Rendering;

namespace SpaceRace.Utils
{
	public class UiHack : MonoBehaviour
	{
		#region Property Flags
		public const int PROPERTY_RESOURCE_TRACKERS = 01;
		public const int PROPERTY_CLEAR_BUILDINGS = 02;
		#endregion

		public static GameObject BUILDING_ITEM_TEMPLATE
		{
			get
			{
				return building_item_template;
			}

			set
			{
				building_item_template = building_item_template == null
					? value
					: building_item_template;
			}
		}

	    public static ErrorHandler ERROR
	    {
	        get { return error_handler; }
	        set
	        {
	            error_handler = error_handler == null
	                ? value
	                : error_handler;
	        }
	    }

		private static GameObject building_item_template;
        private static ErrorHandler error_handler;

        public GameObject Canvas;
		public GameObject AdvanceAge;
		public GameObject AdvanceAgeNotice;
		public GameObject TurnPlayerTracker;
		public GameObject BuildingButtonHolder;
	    public GameObject ErrorAlert;

		private List<GameObject> activeUiItems;
	    private UIController controller;

		#region ResourceTrackers
		public GameObject WoodTracker;
		public GameObject PopTracker;
		public GameObject MoneyTracker;

		private Dictionary<Resource, Text> trackers;
		#endregion


		// Use this for initialization
		void Start()
		{
		    ERROR = ErrorAlert.GetComponent<ErrorHandler>();
			UiHack.BUILDING_ITEM_TEMPLATE = BuildingButtonHolder;
			activeUiItems = new List<GameObject>();
			//Text currentText = WoodTracker.GetComponent<Text>();
			//currentText.text = "" + currentPlayer.Inventory.CheckResource(PlayerTools.Resource.Wood);

			trackers = new Dictionary<Resource, Text>();
			trackers.Add(Resource.Money, MoneyTracker.GetComponent<Text>());
			trackers.Add(Resource.Population, PopTracker.GetComponent<Text>());
			trackers.Add(Resource.Wood, WoodTracker.GetComponent<Text>());
			
			AdvanceAge.SetActive(false);
		}

        /// <summary>
        /// Called at the start of players turn to begin
        /// tracking player
        /// </summary>
        /// <param name="playerController">Current player's UI controller</param>
		public void BindTo(UIController playerController)
		{
			controller = playerController;

			controller.PropertyUpdateEvent.AddListener(updateHandler);
			controller.ResourceUpdate();

            setPlayer();
		}

        /// <summary>
        /// Called at the end of players phase in order to
        /// switch to next player
        /// </summary>
		public void UnbindFrom()
		{
			controller.PropertyUpdateEvent.RemoveAllListeners();
		}

		private void updateHandler(int propertyFlag, object[] updateArgs)
		{
			switch (propertyFlag)
			{
				case PROPERTY_RESOURCE_TRACKERS:
					UpdateResource(updateArgs[0], updateArgs[1]);
					break;
				case PROPERTY_CLEAR_BUILDINGS:
					ClearBuildingMenu();
					break;
			}
		}

		/// <summary>
		/// Called everytime player resources change
		/// </summary>
		public void UpdateResource(object updatedResource, object newValue)
		{
			Resource resource = (Resource) updatedResource;
			int value = (int) newValue;

			if (!trackers.ContainsKey(resource)) return;

			string current = trackers[resource].text;
			string label = current.Split(':')[0];
			trackers[resource].text = label + ": " + value;
		}

		/// <summary>
		/// Display menu of buildings available on the tile
		/// </summary>
		/// <param name="targetTile"></param>
		public void DisplayBuildings (Tile targetTile)
		{
			ClearBuildingMenu();

			if (targetTile.type == 1) return;

			List<Type> buildingTypes;
			if (targetTile.Building != null && targetTile.Building.Upgradeable)
			{
				buildingTypes = GameRules.GET_BUILDING_UPGRADES_FOR(targetTile.Building.GetType());
			}
			else
			{
				buildingTypes = controller.GetValidBuildings(targetTile);
			}

		    float panelWidth = 500f;
		    float startX = Canvas.transform.position.x;
			float xPos = startX - panelWidth / 2;
			float yPos = 30;
			float xSpacing = 10f;
		    float ySpacing = 30f;
			int buttonSize = 64;
			
			foreach (Type building in buildingTypes)
			{
				GameObject menuItem = controller.CreateBuildingButton(building, buttonSize, targetTile);
				menuItem.transform.SetParent(Canvas.transform);
				menuItem.transform.position = new Vector3(xPos, yPos);
                var text = menuItem.transform.GetChild(0);
			    text.GetComponent<Text>().text = building.Name;
			    float scale = 0.5f;
			    menuItem.transform.localScale = new Vector3(scale, scale);
				activeUiItems.Add(menuItem);

				xPos += buttonSize * scale + xSpacing;

			    if (xPos + (buttonSize*scale) >= startX + panelWidth / 2)
			    {
			        xPos = startX;
			        yPos += ySpacing + (buttonSize*scale);
			    }
			}
		}

		public void DisplayAdvanceButton ()
		{
			AdvanceAge.SetActive(true);
		}

		public void AdvancePlayer ()
		{
			GameObject notice = Instantiate(AdvanceAgeNotice, new Vector3(100, 100), Canvas.transform.rotation) as GameObject;
			notice.transform.SetParent(Canvas.transform);

			controller.AdvancePlayerAge();
		}

		public void ClearBuildingMenu ()
		{
			foreach (GameObject uiObject in activeUiItems)
			{
				Destroy(uiObject.gameObject);
			}

			activeUiItems.Clear();
		}

		private void setPlayer ()
		{
			Text display = TurnPlayerTracker.GetComponent<Text>();
			display.text = "Turn " + controller.Player.Turn + " Player " +
                            controller.Player.PlayerName;
		}
	}
}
