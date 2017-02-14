using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SpaceRace.PlayerTools;

public class TileFlag{

	private int tileType;
	private Resource resource;

	public TileFlag(int type){
		this.tileType = type;
		resource = Resource.None;
	}

	public void SetType(int type){
		this.tileType = type;
	}

	public void SetResource(Resource resource){
		this.resource = resource;
	}

	public int GetTileType(){
		return tileType;
	}

	public Resource GetResource(){
		return resource;
	}

}
