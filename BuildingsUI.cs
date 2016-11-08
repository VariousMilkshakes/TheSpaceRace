using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BuildingsUI : MonoBehaviour {
	//initialising the components 
	public GameObject Wood;
	public GameObject Straw;
	public GameObject Iron;
	public GameObject Steel;
	public GameObject Stone;
	public GameObject Coal;
	public GameObject Oil;
	public GameObject Hydrogen;
	public GameObject Population;
	public GameObject Faith;
	public GameObject Era;
	public GameObject Score;
	public GameObject Money;
	// text fields holding the values of the components
	private Text woodText;
	private Text strawText;
	private Text ironText;
	private Text steelText;
	private Text stoneText;
	private Text coalText;
	private Text oilText;
	private Text hydroText;
	private Text popText;
	private Text faithText;
	private List <Text> list;
	private Text eraText;
	private Text scoreText;
	private Text moneyText;




	void Start(){
		//A list containing all of the resources found in the inventory
		list = new List <Text> (){
			woodText,strawText,ironText,steelText,stoneText,coalText,oilText,hydroText,popText,faithText
		};
		//setting initial values for the components
		moneyText = Money.GetComponent <Text> ();
		moneyText.text = "" + 100;
		scoreText = Score.GetComponent <Text> ();
		scoreText.text = "" + 100;
		eraText = Era.GetComponent<Text> ();
		eraText.text = "" + "Roman Era";
		woodText = Wood.GetComponent<Text> ();
		woodText.text = "" + 10;
		strawText = Straw.GetComponent<Text> ();
		strawText.text = "" + 10;
		ironText = Iron.GetComponent<Text> ();
		ironText.text = "" + 0;
		steelText = Steel.GetComponent<Text> ();
		steelText.text = "" + 0;
		stoneText = Stone.GetComponent<Text> ();
		stoneText.text = "" + 0;
		coalText = Coal.GetComponent<Text> ();
		coalText.text = "" + 0;
		oilText = Oil.GetComponent<Text> ();
		oilText.text = "" + 0;
		hydroText = Hydrogen.GetComponent<Text> ();
		hydroText.text = "" + 10;
		popText = Population.GetComponent<Text> ();
		popText.text = "" + 10;
		faithText = Faith.GetComponent<Text> ();
		faithText.text = "" + 10;
	
		
	}

	// Resource ids : 0-11
	void ResourceUpdate(Inventory inv){
		for (int id = 0; id <= 11; id++) {

			inv.CheckResource (id);
			list[id].text = "" + inv.CheckResource (id);
		}
	}

	private class Inventory {
		public int CheckResource(int resourceIndex)
		{
			return 10;
		}
	}



}
