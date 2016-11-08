using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using SpaceRace;


//Tiles don't currently hold their own position (errors)
//Check actionsTaken increment 

public class AI : MonoBehaviour {

	//Initialisation
	ResourceBox resourcesAvailable;
	List<Tile> mapTilesAvailable;
	List<Tile> cityTilesAvailable;
	System.Random random1;
	double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int turn;
	int actionsTaken;
	bool turnFinished;
	MapGenerator mapgen;

	void Start () {
		actionsTaken = 0;
		turn = 0;
		random1 = new System.Random ();
		mapTilesAvailable = mapgen.getTiles ();
		cityTilesAvailable = mapgen.getTiles (); //after hack: change to tiles inside city boundary, currently all tiles on map
		takeTurn();
	}
		

	/******************ignore
	 * Taking a turn:
	 * first turn: build town hall, for other turns:
	 * check resources - which is a priority to increase?
	 * how do I increase that resource? - construction, trade, company? 
	 * 				--> depends on that resource (each resource has if statement which takes to the relevant method)
	 * if no money to pay for buildings/no relevant tiles to place buildings on, do i have faith?
	 * is my opponent significantly ahead of me?
	 * take the best action
	 *******************/

	//Main method to run in each turn
	private void takeTurn () {
		turnFinished = false;
		while (actionsTaken < 3) {
			surveyArea (SpaceRace.PlayerTools.Resources.Free);
			if (turn == 0) { /*after hack: how should turns be handled?*/
				placeTownHall ();
			}
			/*after hack: sort out looping so player doesn't place duplicate buildings if they aren't useful for progression*/

			Tile placeHouseTile = surveyArea (SpaceRace.PlayerTools.Resources.Free); //survey for blank land
			placeBuilding (placeHouseTile, "house"); //place house
			Tile placeLumberTile = surveyArea (SpaceRace.PlayerTools.Resources.Wood); //survey for wood
			placeBuilding (placeLumberTile, "lumber"); //place lumber yard
		}
		turnFinished = true;
		turn++;
	}

	private void placeTownHall () {
		//iterate through tiles available to find area with good surrounding resources
		for (int col=0; col<mapTilesAvailable.Count; col++){
			for (int row = 0; row < mapTilesAvailable.Count; row++) {
				if ((mapTilesAvailable[col][row].type == 1) && //if current tile is empty
					//check surrounding tiles for a tile that isn't blank
					//!!!!!!!shouldn't be mapTilesAvailable, should be the tile in that position
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
					placeBuilding (mapTilesAvailable[col][row], "townHall"); 
				}
			}
		}
		actionsTaken++;
	}


	/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
	private Tile surveyArea (SpaceRace.PlayerTools.Resources toFind) {
		Tile placeOn = null;
		for (int col=0; col<mapTilesAvailable.Count; col++){
			for (int row = 0; row < mapTilesAvailable.Count; row++) {
				//tile in that position, not cityTilesAvaiable
				if (cityTilesAvailable[col][row].type == toFind){
					placeOn = cityTilesAvailable[col][row];
				}
			}
		}
		return placeOn;
	}

	//place chosen building on specific tile
	private void placeBuilding (Tile tile, string buildingToPlace) {
		if (tile != null) {
			Type building = Game.LOOK_FOR_BUILDING (buildingToPlace);
			tile.Build (building);
			actionsTaken++;
		}
	}



	/**************************************************************************************************************ignore*/

	/*checks which building is best to construct next
	 * if no building is best, returns null - make a move other than constructing a building
	 * private String constructionCheck() {
	 * buildingToConstruct = null;
	 * check priority of resources
	 * check building which will best gather those resources
	 * check money - can I afford the best solution? if not, go lower
	 * if no money - building to construct is null
	 * 
	 * return buildingToConstruct;


	/* checks company status and assesses whether interaction should be initiated
	 * private void companyCheck(){
	 * call only if after companies have been introduced to game
	 * iterate through companies
	 * does opponent have company?
	 * will company be beneficial?
	 * how many resources needed to attract company?
	 * initiate interaction with company?
		}
	*/

	/* checks amount of resources aquired and which resource is needed
	changes priority of that resource accordingly
	private Resource setResourcePriority(){
	}
	*/

	/* checks priority of each resource and chooses next action depending on the resources
	private void resourceCheck() {
		check priority of resources
		which do I need to increase for best advancement in the game?
		how will I increase that resource? -> run method which will act on this
	}
	*/


	/*for after hack, returns number of a type of building within the city
	helps in deciding on whether to build a building in a turn*/
	//	private int countConstruction(Building<T> toCount) {
	//		iterate through all tiles inside the city, check the sprite on each tile and which building it corresponds to
	//		return null;
	//	}


	/*for after hack*/
	/*returns a list of resources with their priority for advancement in the game*/
	//	private List<Resources> checkPriority(){
	//	check goal for current turn
	//	}


	/*for after hack*/
	//	private ResourceBox getOpptResource(Player oppt, Resource resource) {		
	//		return null;
	//	}


	/*for after hack*/
	//	private ResourceBox trade(Player oppt) {
	//		actionsTaken++;
	//		return null;
	//	}


	/*for after hack*/
	//	private bool rocketLaunch(int hydrogen){
	//		actionsTaken++;
	//		return null;
	//	}

}

