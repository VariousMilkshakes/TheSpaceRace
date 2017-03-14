using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceRace.PlayerTools;

public class MarketManager : MonoBehaviour {

	private Dictionary<Resource, Text> trackers;
	private MarketController controller;

	//Player's Balance(money) Tracker in Market
	public GameObject BalanceTracker;

	//Market Stock Tracker
	public GameObject FoodStockTracker;
	public GameObject WoodStockTracker;
	public GameObject StoneStockTracker;
	public GameObject IronStockTracker;
	public GameObject CoalStockTracker;
	public GameObject HayStockTracker;

	//Market Buy Price Tracker
	public GameObject FoodBuyPriceTracker;
	public GameObject WoodBuyPriceTracker;
	public GameObject StoneBuyPriceTracker;
	public GameObject IronBuyPriceTracker;
	public GameObject CoalBuyPriceTracker;
	public GameObject HayBuyPriceTracker;

	//Market Sell Price Tracker
	public GameObject FoodSellPriceTracker;
	public GameObject WoodSellPriceTracker;
	public GameObject StoneSellPriceTracker;
	public GameObject IronSellPriceTracker;
	public GameObject CoalSellPriceTracker;
	public GameObject HaySellPriceTracker;

	//Market BuyButton Object
	public GameObject FoodBuyButton;
	public GameObject WoodBuyButton;
	public GameObject StoneBuyButton;
	public GameObject IronBuyButton;
	public GameObject CoalBuyButton;
	public GameObject HayBuyButton;

	//Market SellButton Object
	public GameObject FoodSellButton;
	public GameObject WoodSellButton;
	public GameObject StoneSellButton;
	public GameObject IronSellButton;
	public GameObject CoalSellButton;
	public GameObject HaySellButton;

	//Market Resource Display
	List<Text> stockTrackers = new List<Text>();
	List<Text> BuyPriceTrackers = new List<Text>();
	List<Text> SellPriceTrackers = new List<Text>();
	List<Resource> ResourceIndex = new List<Resource>();


	// Use this for initialization
	void Start () {
		
		controller = new MarketController();

		//Text componet on the market display
		stockTrackers.Add (FoodStockTracker.GetComponent<Text>());
		stockTrackers.Add (WoodStockTracker.GetComponent<Text>());
		stockTrackers.Add (StoneStockTracker.GetComponent<Text>());
		stockTrackers.Add (IronStockTracker.GetComponent<Text>());
		stockTrackers.Add (CoalStockTracker.GetComponent<Text>());
		stockTrackers.Add (HayStockTracker.GetComponent<Text>());

		ResourceIndex.Add (Resource.Food);
		ResourceIndex.Add (Resource.Wood);
		ResourceIndex.Add (Resource.Stone);
		ResourceIndex.Add (Resource.Iron);
		ResourceIndex.Add (Resource.Coal);
		ResourceIndex.Add (Resource.Hay);

		stockTrackers.Add (FoodBuyPriceTracker.GetComponent<Text>());
		stockTrackers.Add (WoodBuyPriceTracker.GetComponent<Text>());
		stockTrackers.Add (StoneBuyPriceTracker.GetComponent<Text>());
		stockTrackers.Add (IronBuyPriceTracker.GetComponent<Text>());
		stockTrackers.Add (CoalBuyPriceTracker.GetComponent<Text>());
		stockTrackers.Add (HayBuyPriceTracker.GetComponent<Text>());

		stockTrackers.Add (FoodSellPriceTracker.GetComponent<Text>());
		stockTrackers.Add (WoodSellPriceTracker.GetComponent<Text>());
		stockTrackers.Add (StoneSellPriceTracker.GetComponent<Text>());
		stockTrackers.Add (IronSellPriceTracker.GetComponent<Text>());
		stockTrackers.Add (CoalSellPriceTracker.GetComponent<Text>());
		stockTrackers.Add (HaySellPriceTracker.GetComponent<Text>());

	}

	public void updateStock(){
		int i;
		for(i=0, i<7, i++){
			stockTrackers[i] = cotroller.checkStock(Target);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
