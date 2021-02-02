using Assets.ScriptableObjects;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Tilemaps
{
    public class TilemapStructure : MonoBehaviour
    {
        public int Width, Height, TileSize, Seed;
        private int[] _tiles;
        Tilemap _graphicMap;

        [Serializable]
        class TileType
        {
            public TileTypeEnum tileType;
            public Color color;
        }
        [SerializeField]
        AlgorithmBase[] _algorithmBases;

        [SerializeField]
        TileType[] tileTypes;

        Dictionary<int, Tile> _tileTypeDictionary;


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                RenderAllTiles();
            }
        }

        private void Awake()
        {
            _graphicMap = GetComponent<Tilemap>();

            _tiles = new int[Width * Height];

            _tileTypeDictionary = new Dictionary<int, Tile>();

            var tileSprite = Sprite.Create(new Texture2D(TileSize, TileSize), new Rect(0, 0, TileSize, TileSize), new Vector2(0.5f, 0.5f), TileSize);

            foreach (var tiletype in tileTypes)
            {
                var tile = ScriptableObject.CreateInstance<Tile>();
                tiletype.color.a = 1;
                tile.color = tiletype.color;
                tile.sprite = tileSprite;
                _tileTypeDictionary.Add((int)tiletype.tileType, tile);
            }
            foreach(var algorithm in _algorithmBases)
            {
                Generate(algorithm);
            }
            RenderAllTiles();
        }

        public void RenderAllTiles()
        {
            var positionsArray = new Vector3Int[Width * Height];
            var tilesArray = new Tile[Width * Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    positionsArray[x * Width + y] = new Vector3Int(x, y, 0);
                    var typeofTile = GetTile(x, y);
                    tilesArray[x * Width + y] = _tileTypeDictionary[typeofTile];
                }
            }

            _graphicMap.SetTiles(positionsArray, tilesArray);
            _graphicMap.RefreshAllTiles();
        }

        public int GetTile(int x, int y)
        {
            return InBounds(x, y) ? _tiles[y * Width + x] : 0;
        }

        public void SetTile(int x, int y, int value)
        {
            if (InBounds(x, y))
            {
                _tiles[y * Width + x] = value;
            }
        }
        public void Generate(AlgorithmBase algorithmBase)
        {
            algorithmBase.Apply(this);
        }
        bool InBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

    }
}
