using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;

public class AI : MonoBehaviour {

	//Initialisation
	ResourceBox resourcesAvailable;
	List<Tile> mapTilesAvailable;
	List<Tile> cityTilesAvailable;
	System.Random random1;
	double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int actionsTaken;
	bool turnFinished;

	void Start () {
		actionsTaken = 0;
		random1 = new Random ();
		mapTilesAvailable = MapGenerator.getTiles ();
		cityTilesAvailable = MapGenerator.getTiles (); //wrong - how is city boundary made?

	}

	//Main method to run in each turn
	private void takeTurn() {
		turnFinished = false;
		while (actionsTaken < 3) {
			surveyArea();
			if (GameRules.turn == 0) { /*CHECK: how are turns handled?*/
				placeTownHall ();
			}
			/*after hack: look for which resource is needed for best advancement - if/while statement*/
			/*after hack: look for which building is needed for best advancement based on that resource - if/while statement*/
			/*after hack: sort out looping so player doesn't place duplicate buildings if they aren't useful for progression*/

			Tile placeHouseTile = surveyArea (1); //survey for blank land
			placeBuilding(placeHouseTile, "house"); //place house
			Tile placeLumberTile = surveyArea (2); //survey for wood
			placeBuilding(placeLumberTile, "lumber"); //place lumber yard
		}
		turnFinished = true;
	}

	private void placeTownHall() {
		//iterate through tiles available to find area with good surrounding resources
		for(int col=0; col<mapTilesAvailable.GetLength(0); col++){
			for (int row = 0; row < mapTilesAvailable.GetLength(1); row++) {
				if((mapTilesAvailable[col][row].type == 1) && //if current tile is empty
					//check surrounding tiles for a tile that isn't blank
					((mapTilesAvailable[col-1][row-1].type != 1) && (mapTilesAvailable[col-1][row-1].type != 0)) ||
					((mapTilesAvailable[col-1][row].type !=1) && (mapTilesAvailable[col-1][row] != 0)) ||
					((mapTilesAvailable[col-1][row+1].type !=1) && (mapTilesAvailable[col-1][row+1] !=0)) ||
					((mapTilesAvailable[col][row-1].type !=1) && (mapTilesAvailable[col][row-1] !=0)) ||
					((mapTilesAvailable[col][row+1].type !=1) && (mapTilesAvailable[col][row+1] !=0)) ||
					((mapTilesAvailable[col+1][row-1].type !=1) && (mapTilesAvailable[col+1][row-1] !=0)) ||
					((mapTilesAvailable[col+1][row].type !=1) && (mapTilesAvailable[col+1][row].type !=0)) ||
					((mapTilesAvailable[col+1][row+1].type !=1) && (mapTilesAvailable[col+1][row+1].type !=0))
				){
					//CHECK this once tile/buliding class have been updated
					mapTilesAvailable[row][col].building = Building.townHall; 
				}
			}
		}
		actionsTaken++;
	}


	/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
	private Tile surveyArea(SpaceRace.PlayerTools.Resources toFind) {
		Tile placeOn = null;
		for(int col=0; col<mapTilesAvailable.GetLength(0); col++){
			for (int row = 0; row < mapTilesAvailable.GetLength(1); row++) {
				if(cityTilesAvailable[col][row].type == toFind){
					placeOn = cityTilesAvailable[col][row];
				}
			}
		}
		return placeOn;
	}

	private void placeBuilding(Tile tile, string buildingToPlace) {
		tile.building = Building.buildingToPlace; //TODO: correct when tile/building class has been updated
		actionsTaken++;
	}



	/*for after hack*/
	//	private Building<T> chooseBuilding() {
	//	}

	/*for after hack*/
	//	private ResourceBox getOpptResource(Player oppt) {
	//		return null;
	//	}

	/*for after hack*/
	//	private int getOpptWealth(Player oppt) {
	//		return null;
	//	}

	/*for after hack*/
	//	private ResourceBox trade(Player oppt) {
	//		actionsTaken++;
	//		return null;
	//	}

	/*for after hack, returns number of a type of building within the city to determine whether it should be built*/
	//	private int countConstruction(Building<T> toCount) {
	//		return null;
	//	}

	/*for after hack*/
	/*returns a list of resources with their priority for advancement in the game*/
	//	private List<Resources> checkPriority(){
	//	}

	/*for after hack*/
	//	private bool rocketLaunch(int hydrogen){
	//		actionsTaken++;
	//		return null;
	//	}

}

