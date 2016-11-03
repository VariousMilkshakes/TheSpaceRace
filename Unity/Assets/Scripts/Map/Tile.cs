using UnityEngine;
using System.Collections;

public class Tile: MonoBehaviour{
	/*
	* This attribute means that this Tiles GameObject cannot be set in the inspector.
	*
	* The SpriteRenderer of this Tiles GameObject to allow sprites to be rendered.
	*/
	[HideInInspector]
	public SpriteRenderer sr;

	/*
	* The BoxCollider2D of this Tiles GameObject to allow the mouse to be detected by the tiles.
	*/
	[HideInInspector]
	private BoxCollider2D bx2D;

	/*
	* Corrisponds to whether the Tile is selected or not.
	*/
	bool selected = false;

	/*
	* The arrays of sprites this Tile could possibly take. 
	*/
	public Sprite[] spriteArray;
	public Sprite[] hoverArray;
	public Sprite[] selectedArray;

	/*
	* The type of Tile that this is. (Currently grass(0) or water(1))
	*/
	public int type;

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
	public void NewTile(int type, Sprite[] sprites, Sprite[] hovers, Sprite[] selected){
		spriteArray = sprites;
		hoverArray = hovers;
		selectedArray = selected;
		this.type = type;
		sr = gameObject.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		bx2D = gameObject.AddComponent (typeof(BoxCollider2D)) as BoxCollider2D;
		bx2D.isTrigger = true;
		bx2D.enabled = true;
		SetTileSprite (this.type);
	}

	/*
	* This method changes the sprite of this tiles sprite renderer if it is not currently seleceted to the hover version of this Tile.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseEnter.html
	*/
	void OnMouseEnter (){
		SpriteRenderer sptren = (SpriteRenderer)this.gameObject.GetComponent ("SpriteRenderer");
		if (!selected) {
			sptren.sprite = hoverArray [type];
		}
	}

	/*
	* This method changes the sprite of this tiles sprite renderer back to its origional sprite.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseExit.html
	*/
	void OnMouseExit(){
		SpriteRenderer sptren = (SpriteRenderer)this.gameObject.GetComponent ("SpriteRenderer");
		if (!selected) {
			sptren.sprite = spriteArray [type];
		}
	}

	/*
	* This method first calls DeselectTile then changes the sprite of this tiles sprite renderer to its corrisponding selected sprite.
	* Also this tile's selected is set to 'true'.
	* see https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseDown.html
	*/
	void OnMouseDown(){
		DeselectTile ();
		SpriteRenderer sprtren = (SpriteRenderer)this.gameObject.GetComponent ("SpriteRenderer");
		if (!selected) {
			selected = true;
			sprtren.sprite = selectedArray [type];
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
						t.sr.sprite = t.spriteArray [t.type];
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
		if (type == 0) {
			sr.sprite = spriteArray[0];
		}else if(type == 1){
			sr.sprite = spriteArray [1];
		}
	}

}
