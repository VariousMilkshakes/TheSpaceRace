using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	//Initialisation
	ResourceBox resourcesAvailable;
	MapGenerator.tiles mapTilesAvailable;
	/*Change to correct reference once other class is finished*/MapGenerator.tiles cityTilesAvailable;
	Random random1;
	double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int actionsTaken;
	bool turnFinished;

	void Start () {
		actionsTaken = 0;
		random1 = new Random ();
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
			
			Tile[][] placeHouseTile = surveyArea (1); //survey for blank land
			placeBuilding(placeHouseTile, "house"); //place house
			Tile[][] placeLumberTile = surveyArea (2); //survey for wood
			placeBuilding(placeLumberTile, "lumber"); //place lumber yard
		}
		turnFinished = true;
	}

	private void placeTownHall() {
		//iterate through tiles available to find area with good surrounding resources
		for(int col=0; col<mapTilesAvailable[0].Length; col++){
			for (int row = 0; row < mapTilesAvailable.Rank; row++) {
				//TODO: make this more useful														**************!**************
				if((mapTilesAvailable[row][col].type && (mapTilesAvailable[row-1][col].type
					|| mapTilesAvailable[row][col+1].type || mapTilesAvailable[row+1][col+1].type 
					|| mapTilesAvailable[row][col-1).type) != 1){
						//CHECK this once tile/buliding class have been updated
						mapTilesAvailable[row][col].building = Building.townHall; 
					}
				}
			}
		actionsTaken++;
	}


	/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
	private Tile[][] surveyArea(Resources toFind) {
		Tile[][] placeOn = null;
		//TODO: make this more useful																**************!************
		for(int col=0; col<cityTilesAvailable[0].Length; col++){
			for (int row = 0; row < cityTilesAvailable.Rank; row++) {
				if(cityTilesAvailable[row][col].type == toFind){
					placeOn = cityTilesAvailable[row][col];
				}
			}
		}
		return placeOn;
	}

	/*Search for a specific resource within the available area
	Returns tile containing that resource*/
	private bool searchForResource(Resources toFind) {
		Tile[][] hasResource = null;
		//TODO: make this more useful																************!***********
		for(int col=0; col<cityTilesAvailable[0].Length; col++){
			for (int row = 0; row < cityTilesAvailable.Rank; row++) {
				if(tilesAvailable[row][col]./*TODO: find resource of that tile*/ == toFind){
					hasResource = cityTilesAvailable[row][col];
				}
			}
		}
		return hasResource;			
	}

	private void placeBuilding(Tile[][] tile, String buildingToPlace) {
		tile.building = Building.buildingToPlace; //TODO: correct when tile/building class has been updated
		actionsTaken++;
	}



	/*for after hack*/
	private Building chooseBuilding() {
	}

	/*for after hack*/
	private ResourceBox getOpptResource(Player oppt) {
		return null;
	}

	/*for after hack*/
	private int getOpptWealth(Player oppt) {
		return null;
	}

	/*for after hack*/
	private ResourceBox trade(Player oppt) {
		actionsTaken++;
		return null;
	}

	/*for after hack, returns number of a type of building within the city to determine whether it should be built*/
	private int countConstruction(Building toCount) {
		return null;
	}

	/*for after hack*/
	private bool rocketLaunch(int hydrogen){
		actionsTaken++;
		return null;
	}

}

