using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Tilemaps
{
    public class TileGrid : MonoBehaviour
    {
        public int Width, Height;
        public int TileSize, Seed;
        public Dictionary<int,Tile> TilesDictionary // <enum.tiletype, Tile>
        { get; set; }


        [Serializable]
        class GroundTiles
        {
            public GroundTileType groundTileType;
            public Texture2D texture;
            public Color color;
        }

        [Serializable]
        class ObjectTiles
        {
            public ObjectTileType objectTileType;
            public Texture2D texture;
            public Color color;
        }

        [SerializeField]
        GroundTiles[] groundTilesTypes;
        [SerializeField]
        ObjectTiles[] objectTilesTypes;

        public List<Tile> tilesList;

        public Dictionary<TilemapType, TilemapStructure> TilemapType;
        private void Awake()
        {

            TilesDictionary = InitializeTile();

            TilemapType = new Dictionary<TilemapType, TilemapStructure>();

            foreach(Transform child in transform)
            {
                TilemapStructure tilemap = child.GetComponent<TilemapStructure>();
                if (tilemap == null) continue;
                if (TilemapType.ContainsKey(tilemap.Type))
                {
                    throw new Exception("Duplicate Tilemap type: " + tilemap.Type);
                }
                TilemapType.Add(tilemap.Type, tilemap);
            }

            foreach(TilemapStructure tilemap in TilemapType.Values)
            {
                tilemap.Initialize();
            }
        }
        Dictionary<int, Tile> InitializeTile()
        {
            var dictionary = new Dictionary<int, Tile>();
            foreach (var tileType in groundTilesTypes) // Create a Tile foreach GroundTileType
            {
                if (tileType.groundTileType == 0) continue;
                Tile tile = CreateTile(tileType.color, tileType.texture);     // scriptableobject.createinstance<tile>();
                dictionary.Add((int)tileType.groundTileType, tile);
            }

            foreach (var tileType in objectTilesTypes)
            {
                if (tileType.objectTileType == 0) continue;
                Tile tile = CreateTile(tileType.color, tileType.texture);

                dictionary.Add((int)tileType.objectTileType, tile);
            }

            return dictionary;
        }

        Tile CreateTile(Color color,Texture2D texture2D)
        {
            bool setColor = false;

            if (texture2D == null) // ObjectTileType의 Texture( 그림 ) 이 없으면 색깔로 대신한다.
            {
                setColor = true;
                texture2D = new Texture2D(TileSize, TileSize);
            }
            texture2D.filterMode = FilterMode.Point;

            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, TileSize, TileSize), new Vector2(0.5f,0.5f), TileSize);
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tilesList.Add(tile);
            if (setColor)
            {
                color.a = 1; 
                tile.color = color;
            }
            tile.sprite = sprite;

            return tile;
        }
    }
}
