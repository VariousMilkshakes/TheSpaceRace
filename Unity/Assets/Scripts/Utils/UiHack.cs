using UnityEngine;
using System.Collections.Generic;
using System;
using Assets.Scripts.Utils;
using SpaceRace.Game;
using SpaceRace.PlayerTools;
using SpaceRace.World;
using UnityEngine.UI;
using SpaceRace.World.Buildings;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using SpaceRace.World.Disasters;

namespace SpaceRace.Utils
{
	public class UiHack : MonoBehaviour
	{
		#region Property Flags
		public const int PROPERTY_RESOURCE_TRACKERS = 01;
		public const int PROPERTY_CLEAR_BUILDINGS = 02;
	    public const int PROPERTY_BUILDING_TIP = 03;
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
	    public GameObject CommandInput;
	    public GameObject InfoPanel;
	    public GameObject BuildingPanel;

		private GameObject DisasterPrefab;

		private List<GameObject> activeUiItems;
	    private UIController controller;

	    private Tile selectedTile;

	    private bool inputOpen = false;

        #region ResourceTrackers
        public GameObject MoneyTracker;
		public GameObject PopTracker;
	    public GameObject FaithTracker;
		public GameObject FoodTracker;
        public GameObject WoodTracker;
		public GameObject StoneTracker;
	    public GameObject IronTracker;
	    public GameObject SteelTracker;
	    public GameObject TourismTracker;
	    public GameObject HydrogenTracker;

        private Dictionary<Resource, Text> trackers;
		#endregion

		void Awake(){
		}

		// Use this for initialization
		void Start()
		{
            ButtonHover.SHOW_TOOL_TIP(false);

            ERROR = ErrorAlert.GetComponent<ErrorHandler>();
			UiHack.BUILDING_ITEM_TEMPLATE = BuildingButtonHolder;
			activeUiItems = new List<GameObject>();
			//Text currentText = WoodTracker.GetComponent<Text>();
			//currentText.text = "" + currentPlayer.Inventory.CheckResource(PlayerTools.Resource.Wood);

			trackers = new Dictionary<Resource, Text>();
			trackers.Add(Resource.Money, MoneyTracker.GetComponent<Text>());
			trackers.Add(Resource.Population, PopTracker.GetComponent<Text>());
            trackers.Add(Resource.Faith, FaithTracker.GetComponent<Text>());
			trackers.Add(Resource.Food, FoodTracker.GetComponent<Text>());
			trackers.Add(Resource.Wood, WoodTracker.GetComponent<Text>());
			trackers.Add(Resource.Stone, StoneTracker.GetComponent<Text>());
            trackers.Add(Resource.Iron, IronTracker.GetComponent<Text>());
            trackers.Add(Resource.Steel, SteelTracker.GetComponent<Text>());
            trackers.Add(Resource.Tourism, TourismTracker.GetComponent<Text>());
            trackers.Add(Resource.Hydrogen, HydrogenTracker.GetComponent<Text>());

            AdvanceAge.SetActive(false);
		}

		public UIController GetController(){
			return controller;
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
            ClearBuildingMenu();
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
                case PROPERTY_BUILDING_TIP:
                    displayBuildingInfo(updateArgs[0]);
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

			trackers[resource].text = value + "";
		}

		/// <summary>
		/// Display menu of buildings available on the tile
		/// </summary>
		/// <param name="targetTile"></param>
		public void DisplayBuildings (Tile targetTile)
		{
		    selectedTile = targetTile;
			ClearBuildingMenu();

			List<Type> buildingTypes;
			if (targetTile.Building != null && targetTile.Building.Upgradeable)
			{
				buildingTypes = GameRules.GET_BUILDING_UPGRADES_FOR(targetTile.Building.GetType());
			}
			else
			{
				buildingTypes = controller.GetValidBuildings(targetTile);
			}

		    float panelWidth = 700f;
		    float xPos = BuildingPanel.transform.position.x - panelWidth / 2;
		    float startX = xPos;
			float yPos = 30;
			float xSpacing = 30f;
		    float ySpacing = 30f;
			int buttonSize = 64;
			
			foreach (Type building in buildingTypes)
			{
				GameObject menuItem = controller.CreateBuildingButton(building, buttonSize, targetTile);
				menuItem.transform.SetParent(BuildingPanel.transform);
				menuItem.transform.position = new Vector3(xPos, yPos);
                var text = menuItem.transform.GetChild(0);
			    text.GetComponent<Text>().text = building.Name;
			    float scale = 0.5f;
			    menuItem.transform.localScale = new Vector3(scale, scale);
				activeUiItems.Add(menuItem);

				xPos += buttonSize * scale + xSpacing;

			    if (xPos + (buttonSize*scale) >= startX + panelWidth)
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
            controller.AdvancePlayerAge();
		    string advanceMessage = "You have advanced to the \n" +
		                            Enum.GetName(typeof(WorldStates), controller.Player.Age) +
		                            " Age";
			ERROR.Handle(new PlayerException(advanceMessage));
            AdvanceAge.SetActive(false);
		}

		public void ClearBuildingMenu ()
		{
            ButtonHover.SHOW_TOOL_TIP(false);

			foreach (GameObject uiObject in activeUiItems)
			{
				Destroy(uiObject.gameObject);
			}

			activeUiItems.Clear();
            BuildingPanel.SetActive(true);
		}

		private void setPlayer ()
		{
			Text display = TurnPlayerTracker.GetComponent<Text>();
			display.text = "Turn " + controller.Player.Turn + " Player " +
                            controller.Player.Name;
		}

	    public void CastNaturalDisaster ()
	    {
			string prefabName = EventSystem.current.currentSelectedGameObject.name;
			Debug.Log (prefabName);

			/*try{*/

			controller.Cast(selectedTile, prefabName);
			/*}catch(Exception e){
				Debug.Log (e.ToString());
			}*/
	    }

	    public void DisplayToolTop (Type building)
	    {
	        controller.FetchBuildingInfo(building);
	    }

	    public void ClearToolTip ()
	    {
            Text infoText = InfoPanel.GetComponent<Text>();
            infoText.text = "";
        }

	    private void displayBuildingInfo (object info)
	    {
	        Text infoText = InfoPanel.GetComponent<Text>();
	        infoText.text = (string)info;
	    }

	    void Update ()
	    {
	        if (Input.GetKeyDown(KeyCode.F8)) { 
//	            && Input.GetKeyDown(KeyCode.LeftShift)) {
	            CommandInput.SetActive(!inputOpen);
	            inputOpen = !inputOpen;
	        }
	    }

	    public void CompleteTurn ()
	    {
            controller.Player.TurnComplete = true;
        }
	}
}
