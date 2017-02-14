﻿using UnityEngine;
using System;
using SpaceRace;
using SpaceRace.PlayerTools;
using System.Collections.Generic;
using SpaceRace.World.Buildings.Collection;

/********Problems/plan for this class
 Tiles don't currently hold their own position (errors)
 Check actionsTaken increment 
 after hack: sort out looping so player doesn't place duplicate buildings if they aren't useful for progression
 * Taking a turn:
 * first turn: build town hall, for other turns:
 * check resources - which is a priority to increase?
 * how do I increase that resource? - construction, trade, company? 
 * 				--> depends on that resource (each resource has if statement which takes to the relevant method)
 * if no money to pay for buildings/no relevant tiles to place buildings on, do i have faith?
 * is my opponent significantly ahead of me?
 * take the best action
 *******************/

public class AI : Player {
	//Initialisation
	ResourceBox resourcesAvailable;
	System.Random random1;
    TileFlag[,] mapTilesAvailable;
    List<Tile> cityTilesAvailable;
    double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int turn;
	int actionsTaken;
	bool turnFinished;
	MapGenerator mapGen;

	public AI (Player player) {
		mapGen = GameObject.FindGameObjectWithTag("PlaneManager")
			.GetComponent<MapGenerator>();
        mapTilesAvailable = mapGen.GetGridPos();
        playerAI = player;
		actionsTaken = 0;
		turn = 0;
		random1 = new System.Random ();
	}

	public void OnTurn () {

		playerAI.OnTurn ();
		turnFinished = false;
		actionsTaken = 0;

        //If the town hall has been placed, update the tiles inside cityTilesAvailable
        if (GetPlayerBuildings().Exists(x => x.GetType().Equals("TownHall")))
        {
            TownHall townHall = (TownHall)GetPlayerBuildings().Find(x => x.GetType().Equals("TownHall"));
            cityTilesAvailable = townHall.GetCityTiles();
        }

        //Take three turns in this turn
        while (actionsTaken < 3) {
            //For the first turn, the AI must place a town hall
			if (turn == 0) { 
				placeTownHall ();
                actionsTaken++;
			}

		//	Tile placeHouseTile = surveyArea (SpaceRace.PlayerTools.Resource.Free); //survey for blank land
//			placeBuilding (mapGen.GetTile(5,5), "House"); //place house
			if (turn == 0) {
				mapGen.GetTile (1, 1).Build (Game.LOOK_FOR_BUILDING ("House"), playerAI);
				actionsTaken++;
			}
			if (turn == 1) {
				mapGen.GetTile (2, 3).Build (Game.LOOK_FOR_BUILDING ("House"), playerAI);
				actionsTaken++;
			}
			if (turn == 2) {
				mapGen.GetTile (6, 6).Build (Game.LOOK_FOR_BUILDING ("House"), playerAI);
				actionsTaken++;
			}
		//	Tile placeLumberTile = surveyArea (SpaceRace.PlayerTools.Resource.Wood); //survey for wood
		//	placeBuilding (placeLumberTile, "LumberYard"); //place lumber yard
		}
		turnFinished = true;
	//	turn++;
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
					placeBuilding (mapGen.GetTile (col, row), "TownHall"); 
				}
			}
		}
		actionsTaken++;
	}



	/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
/*	private Tile surveyArea (SpaceRace.PlayerTools.Resource toFind) {
		Tile placeOn = null;
		for (int col=0; col<mapTilesAvailable.GetLength(0); col++){
			for (int row = 0; row < mapTilesAvailable.GetLength(1); row++) {
				//tile in that position, not cityTilesAvaiable
				if (mapGen.GetTile(col,row).type.Equals(toFind)){ /// Tile does not yet use resource types
					placeOn = mapGen.GetTile(col,row);
					return placeOn;
				}
			}
		}
		return placeOn;
	}
*/
	//place chosen building on specific tile
	private void placeBuilding (Tile tile, string buildingToPlace) {
	//	if (tile != null) {
			Type building = Game.LOOK_FOR_BUILDING (buildingToPlace);
			tile.Build (building, playerAI);
			actionsTaken++;
	//	}
	}



	/*****************************************************************************************************planning

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
	

		/* checks amount of resources aquired and which resource is needed
		changes priority of that resource accordingly
		private Resource setResourcePriority(){
		}
		

		/* checks priority of each resource and chooses next action depending on the resources
		private void resourceCheck() {
			check priority of resources
			which do I need to increase for best advancement in the game?
				how will I increase that resource? -> run method which will act on this
			}
		


		/*for after hack, returns number of a type of building within the city
		helps in deciding on whether to build a building in a turn
		//	private int countConstruction(Building<T> toCount) {
		//		iterate through all tiles inside the city, check the sprite on each tile and which building it corresponds to
		//		return null;
		//	}


		/*for after hack
		/*returns a list of resources with their priority for advancement in the game
		//	private List<Resource> checkPriority(){
		//	check goal for current turn
		//	}


		/*for after hack
		//	private List<Resource> getOpptResource(Player oppt, Resource resourceToFind) {		
		//		return null;
		//	}


		/*for after hack
		//	private void trade(Player oppt, Resource resourceToTrade) {
		//		actionsTaken++;
		//		return null;
		//	}


		/*for after hack
		//	private bool rocketLaunch(int hydrogen){
		//		how is this initiated?
		//		actionsTaken++;
		//		return null;
		//	}*/

	}

