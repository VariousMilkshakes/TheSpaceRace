using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SpaceRace.PlayerTools;
using Zones;
/*
	TODO: decouple logic into other classes, already started with zone class (only for methods which do not refer to the Unity API).
*/
/// <summary>
/// Map generator.
/// Responsible for generating the map and keeping track of tiles.
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// The number of columns (x).
    /// </summary>
    [Range(1, 100)]
    public int size;

    /// <summary>
    /// The seed to pass to the random number generator in the RandomiseGrid method.
    /// </summary>
    public string seed;

    /// <summary>
    /// The boolean decides whether to use the given seed or generate a rondom float between -1000 and 1000.
    /// </summary>
    public bool randomSeed;


    /// <summary>
    /// The percentage of the map covered in water before smoothing and processing.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/aa288454(v=vs.71).aspx"/>
    [Range(0, 100)]
    public float waterPercentage;

    /// <summary>
    /// The sprite paths.
    /// </summary>
    private Dictionary<TileTypes, string> spritePaths;

    /// <summary>
    /// The resource sprite paths.
    /// </summary>
    private Dictionary<Resource, string> resourceSpritePaths;

    /// <summary>
    /// The hover sprite.
    /// </summary>
    public Sprite hoverSprite;

    private String hoverSpritePath = "Sprites/Highlight_Sprite";

    /// <summary>
    /// The selected sprite.
    /// </summary>
    public Sprite selectedSprite;

    private String selectedSpritePath = "Sprites/Selected_Sprite";

    /// <summary>
    /// The building sprite.
    /// </summary>
    public Sprite buildingSprite;

    /// <summary>
    /// The static sprites.
    /// </summary>
    private Sprite[] staticSprites;

    /// <summary>
    /// The position of the bottom left hand corner of the grid.
    /// </summary>
    private Transform mapHolder;

    private System.Random rnd = new System.Random();

    /// <summary>
    /// The tiles.
    /// </summary>
    [HideInInspector]
    public List<Tile> tiles;

    /// <summary>
    /// The two dimensional integer array of grid positions used to refer to each position on the grid.
    /// </summary>
    TileFlag[,] gridPos;

    /// <summary>
    /// Gets the static sprites.
    /// </summary>
    /// <returns>The static sprites.</returns>
    public Sprite[] GetStaticSprites()
    {
        return staticSprites;
    }

    /*
	* Setters
	*/
    /// <summary>
    /// Sets the width of the map.
    /// </summary>
    /// <param name="width">Width.</param>
    public void SetMapSize(float size)
    {
        size = (int)size;
    }

    /// <summary>
    /// Sets the map water percentage.
    /// </summary>
    /// <param name="water">Water.</param>
    public void SetMapWaterPercentage(float water)
    {
        waterPercentage = water;
    }

    /// <summary>
    /// Sets the map seed.
    /// </summary>
    /// <param name="seed">Seed.</param>
    public void SetMapSeed(string seed)
    {
        randomSeed = false;
        this.seed = seed;
    }

    /// <summary>
    /// Gets the grid position.
    /// </summary>
    /// <returns>The grid position.</returns>
    public TileFlag[,] GetGridPos()
    {
        return gridPos;
    }

    /// <summary>
    /// Gets the tiles.
    /// </summary>
    /// <returns>The tiles.</returns>
    public List<Tile> GetTiles()
    {
        return tiles;
    }

    /// <summary>
    /// Gets the sprite paths.
    /// </summary>
    /// <returns>The sprite paths.</returns>
    public Dictionary<TileTypes, string> GetSpritePaths()
    {
        return spritePaths;
    }

    /// <summary>
    /// Gets the resource sprite paths.
    /// </summary>
    /// <returns>The resource sprite paths.</returns>
    public Dictionary<Resource, string> GetResourceSpritePaths()
    {
        return resourceSpritePaths;
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        spritePaths = new Dictionary<TileTypes, string>();
        resourceSpritePaths = new Dictionary<Resource, string>();
        setUpSpriteDict();
        staticSprites = new Sprite[3];
        staticSprites[0] = Resources.Load(hoverSpritePath, typeof(Sprite)) as Sprite;
        staticSprites[1] = Resources.Load(selectedSpritePath, typeof(Sprite)) as Sprite;
        staticSprites[2] = buildingSprite;
        if (randomSeed)
        {
            System.Random random = new System.Random();
            seed = "" + random.Next();
        }
        tiles = new List<Tile>();

        int zoneSise = size / 2;

        List<Zone> zones = new List<Zone>();
        zones.Add(new Zone(zoneSise, (float)seed.GetHashCode(), (int)waterPercentage));
        zones.Add(new Zone(zoneSise, -(float)seed.GetHashCode(), (int)waterPercentage));
        zones.Add(new Zone(zoneSise, 2f * (float)seed.GetHashCode(), (int)waterPercentage));
        zones.Add(new Zone(zoneSise, -2f * (float)seed.GetHashCode(), (int)waterPercentage));

        ZoneManager ZM = new ZoneManager();
        Zone newZone = ZM.CombineZones(zones, zoneSise);
        gridPos = newZone.GetZone();
        size = zoneSise * 2;
        SetUpMap();
    }

    private void setUpSpriteDict()
    {
        spritePaths.Add(TileTypes.GRASS, "Sprites/Grass_Tile_Sprite");
        spritePaths.Add(TileTypes.MOUNTAIN, "Sprites/Grey_Tile_Sprite");
        spritePaths.Add(TileTypes.SAND, "Sprites/Sand_Tile_Sprite");
        spritePaths.Add(TileTypes.WATER, "Sprites/Water_Tile_Sprite");

        resourceSpritePaths.Add(Resource.Food, "Resources/Fish");
        resourceSpritePaths.Add(Resource.Wood, "Resources/Forest");
        resourceSpritePaths.Add(Resource.Stone, "Resources/Stone");
        resourceSpritePaths.Add(Resource.Iron, "Resources/Iron");
    }


    /// <summary>
    /// Gets the tile.
    /// </summary>
    /// <returns>The tile.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public Tile GetTile(int x, int y)
    {
        foreach (Tile t in tiles)
        {
            if (t.transform.position.x == x && t.transform.position.y == y)
            {
                return t;
            }
        }
        throw new NoTileException("No such Tile");
    }

    /// <summary>
    /// Sets up map.
    /// </summary>
    /// <description>
    /// This method is responsable for creating the Tile of the type as set in gridPos by the RandomiseGrid method.
    /// </description>
    void SetUpMap()
    {
        mapHolder = new GameObject("Map").transform;
        mapHolder.tag = "Map";

        if (gridPos != null)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    GameObject tile = new GameObject("Tile");
                    tile.transform.SetParent(GameObject.Find("PlaneManager").transform);
                    tile.AddComponent<Tile>();
                    Tile script = (Tile)tile.GetComponent("Tile");
                    script.SetUpTile(gridPos[x, y]);
                    GameObject instance = Instantiate(tile, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(mapHolder);
                }
            }
        }

        Component[] actualTiles = mapHolder.GetComponentsInChildren<Tile>();
        foreach (Component tile in actualTiles)
        {
            tiles.Add((Tile)tile);
        }

        GameObject planeManager = GameObject.FindGameObjectWithTag("PlaneManager");
        Component[] oldTiles = planeManager.GetComponentsInChildren<Tile>();
        foreach (Tile t in oldTiles)
        {
            Destroy(t.gameObject);
        }
    }
}