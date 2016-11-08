using UnityEngine;
using System.Collections.Generic;
using System;

using SpaceRace;
using SpaceRace.PlayerTools;

using UnityEngine.UI;

namespace SpaceRace.Utils
{
	public class UiHack : MonoBehaviour
	{

		public GameObject BuildingMenuItem;
		public GameObject Canvas;
		
		public Player currentPlayer;

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
		}

		/// <summary>
		/// Called everytime player resources change
		/// </summary>
		void ResourceUpdate()
		{
			Inventory inv = currentPlayer.Inventory;
			moneyValue.text = "" + inv.CheckResource(PlayerTools.Resources.Money);
			popValue.text = "" + inv.CheckResource(PlayerTools.Resources.Population);
			woodValue.text = "" + inv.CheckResource(PlayerTools.Resources.Wood);
		}

		/// <summary>
		/// Display menu of buildings available on the tile
		/// </summary>
		/// <param name="targetTile"></param>
		public void DisplayBuildings (Tile targetTile)
		{
			clearBuildingMenu();

			if (targetTile.type == 1) return;

			int xPos = 40;
			int yPos = 40;
			int xSpacing = 10;

			int buttonSize = 64;

			foreach (Type building in Game.BUILDING_REPO)
			{
				GameObject menuItem = Instantiate(BuildingMenuItem, new Vector3(xPos, yPos), BuildingMenuItem.transform.rotation) as GameObject;
				Button menuButton = menuItem.GetComponent<Button>();
				menuButton.onClick.AddListener(delegate
				{
					targetTile.Build(building);
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

		private void clearBuildingMenu ()
		{
			foreach (GameObject uiObject in activeUIItems)
			{
				Destroy(uiObject.gameObject);
			}

			activeUIItems.Clear();
		}
	}
}
