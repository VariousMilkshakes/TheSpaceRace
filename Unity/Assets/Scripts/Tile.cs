using UnityEngine;
using System.Collections;

public class Tile{
	/*
	* This attribute means that this Tiles GameObject cannot be set in the inspector.
	* 
	* Sets this Tiles GameObject to a new GameObject called Tile so that it cab be displayed and manipulated by the Unity engine.
	*/
	[HideInInspector]
	public GameObject tile = new GameObject ("Tile");

	/*
	* The SpriteRenderer of this Tiles GameObject to allow sprites to be rendered.
	*/
	private SpriteRenderer sr;

	/*
	* The array of sprites this Tile could possibly take. 
	*/
	public Sprite[] spriteArray;

	/*
	* The type of Tile that this is. (Currently grass(0) or water(1))
	*/
	public int type;

	/*
	* Constructor.
	* 
	* Sets the spriteArray to sprites.
	* Sets the this type to the given type.
	* Sets sr as a new SpriteRenderer component of this Tile.
	* Sets the sprite that sr renders to the type of tile that this is.
	*/
	public Tile(int type, Sprite[] sprites){
		spriteArray = sprites;
		this.type = type;
		sr = tile.AddComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		SetTileType (this.type);
	}

	/*
	* This method sets the sprite of the tile based on the type of tile it is. 
	* This means that the array of sprites passed to the Tile object cannot changed or the indexing of the arry will fail.
	*/
	void SetTileType(int type){
		if (type == 0) {
			sr.sprite = spriteArray[0];
		}else if(type == 1){
			sr.sprite = spriteArray [1];
		}
	}
}
