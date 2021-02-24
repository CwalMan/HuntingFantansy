using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Tilemaps;
using Assets.ScriptableObjects;
using System;

public class MobLandMark : MonoBehaviour
{
    TileGrid tileGrid;

    public PoissonSamp poissonSamp;
    public Vector2 totalSize;

    [Range(2,100)]
    public float radius;
    public float viewRadius;

    public bool Giz;

    List<Vector2> points;
    List<Vector2Int> electedVectorList;

    int numSampleBeforeRejection = 30;
    int Width, Height;
    private void Awake()
    {
        tileGrid = GetComponentInParent<TileGrid>();
        Initialize();
    }
    void Initialize()
    {
        Width = tileGrid.Width;
        Height = tileGrid.Height;
        totalSize = new Vector2(Width, Height);
        radius = poissonSamp.radius;
        numSampleBeforeRejection = poissonSamp.RejectionSamples;
    }
    private void Start()
    {
        points = poissonSamp.GenerateSpawnPoint(radius, totalSize, numSampleBeforeRejection);
        List<Vector2Int> pointsInt = ToInt2(points);
        //electedVectorList = isApprovedTile(pointsInt);
        electedVectorList = TestTile(pointsInt);

        for (int i = 0; i < electedVectorList.Count; i++)
        {
            Vector2 electedVectors = new Vector2(electedVectorList[i].x, electedVectorList[i].y);
            GameObject gameObject = Instantiate(poissonSamp.mobPrefab, electedVectors, Quaternion.identity);
            gameObject.transform.parent = this.transform;
        }
    }

    List<Vector2Int> TestTile(List<Vector2Int> pointsInt)
    {
        List<Vector2Int> vector2Ints = new List<Vector2Int>();
        TilemapStructure objectTileStruct = tileGrid.TilemapType[TilemapType.Object];
        TilemapStructure groundTileStruct = tileGrid.TilemapType[TilemapType.Ground];

        for (int i =0; i < pointsInt.Count; i++)
        {
            Vector2Int tilePos = new Vector2Int(pointsInt[i].x, pointsInt[i].y);

            int objectTileType = objectTileStruct.GetTileEnumNumber(tilePos.x, tilePos.y);
            int groundTileType = groundTileStruct.GetTileEnumNumber(tilePos.x, tilePos.y);

            if (objectTileType != (int)ObjectTileType.Tree)
            {
                for(int j = 0; j < poissonSamp.spawnTileType.Length; j++)
                {
                    if(groundTileType == (int)poissonSamp.spawnTileType[j])
                    {
                        vector2Ints.Add(tilePos);
                    }
                }
            }
        }
        return vector2Ints;
    }

    void OnDrawGizmos()
    {
        if(Giz)
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

    public Vector2Int changeVector2Int(Vector3 vector3)
    {
        Vector2Int vector2int;
        vector2int = new Vector2Int((int)vector3.x, (int)vector3.y);
        return vector2int;
    }

    public List<Vector2Int> ToInt2(List<Vector2> point)
    {
        List<Vector2Int> pointsInt = new List<Vector2Int>();

        for (int i = 0; i < point.Count; i++)
        {
            pointsInt.Add(new Vector2Int((int)point[i].x, (int)point[i].y));
        }
        return pointsInt;
    }
}
