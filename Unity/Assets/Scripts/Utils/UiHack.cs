using UnityEngine;
using System.Collections.Generic;
using System;

using SpaceRace;
using UnityEngine.UI;

namespace SpaceRace.Utils
{
	public class UiHack : MonoBehaviour
	{

		public GameObject BuildingMenuItem;
		public GameObject Canvas;

		private List<GameObject> activeUIItems;


		// Use this for initialization
		void Start()
		{
			activeUIItems = new List<GameObject>();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void DisplayBuildings (Tile targetTile)
		{
			clearBuildingMenu();

			int xPos = 40;
			int yPos = 25;

			foreach (Type building in Game.BUILDING_REPO)
			{
				GameObject menuItem = Instantiate(BuildingMenuItem, new Vector3(xPos, yPos), BuildingMenuItem.transform.rotation) as GameObject;
				Button menuButton = menuItem.GetComponent<Button>();
				menuButton.onClick.AddListener(delegate
				{
					targetTile.Build(building);
				});

				try
				{
					Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
					string spritePath = buildingConfigs.LookForProperty(building.Name, "Sprite.All").Value;
					Sprite rawSprite = Resources.Load(spritePath) as Sprite;
					menuButton.image.sprite = rawSprite;
				}
				catch (Exception e)
				{
					Debug.Log(e);
				}

				menuItem.transform.SetParent(Canvas.transform);

				xPos += 50;

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
