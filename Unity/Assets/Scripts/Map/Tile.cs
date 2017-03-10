﻿using UnityEngine;
using System.Collections;
using System.IO;

using SpaceRace.World.Buildings;
using SpaceRace.World.Buildings.Collection;
using System;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using SpaceRace.World;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

/// <summary>
/// Tile.
/// describes how tiles act in the game.
/// </summary>
public class Tile: MonoBehaviour{

    public enum TileType
    {
        Grass,
        Water,
        Sand,
        Mountain
    }

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	/// <description>
	/// This attribute means that this Tiles GameObject cannot be set in the inspector.
	/// The SpriteRenderer of this Tiles GameObject to allow sprites to be rendered.
	/// </description>
	[HideInInspector]
	public SpriteRenderer sr;

	/// <summary>
	/// The resource sprite renderer.
	/// </summary>
	[HideInInspector]
	public SpriteRenderer rsr;

	/// <summary>
	/// The initial resource of this tile.
	/// </summary>
	public Resource resource = Resource.None;

	/// <summary>
	/// The resource box.
	/// </summary>
	public ResourceBox reBox;

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

	GameObject resourceSpriteRenderer;

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
	/// The type of Tile that this is. (Currently grass(0), water(1), sand(2), mountain(3))
	/// </summary>
	public int type;

    public TileType Type
    {
        get { return (TileType)type; }
    }

	/// <summary>
	/// The score.
	/// </summary>
	public int score;


	private String owner;

	/// <summary>
	/// Sets the owner of this tile
	/// </summary>
	/// <param name="owner">Owner.</param>
	public void SetOwner(String owner, Color colour){
		this.owner = owner;	
		this.ApplyPlayerColor (colour);
	}

	/// <summary>
	/// Return the player who owns this tile
	/// </summary>
	public String GetOwner(){
		return owner;
	}

	private MapGenerator mapGen;

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
	/// Sets up tile.
	/// </summary>
	/// <description>
	/// Constructor.
	///		Not realy a constructor but acts as one.
	///		This is necessary because the Tile script is a MonoBehaviour.
	/// 	This means that the 'new' keyword cannot be used to instantiate this class.
	/// 	This method therefore must be called everytime a new Tile object is instantiated to set default values and pass the object parameters.
	/// </description>
	/// <see cref="https://docs.unity3d.com/ScriptReference/MonoBehaviour.html"/>
	/// <param name="flags">Flags.</param>
	public void SetUpTile(TileFlag flags){
		mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent ("MapGenerator") as MapGenerator;
		Sprite[] statics = mapGen.GetStaticSprites();
		hoverSprite = statics[(int)SpriteType.HOVER];
		selectedSprite = statics[(int)SpriteType.SELECTED];
		this.type = flags.GetTileType ();
		this.resource = flags.GetResource ();
		sr = this.gameObject.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;

		highlighter = new GameObject ("Highliter");
		highlighter.transform.SetParent (this.gameObject.transform);
		hsr = highlighter.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		hsr.sortingOrder = 2;
		hsr.sprite = null;
		SetTileSprite ((TileTypes)flags.GetTileType());

		reBox = ResourceBox.EMPTY ();
		this.AddResource (resource);

		bx2D = gameObject.AddComponent (typeof(BoxCollider2D)) as BoxCollider2D;
		bx2D.isTrigger = true;
		bx2D.enabled = true;
		building = null;
		buildingSprite = statics[(int)SpriteType.BUILDING];
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	/*void Update(){
		if (GetResourceBox().Quantity == 0){
			reBox = ResourceBox.EMPTY ();
			resource = Resource.None;
			//SetTileSprite ((TileTypes)type);
		}
	}*/

	/// <summary>
	/// Gets the x.
	/// </summary>
	/// <returns>The x.</returns>
	public int GetX(){
		return (int)gameObject.transform.position.x;
	}

	/// <summary>
	/// Gets the y.
	/// </summary>
	/// <returns>The y.</returns>
	public int GetY(){
		return (int)gameObject.transform.position.y;
	}

	/// <summary>
	/// Gets the resource.
	/// </summary>
	/// <returns>The resource.</returns>
	public Resource GetResource(){
		return resource;
	}

	/// <summary>
	/// Gets the resource box.
	/// </summary>
	/// <returns>The resource box.</returns>
	public ResourceBox GetResourceBox(){
		return reBox;
	}

	/// <summary>
	/// Adds the resource.
	/// </summary>
	/// <param name="resouce">Resource.</param>
	public void AddResource(Resource resource){
		this.resource = resource;
		SetResourceSprite (resource);
		reBox = new ResourceBox (resource, 10, 10);
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
	void OnMouseDown()
	{

	    if (EventSystem.current.IsPointerOverGameObject()) return;

		Component[] sprtren = this.gameObject.GetComponentsInChildren <SpriteRenderer> ();
		SpriteRenderer hisr = (SpriteRenderer)sprtren [1];
		if (!selected) {
			DeselectTile ();
			selected = true;
			hisr.sprite = selectedSprite;
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
	void SetTileSprite(TileTypes type){
		//try{
			this.sr.sprite = Resources.Load(GetSpritePath(type), typeof(Sprite)) as Sprite;
		/*}catch(Exception e){
			Debug.Log (e.StackTrace);
		}*/
	}

	void SetResourceSprite(Resource resource){
		if (resource != Resource.None && resource != Resource.Straw) {
			sr.sprite = Resources.Load (GetResourceSpritePath (resource), typeof(Sprite)) as Sprite;
		}
	}

	public string GetSpritePath(TileTypes type){
		string outString;
		mapGen = GameObject.FindGameObjectWithTag ("PlaneManager").GetComponent ("MapGenerator") as MapGenerator;
		if(mapGen.GetSpritePaths().TryGetValue (type, out outString)){
			return outString;
		}
		return "Sprites/Water_Tile_Sprite";
	}

	public string GetResourceSpritePath(Resource type){
		string outString;
		if(mapGen.GetResourceSpritePaths().TryGetValue (type, out outString)){
			return outString;
		}
		return "Sprites/Fish";
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
	        newBuilding = genericBuildMethod.Invoke(null, new object[] {builder, this}) as Building;
	    }
		catch (Exception e)
		{
		    if (e.InnerException.GetType() == typeof(BuildingException))
		    {
		        UiHack.ERROR.Handle((BuildingException)e.InnerException);
		    }

			Debug.Log(e);
			return false;
		}

        // Get rid of old buildings
	    if (building != null) {
	        builder.RemoveBuilding(building);
	    }

        builder.TrackBuilding(newBuilding);
		building = newBuilding;
	    sr.sprite = building.GetActiveSprite();
		builder.Inventory.AddResource(building.OnBuild());

		return true;
	}

    //TODO: Move this to building class
    public void DestoryBuilding ()
    {
        if (building == null) return;

        Player owner = building.Owner;
        owner.RemoveBuilding(building);

        building = null;
		SetTileSprite((TileTypes)this.type);
		SetResourceSprite (resource);
    }

	/// <summary>
	/// Applies the color of the player.
	/// </summary>
	/// <param name="playerColor">Player color.</param>
	public void ApplyPlayerColor(Color playerColor)
	{
		sr.color = playerColor;
	}

	public Color GetTileColour(){
		return sr.color;
	}

}
