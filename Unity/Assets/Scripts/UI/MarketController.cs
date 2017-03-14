using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;

public class MarketController{

	private Inventory stock;
	private float foodmodifier;


	// Use this for initialization
	void Start () {

		stock = new Inventory ();
		stock.AddResource (Resource.Food,500);
		stock.AddResource (Resource.Wood, 500);
		stock.AddResource (Resource.Coal, 500);
		stock.AddResource (Resource.Fish, 100);
		stock.AddResource (Resource.Hydrogen, 100);
		stock.AddResource (Resource.Iron, 500);
		stock.AddResource (Resource.Money, 500);
		stock.AddResource (Resource.Oil, 500);
		stock.AddResource (Resource.Steel, 500);
		stock.AddResource (Resource.Stone, 500);
		stock.AddResource (Resource.Straw, 500);
		//stock.AddResource (Resource.Faith, 100);
		//stock.AddResource (Resource.Population, 0);
	}

	public int checkStock(Resource targetResource){

		return stock.CheckResource();
	}


	public ResourceBox Buy(int quantity, Resource resource, ResourceBox playermoney){



		return 
	}

	public ResourceBox Sell(int quantity, Resource resource, ResourceBox playermoney){

		return
	}

	// Update is called once per frame
	void Update () {
		
	}
}
