using UnityEngine;
using System.Collections.Generic;
using System;

using SpaceRace;
using SpaceRace.PlayerTools;

using UnityEngine.UI;
using SpaceRace.World.Buildings;

namespace SpaceRace.Utils
{
	public class UiHack : MonoBehaviour
	{

		public GameObject BuildingMenuItem;
		public GameObject Canvas;
		public GameObject AdvanceAge;
		public GameObject AdvanceAgeNotice;
		public GameObject TurnPlayerTracker;
		
		private Player currentPlayer;

		private List<GameObject> activeUIItems;

		#region ResourceTrackers
		public GameObject WoodTracker;
		public GameObject PopTracker;
		public GameObject MoneyTracker;

		private Text woodValue;
		private Text popValue;
		private Text moneyValue;
		#endregion


		// Use this for initialization
		void Start()
		{
			activeUIItems = new List<GameObject>();
			//Text currentText = WoodTracker.GetComponent<Text>();
			//currentText.text = "" + currentPlayer.Inventory.CheckResource(PlayerTools.Resources.Wood);
			moneyValue = MoneyTracker.GetComponent<Text>();
			popValue = PopTracker.GetComponent<Text>();
			woodValue = WoodTracker.GetComponent<Text>();
			AdvanceAge.SetActive(false);
		}

		/// <summary>
		/// Called everytime player resources change
		/// </summary>
		public void ResourceUpdate()
		{
			Inventory inv = currentPlayer.Inventory;
			moneyValue.text = "Money: " + inv.CheckResource(PlayerTools.Resources.Money);
			popValue.text = "Population: " + inv.CheckResource(PlayerTools.Resources.Population);
			woodValue.text = "Wood: " + inv.CheckResource(PlayerTools.Resources.Wood);
		}


		/// <summary>
		/// At the start of the turn bind to the current player
		/// </summary>
		/// <param name="targetPlayer">Player to bind to</param>
		public void BindPlayer (Player targetPlayer)
		{
			currentPlayer = targetPlayer;
			currentPlayer.Inventory.AddListener(ResourceUpdate);
		}

		/// <summary>
		/// Un bind from previous player in order to bind to new player
		/// </summary>
		public void UnBindPlayer ()
		{
			currentPlayer.Inventory.ClearListeners();
		}

		/// <summary>
		/// Display menu of buildings available on the tile
		/// </summary>
		/// <param name="targetTile"></param>
		public void DisplayBuildings (Tile targetTile)
		{
			clearBuildingMenu();

			if (targetTile.type == 1 || targetTile.Building != null) return;

			int xPos = 200;
			int yPos = 50;
			int xSpacing = 10;

			int buttonSize = 64;

			foreach (Type building in Game.BUILDING_REPO)
			{
				GameObject menuItem = Instantiate(BuildingMenuItem, new Vector3(xPos, yPos), BuildingMenuItem.transform.rotation) as GameObject;
				Button menuButton = menuItem.GetComponent<Button>();

				Type targetBuilding = building;
				menuButton.onClick.AddListener(delegate
				{
					var builder = currentPlayer;
					
					if (targetTile.Build(targetBuilding, builder))
						targetTile.ApplyPlayerColor(builder.Color);

					clearBuildingMenu();
				});

				/// Set sprite for building button
				try
				{
					Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
					string spritePath = buildingConfigs.LookForProperty(building.Name, "Sprite.All").Value;
					Texture2D rawSprite = UnityEngine.Resources.Load(spritePath, typeof(Texture2D)) as Texture2D;
					menuButton.image.sprite = Sprite.Create(rawSprite,
						new Rect(0, 0, buttonSize, buttonSize),
						new Vector2());
				}
				catch (Exception e)
				{
					Debug.Log(e);
				}

				menuItem.transform.SetParent(Canvas.transform);

				xPos += buttonSize + xSpacing;

				activeUIItems.Add(menuItem);
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
			currentPlayer.Age = World.WorldStates.Viking;
		}

		private void clearBuildingMenu ()
		{
			foreach (GameObject uiObject in activeUIItems)
			{
				Destroy(uiObject.gameObject);
			}

			activeUIItems.Clear();
		}

		public void SetTurn (int turn, Player activePlayer)
		{
			UnBindPlayer();
			BindPlayer(activePlayer);

			Text display = TurnPlayerTracker.GetComponent<Text>();
			display.text = "Turn " + turn + " Player " + currentPlayer.PlayerName;
		}
	}
}
