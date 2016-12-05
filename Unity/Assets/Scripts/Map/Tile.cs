using UnityEngine;
using System.Collections;
using System.IO;

using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using System;
using SpaceRace.PlayerTools;
using SpaceRace.World;

public class Tile: MonoBehaviour{
	/// <summary>
	/// The sr.
	/// </summary>
	/// <description>
	/// This attribute means that this Tiles GameObject cannot be set in the inspector.
	/// The SpriteRenderer of this Tiles GameObject to allow sprites to be rendered.
	/// </description>
	[HideInInspector]
	public SpriteRenderer sr;

	/// <summary>
	/// The highlight sr.
	/// </summary>
	/// <description>
	/// The SpriteRenderer of this Tiles Highlighter GameObject to allow sprites to be rendered when the Tile is highlighted.
	/// </description>
	[HideInInspector]
	public SpriteRenderer hsr;

	/// <summary>
	/// The BoxCollider2D of this Tiles GameObject to allow the mouse to be detected by the tiles.
	/// </summary>
	[HideInInspector]
	private BoxCollider2D bx2D;

	/// <summary>
	/// The highlighter.
	/// </summary>
	/// <description>
	/// A GameObject which is a child of the Tile and has a SpriteRenderer component.
	/// </description>
	[HideInInspector]
	GameObject highlighter;

	/// <summary>
	/// Gets the building to be placed on this tile.
	/// </summary>
	/// <value>The building.</value>
	public Building Building
	{
		get { return building; }
	}
	private Building building;


	/// <summary>
	/// Is the tile selected.
	/// </summary>
	/// <description>
	/// Corrisponds to whether the Tile is selected or not.
	/// </description>
	/// <value>
	/// false
	/// </value>
	bool selected = false;

	/// <summary>
	/// The hover sprite.
	/// </summary>
	public Sprite hoverSprite;

	/// <summary>
	/// The selected sprite.
	/// </summary>
	public Sprite selectedSprite;

	/// <summary>
	/// The building sprite.
	/// </summary>
	public Sprite buildingSprite;

	/// <summary>
	/// The type of Tile that this is. (Currently grass(0) or water(1))
	/// </summary>
	public int type;

	/// <summary>
	/// The x position.
	/// </summary>
	int x;

	/// <summary>
	/// The y position.
	/// </summary>
	int y;

	/// <summary>
	/// The score.
	/// </summary>
	public int score;

	/// <summary>
	/// Sets the state.
	/// </summary>
	/// <value>The state.</value>
	public WorldStates State
	{
		set { tileState = value; }
	}
	private WorldStates tileState;

	/// <summary>
	/// Sprite type.
	/// </summary>
	enum SpriteType { HOVER, SELECTED, BUILDING };

	/// <summary>
	/// Initiates a new tile.
	/// </summary>
	/// <description>
	/// Constructor.
	///		Not realy a constructor but acts as one.
	///		This is necessary because the Tile script is a MonoBehaviour.
	/// 	This means that the 'new' keyword cannot be used to instantiate this class.
	/// 	This method therefore must be called everytime a new Tile object is instantiated to set default values and pass the object parameters.
	/// </description>
	/// <param name="type">Type.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <see cref="https://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
	public void NewTile(int type, int x, int y){
		MapGenerator mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent ("MapGenerator") as MapGenerator;
		Sprite[] statics = mapGen.GetStaticSprites();
		hoverSprite = statics[(int)SpriteType.HOVER];
		selectedSprite = statics[(int)SpriteType.SELECTED];
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
		buildingSprite = statics[(int)SpriteType.BUILDING];
	}

	/// <summary>
	/// Gets the x.
	/// </summary>
	/// <returns>The x.</returns>
	public int GetX(){
		return x;
	}

	/// <summary>
	/// Gets the y.
	/// </summary>
	/// <returns>The y.</returns>
	public int GetY(){
		return y;
	}
		
	/// <summary>
	/// Raises the mouse enter event.
	/// </summary>
	/// <description>
	/// This method changes the sprite of this tiles sprite renderer if it is not currently seleceted to the hover version of this Tile.
	/// </description>
	/// <see cref="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseEnter.html"/>
	void OnMouseEnter (){
		Component[] sptren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer hisr = (SpriteRenderer)sptren [1];
		if (!selected) {
			hisr.sprite = hoverSprite;
		}
	}
		
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	/// <description>
	/// This method changes the sprite of this tiles sprite renderer back to its origional sprite.
	/// </description>
	/// <see cref="see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseExit.html"/>
	void OnMouseExit(){
		Component[] sptren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer hisr = (SpriteRenderer)sptren [1];
		if (!selected) {
			hisr.sprite = null;
		}
	}
		

	/// <summary>
	/// Raises the mouse down event.
	/// </summary>
	/// <description>
	/// This method first calls DeselectTile then changes the sprite of this tiles sprite renderer to its corrisponding selected sprite.
	/// Also this tile's selected is set to 'true'.
	/// </description>
	/// <see cref="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseDown.html"/>
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
		
	/// <summary>
	/// Deselects the tile.
	/// </summary>
	/// <description>
	/// Finds any selected tiles in the game and change their selected value to false and change their sprite to their origional type.
	/// </description>
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
		
	/// <summary>
	/// Sets the tile sprite.
	/// </summary>
	/// <description>
	/// This method sets the sprite of the tile based on the type of tile it is.
	/// This means that the array of sprites passed to the Tile object cannot changed or the indexing of the arry will fail.
	/// </description>
	/// <param name="type">Type.</param>
	void SetTileSprite(int type){
		MapGenerator mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent ("MapGenerator") as MapGenerator;
		sr.sprite = mapGen.GetSprite (type);
	}

	/// <summary>
	/// Build the specified buildingType for the specified builder.
	/// </summary>
	/// <param name="buildingType">Building type.</param>
	/// <param name="builder">Builder.</param>
	public bool Build (Type buildingType, Player builder)
	{
		var buildMethod = typeof(Building).GetMethod("BUILD");
		var genericBuildMethod = buildMethod.MakeGenericMethod(buildingType);

		Building newBuilding;

		try
		{
			newBuilding = genericBuildMethod.Invoke(null, new object[] { builder }) as Building;
		}
		catch (Exception)
		{
			Debug.Log("Not enough resources");
			return false;
		}

		builder.TrackBuilding(newBuilding);
		building = newBuilding;
		sr.sprite = building.ActiveSprite;
		builder.Inventory.AddResource(building.OnBuild());

		return true;
	}

	/// <summary>
	/// Applies the color of the player.
	/// </summary>
	/// <param name="playerColor">Player color.</param>
	public void ApplyPlayerColor(Color playerColor)
	{
		sr.color = playerColor;
	}

}
