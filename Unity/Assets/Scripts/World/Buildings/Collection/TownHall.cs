using System;
using SpaceRace.World.Buildings;
using SpaceRace.PlayerTools;
using UnityEngine;
using SpaceRace.Utils;
using System.Collections.Generic;

namespace SpaceRace.World.Buildings.Collection
{
	public class TownHall : Building
	{
		MapGenerator mapGen;
		Player player;

		public TownHall ()
		{
			Sprite sprite = null;
			setCityTiles ();
			mapGen = GameObject.FindGameObjectWithTag("PlaneManager")
				.GetComponent<MapGenerator>();
			

			/// Set sprite for building
			try
			{
				Config buildingConfigs = GameRules.CONFIG_REPO["Buildings"];
				string spritePath = buildingConfigs.LookForProperty("TownHall", "Sprite.All").Value;
				sprite = UnityEngine.Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
			_buildingSprites.Add(WorldStates.All, sprite);

			//Input = new PT.ResourceBox(PT.Resources.Free, 0, 0);
			//Output = new PT.ResourceBox(PT.Resources.Money, 0, 10);
		}

		public override SpaceRace.PlayerTools.ResourceBox BuildRequirements()
		{
			return new SpaceRace.PlayerTools.ResourceBox(SpaceRace.PlayerTools.Resources.None);
		}

		public override void OnTurn()
		{
			base.OnTurn();
		}

		/*find position of this player's town hall*/
		private Tile findTownHall(){
			List<Tile> toSearch = mapGen.tiles;
			for(int i = 0; i<toSearch.Count; i++){
				if(toSearch[i].Building == TownHall /*&& tile belongs to this player*/){
					return toSearch [i];
				}
			}
		}

		/*set tiles surrounding town hall to the player's colour*/
		private void setCityTiles(){
			Tile townHallPos = findTownHall ();


		}

		private void expandCityBoundary(){
		}
	}
}

