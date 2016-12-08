using UnityEngine;
using System.Collections;
using System.IO;

using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using System;
using SpaceRace.PlayerTools;
using SpaceRace.World;

public class Tile: MonoBehaviour{
	/*
	* This attribute means that this Tiles GameObject cannot be set in the inspector.
	*
	* The SpriteRenderer of this Tiles GameObject to allow sprites to be rendered.
	*/
	[HideInInspector]
	public SpriteRenderer sr;
	[HideInInspector]
	public SpriteRenderer hsr;
	/*
	* The BoxCollider2D of this Tiles GameObject to allow the mouse to be detected by the tiles.
	*/
	[HideInInspector]
	private BoxCollider2D bx2D;

	[HideInInspector]
	GameObject highlighter;

	/*
	* The building on this tile.
	*/
	public Building Building
	{
		get { return building; }
	}
	private Building building;

	/*
	* Corrisponds to whether the Tile is selected or not.
	*/
	bool selected = false;

	/*
	* The arrays of sprites this Tile could possibly take. 
	*/
	public Sprite[] spriteArray;
	public Sprite hoverSprite;
	public Sprite selectedSprite;
	public Sprite buildingSprite;

	/*
	* The type of Tile that this is. (Currently grass(0) or water(1))
	*/
	public int type;

	int x;
	int y;

	public int score;

	private int currentPlayer;

	public WorldStates State
	{
		set { tileState = value; }
	}
	private WorldStates tileState;

	/*
	* Constructor.
	* 		Not realy a constructor but acts as one.
	* 		This is necessary because the Tile script is a MonoBehaviour (see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html).
	* 		This means that the 'new' keyword cannot be used to instantiate this class.
	* 		This method therefore must be called everytime a new Tile object is instantiated to set default values and pass the object parameters.
	* 
	* Sets the spriteArray to sprites.
	* Sets the hoverArray to hovers.
	* Sets the selectedArray to selected.
	* Sets this type to the given type.
	* Sets sr as a new SpriteRenderer component of this Tile.
	* Sets bx2D as a new BoxCollider2D component of this Tile. Also sets bx2D's isTrigger and enabled values to 'true'.
	* Sets the sprite that sr renders to the type of tile that this is.
	*/
	public void NewTile(int type, Sprite[] sprites, Sprite hover, Sprite selected, Sprite buildingSprite, int x, int y){
		spriteArray = sprites;
		hoverSprite = hover;
		selectedSprite = selected;
		this.type = type;
		sr = gameObject.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		this.x = x;
		this.y = y;

		highlighter = new GameObject ("Highliter");
		highlighter.transform.SetParent (this.gameObject.transform);
		hsr = highlighter.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		hsr.sortingOrder = 1;
		hsr.sprite = null;

		bx2D = gameObject.AddComponent (typeof(BoxCollider2D)) as BoxCollider2D;
		bx2D.isTrigger = true;
		bx2D.enabled = true;
		SetTileSprite (this.type);
		building = null;
		this.buildingSprite = buildingSprite;
	}

	public int GetX(){
		return (int)gameObject.transform.position.x ;
	}

	public int GetY(){
		return (int)gameObject.transform.position.y;
	}

	/*
	* This method changes the sprite of this tiles sprite renderer if it is not currently seleceted to the hover version of this Tile.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseEnter.html
	*/
	void OnMouseEnter (){
		Component[] sptren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer hisr = (SpriteRenderer)sptren [1];
		if (!selected) {
			hisr.sprite = hoverSprite;
		}
	}

	/*
	* This method changes the sprite of this tiles sprite renderer back to its origional sprite.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseExit.html
	*/
	void OnMouseExit(){
		Component[] sptren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer hisr = (SpriteRenderer)sptren [1];
		if (!selected) {
			hisr.sprite = null;
		}
	}

	/*
	* This method first calls DeselectTile then changes the sprite of this tiles sprite renderer to its corrisponding selected sprite.
	* Also this tile's selected is set to 'true'.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseDown.html
	*/
	void OnMouseDown(){
		
		Component[] sprtren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer tisr = (SpriteRenderer)sprtren [0];
		SpriteRenderer hisr = (SpriteRenderer)sprtren [1];
		if (!selected) {
			DeselectTile ();
			selected = true;
			hisr.sprite = selectedSprite;
		} else if (selected && type == 0) {
			tisr.sprite = buildingSprite;
		}

		try
		{
			GameObject ui = GameObject.Find("TempUIHandler");
			SpaceRace.Utils.UiHack uih = ui.GetComponent<SpaceRace.Utils.UiHack>();
			uih.DisplayBuildings(this);
		}catch
		{
			Debug.Log("Could not find");
		}

	}

	/*
	* Finds any selected tiles in the game and change their selected value to false and change their sprite to their origional type.
	*/
	void DeselectTile(){
		GameObject map = GameObject.FindWithTag ("Map");
		if (map != null) {
			Component[] tiles = map.GetComponentsInChildren<Tile> ();
			if (tiles != null) {
				foreach (Tile t in tiles) {
					if (t.selected) {
						t.selected = false;
						Component[] sptren = t.GetComponentsInChildren<SpriteRenderer> ();
						//SpriteRenderer tisr = (SpriteRenderer)sptren [0];
						SpriteRenderer hisr = (SpriteRenderer)sptren [1];
						hisr.sprite = null;
					}
				}
			}
		} 
	}

	/*
	* This method sets the sprite of the tile based on the type of tile it is. 
	* This means that the array of sprites passed to the Tile object cannot changed or the indexing of the arry will fail.
	*/
	void SetTileSprite(int type){
		sr.sprite = spriteArray [type];
	}

	public bool Build (Type buildingType, Player builder)
	{
		var buildMethod = typeof(Building).GetMethod("BUILD");
		var genericBuildMethod = buildMethod.MakeGenericMethod(buildingType);

		Building newBuilding;

		try
		{
			newBuilding = genericBuildMethod.Invoke(null, new object[] { builder }) as Building;
		}
		catch (Exception e)
		{
			Debug.Log(e);
			return false;
		}

		builder.TrackBuilding(newBuilding);
		building = newBuilding;
		sr.sprite = building.ActiveSprite;
		builder.Inventory.AddResource(building.OnBuild());

		return true;
	}

	public void ApplyPlayerColor(Color playerColor)
	{
		sr.color = playerColor;
	}

	public Color GetTileColour(){
		return sr.color;
	}

}
