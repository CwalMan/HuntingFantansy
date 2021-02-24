using Assets.ScriptableObjects;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Tilemaps
{

    public class TilemapStructure : MonoBehaviour
    {
        [SerializeField]
        TilemapType _type;
        public TilemapType tilemapType { get { return _type; } }
        [HideInInspector]
        public int Width, Height;
        private int[] tilesInt;
        Tilemap tilemap;

        [HideInInspector]
        public TileGrid grid;

        public AlgorithmBase[] algorithmArray;



        public void Initialize()
        {
            tilemap = GetComponent<Tilemap>();
            grid = transform.parent.GetComponent<TileGrid>();

            Width = grid.Width;
            Height = grid.Height;

            tilesInt = new int[Width * Height];

            foreach (AlgorithmBase algorithm in algorithmArray)
            {
                algorithm.Apply(this); // get setTile
            }
            RenderAllTiles();
        }
        public void RenderAllTiles()
        {
            Vector3Int[] positionsArray = new Vector3Int[Width * Height];

            Tile[] tilesArray = new Tile[Width * Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    positionsArray[x * Width + y] = new Vector3Int(x, y, 0); // 각 타일의 위치를 구한다
                    int typeofTile = GetTileEnumNumber(x, y); 

                    if (!grid.TilesDictionary.TryGetValue(typeofTile, out Tile tile))
                    {
                        if (typeofTile != 0) 
                        {
                            Debug.LogError("Tile not defined for id: " + typeofTile);
                        }

                        tilesArray[x * Width + y] = null;
                        continue; // TileGrid의 TileDictionary부터 값을 받지못했다면 여기까지하고 다시 반복문시작
                    }

                    tilesArray[x * Width + y] = tile;
                }
            }

            tilemap.SetTiles(positionsArray, tilesArray); // Instantiate the tile in position[];
            tilemap.RefreshAllTiles();
        }
        public List<KeyValuePair<Vector2Int,int>> GetNeighbors(int tileX, int tileY)
        {
            // 이 메소드로 어느 한 타일의 8곳의 이웃을 구할 수 있다.
            int startX = tileX - 1;
            int startY = tileY - 1;
            int endX = tileX + 1;
            int endY = tileY + 1;

            var neighbors = new List<KeyValuePair<Vector2Int, int>>();

            for(int x = startX; x< endX+1; x++)
            {
                for(int y= startY; y < endY+1;y++)
                {
                    // dont need to add tile we're getting the neighbors of.
                    if (x == tileX && y == tileY) continue;

                    if(InBounds(x,y))
                    {
                        neighbors.Add(new KeyValuePair<Vector2Int, int>(new Vector2Int(x, y), GetTileEnumNumber(x, y)));
                    }
                }
            }
            return neighbors;
        }



        //check if the tile position is valid
        bool InBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
        // Return type of tile, otherwise 0 if invalid position
        public int GetTileEnumNumber(int x, int y)
        {
            return InBounds(x, y) ? tilesInt[y * Width + x] : 0;
        }
        //set type of tile at the given position
        public void SetTileEnumNumber(int x, int y, int value)
        {
            if (InBounds(x, y))
            {
                tilesInt[y * Width + x] = value;
            }
        }

    }
}
