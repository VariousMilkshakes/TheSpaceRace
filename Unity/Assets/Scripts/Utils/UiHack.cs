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
					Texture2D rawSprite = Resources.Load(spritePath, typeof(Texture2D)) as Texture2D;
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
