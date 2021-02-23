using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Tilemaps;
using Assets.ScriptableObjects;
using System;

public class MobLandMark : MonoBehaviour
{
    public TileGrid tileGrid;

    public PoissonSamp poissonSamp;

    public List<Vector2> points;
    public int numSampleBeforeRejection;
    int Width, Height;
    public Vector2 totalSize;

    [Range(1,100)]
    public float radius;
    public float viewRadius;
    public Tilemap tilemap;
    public List<Vector2Int> pointsInt;
    public List<Vector2Int> specificTileList;

    private void Start()
    {
        tileGrid = GetComponent<TileGrid>();

        Initialize();
        pointsInt = ToInt2(points);
        specificTileList = isApprovedTile(pointsInt);
        
        for (int i = 0; i < specificTileList.Count; i++)
        {
            GameObject gameObject = Instantiate(poissonSamp.mobPrefab, new Vector2(specificTileList[i].x, specificTileList[i].y), Quaternion.identity);
            gameObject.transform.parent = this.transform;
        }
        
    }

    void Initialize()
    {
        Width = tileGrid.Width;
        Height = tileGrid.Height;
        totalSize = new Vector2(Width, Height);
        radius = poissonSamp.radius;
        numSampleBeforeRejection = poissonSamp.RejectionSamples;
    }

    private void OnValidate()
    {
        if(poissonSamp != null)
        {
            points = poissonSamp.GenerateSpawnPoint(radius, totalSize, numSampleBeforeRejection);
        }
    }

    List<Vector2Int> isApprovedTile(List<Vector2Int> pointsInt)
    {
        List<Vector2Int> vector2ints = new List<Vector2Int>();
        for (int i = 0; i < pointsInt.Count; i++)
        {
            Vector3Int vector3Int = new Vector3Int(pointsInt[i].x, pointsInt[i].y,0);

            if(tilemap.HasTile(vector3Int))
            {
                Tile tile = tilemap.GetTile<Tile>(vector3Int);
                tileGrid.TilesDictionary.TryGetValue((int)poissonSamp.spawnTileType, out Tile outTile);
                if(tile == outTile)
                {
                    vector2ints.Add(new Vector2Int(vector3Int.x, vector3Int.y));
                }
                
            }
            else
            {
                Debug.LogError("Cant find tile on " + vector3Int);
            }

        }
        /*
        allTiles.Add(tile);
        if (tileGrid.TilesDictionary.TryGetValue((int)poissonSamp.spawnTileType, out Tile owo))
        {
            Tile result = allTiles.Find(x => x != outTile);
            allTiles.Remove(result);
        }
        */
        return vector2ints;
    }


    public Vector2Int changeVector2Int(Vector3 vector3)
    {
        Vector2Int vector2int;
        vector2int = new Vector2Int((int)vector3.x, (int)vector3.y);
        return vector2int;
    }

    public List<Vector2Int> ToInt2(List<Vector2> point)
    {
        List<Vector2Int> pointsInt = new List<Vector2Int>();

        for(int i = 0; i< point.Count; i++)
        {
            pointsInt.Add(new Vector2Int((int)point[i].x, (int)point[i].y));
        }
        return pointsInt;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(totalSize / 2, totalSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                Gizmos.DrawWireSphere(point, viewRadius);
            }
        }
    }
}
