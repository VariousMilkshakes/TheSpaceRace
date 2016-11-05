puusing UnityEngine;
using System.Collections;

public class AI : Player {

	//Initialisation
	Resource resourcesAvailable;
	int[columns,rows] tilesAvailable;
	Random random1;
	double rocketSuccessProb;
	Player playerAI;
	Player oppt;
	int actionsTaken;
	Inventory inventory;
	bool turnFinished;

	void Start () {
		actionsTaken = 0;
		random1 = new Random ();
		tilesAvailable = new int[8,8] {{0,2,3,4,2,4,2,2},
			{2,4,5,6,3,4,1,1}}; //test
	}

	//Main method to run in each turn
	private void takeTurn() {
		turnFinished = false;
		while (actionsTaken < 3) {
			surveyArea();
			if (turn == 0) { /*TODO: how are turns handled?*/
				placeTownHall ();
				actionsTaken++;
			}
			/*after hack: look for which resource is needed for best advancement - if/while statement*/
			/*after hack: look for which building is needed for best advancement based on that resource - if/while statement*/

			/*TODO: how to make sure player doesn't keep just placing houses on next turn if they run out of moves
			         - depends on how many moves per turn, is this for after the hack?*/
			Tile[][] placeHouse = surveyArea (/*TODO: resource to find is blank tile*/);
			placeBuilding(placeHouse);
			Tile[][] placeMine = surveyArea(/*TODO: resource to find is mine*/);
			placeBuilding(placeMine);


		}
		turnFinished = true;
	}

	private void placeTownHall() {
		//iterate through tiles available to find area with good surrounding resources
		for(int col=0; col<tilesAvailable[0].Length; col++){
			for (int row = 0; row < tilesAvailable.Rank; row++) {
				if((tilesAvailable[row][col].getTileType && (tilesAvailable[row-1][col].getTileType
					|| tilesAvailable[row][col+1].getTileType || tilesAvailable[row+1][col+1].getTileType 
					|| tilesAvailable[row][col-1).getTileType) != /*blank tile*/){
						//TODO: place town hall in tilesAvailable[row][col];
					}
						}
						}
						actionsTaken++;
						}


						/*Surveys tiles avilable to place a building
	 * Returns the tile to place the building on*/
						private Tile[][] surveyArea(Resources toFind) {
							Tile[][] placeOn = null;
							for(int col=0; col<tilesAvailable[0].Length; col++){
								for (int row = 0; row < tilesAvailable.Rank; row++) {
									if(tilesAvailable[row][col].getTileType == toFind){
										placeOn = tilesAvailable[row][col];
									}
								}
							}
							return placeOn;
						}

						/*Search for a specific resource within the available area
						Returns tile containing that resource*/
						private boolean searchForResource(Resources toFind) {
							Tile[][] hasResource = null;
							for(int col=0; col<tilesAvailable[0].Length; col++){
								for (int row = 0; row < tilesAvailable.Rank; row++) {
									if(tilesAvailable[row][col]./*TODO: find resource of that tile*/ == toFind){
										hasResource = tilesAvailable[row][col];
									}
								}
							}
							return hasResource;			
						}

						private void placeBuilding(Tile[][] placeHouse) {
							/*TODO: is this done in the player class?*/
							actionsTaken++;
						}





						/*for after hack*/
						private Building chooseBuilding() {
						}

						/*for after hack*/
						private Resources<> getOpptResource(Player oppt) {
							return null;
						}

						/*for after hack*/
						private int getOpptWealth(Player oppt) {
							return null;
						}

						/*for after hack*/
						private Resources<> trade(Player oppt) {
							return null;
						}

						/*for after hack*/
						private bool rocketLaunch(int hydrogen){
							return null;
						}

						}
