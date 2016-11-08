using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace;
using SpaceRace.PlayerTools;
using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;


//Tiles don't currently hold their own position (errors)
//Check actionsTaken increment 

public class AI : TurnObject {

	//Initialisation
	ResourceBox resourcesAvailable;
	int[,] mapTilesAvailable;
	int[,] cityTilesAvailable;	//will be different once town hall has been written
	System.Random random1;
	double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int turn;
	int actionsTaken;
	bool turnFinished;
	MapGenerator mapGen;

	public AI (Player player) {
		actionsTaken = 0;
		turn = 0;
		random1 = new System.Random ();
		mapTilesAvailable = mapGen.getGridPos();
		cityTilesAvailable = mapGen.getGridPos (); //will be different once town hall has been written
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

	//Main GetTile to run in each turn
	public void OnTurn () {
		playerAI.OnTurn ();
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
		//iterate through tiles available to find area with surrounding resources
		for (int col = 0; col < mapTilesAvailable.GetLength (0); col++) {
			for (int row = 0; row < mapTilesAvailable.GetLength (1); row++) {
				//assign surrounding tiles a score (for hack, 1 - has a resource, 0 doesn't)
				//after loop - find surrounding area with largest score
				int score = 0;
				if (mapGen.GetTile (col, row).type > 1) {
					mapGen.GetTile(col, row).score += score;
				}
			}
		}

		for (int col = 0; col < mapTilesAvailable.GetLength (0); col++) {
			for (int row = 0; row < mapTilesAvailable.GetLength (1); row++) {
				int highScore = 0;
				if (mapGen.GetTile (col, row).type == 1) { //if current tile is empty
					//add together surrounding tiles' score
					int currentScore = mapGen.GetTile (col - 1, row - 1).score + mapGen.GetTile (col - 1, row).score +
						mapGen.GetTile (col - 1, row + 1).score + mapGen.GetTile (col, row - 1).score + 
						mapGen.GetTile (col, row + 1).score + mapGen.GetTile (col + 1, row - 1).score + 
						mapGen.GetTile (col + 1, row).score + mapGen.GetTile (col + 1, row + 1).score;
					if (currentScore > highScore) {
						highScore = currentScore;
					}
				}
				if (mapGen.GetTile (col, row).score == highScore) {
					placeBuilding (mapGen.GetTile (col, row), "townHall"); 
				}
			}
		}
		actionsTaken++;
	}



	/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
	private Tile surveyArea (SpaceRace.PlayerTools.Resources toFind) {
		Tile placeOn = null;
		for (int col=0; col<mapTilesAvailable.GetLength(0); col++){
			for (int row = 0; row < mapTilesAvailable.GetLength(1); row++) {
				//tile in that position, not cityTilesAvaiable
				if (mapGen.GetTile(col,row).type.Equals(toFind)){
					placeOn = mapGen.GetTile(col,row);
				}
			}
		}
		return placeOn;
	}

	//place chosen building on specific tile
	private void placeBuilding (Tile tile, string buildingToPlace) {
		if (tile != null) {
			Type building = Game.LOOK_FOR_BUILDING (buildingToPlace);
			tile.Build (building, playerAI);
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
		//	private List<Resources> getOpptResource(Player oppt, Resource resourceToFind) {		
		//		return null;
		//	}


		/*for after hack*/
		//	private void trade(Player oppt, Resource resourceToTrade) {
		//		actionsTaken++;
		//		return null;
		//	}


		/*for after hack*/
		//	private bool rocketLaunch(int hydrogen){
		//		how is this initiated?
		//		actionsTaken++;
		//		return null;
		//	}

	}

