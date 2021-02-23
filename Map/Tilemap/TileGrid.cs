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
        [Serializable]
        class GrassTiles
        {
            public ObjectTileType objectTileType;
            public Texture2D texture;
            public Color color;
        
        }

        [SerializeField]
        GroundTiles[] groundTilesTypes;
        [SerializeField]
        ObjectTiles[] objectTilesTypes;
        [SerializeField]
        GrassTiles[] grassTilesTypes;

        public Dictionary<TilemapType, TilemapStructure> TilemapType;

        /// <summary>
        /// 1. TileGrid, get dictionary form TileTypes, but do not create
        /// 2. Check if there is tilemap, and is correct Type.
        /// 3. do Algorithms.
        /// </summary>
        private void Awake()
        {

            TilesDictionary = InitializeTile(); 

            TilemapType = new Dictionary<TilemapType, TilemapStructure>();
            
            
            foreach (Transform child in transform) // get Transform from 'this' transform.gameobject
            {
                TilemapStructure tilemap = child.GetComponent<TilemapStructure>();
                if (tilemap == null) continue; // if there is no tilemap, return.
                if (TilemapType.ContainsKey(tilemap.tilemapType))
                {
                    throw new Exception("Duplicate Tilemap type: " + tilemap.tilemapType);
                }
                TilemapType.Add(tilemap.tilemapType, tilemap);
            }

            foreach(TilemapStructure tilemap in TilemapType.Values)
            {
                tilemap.Initialize();
            }
        }
        Dictionary<int, Tile> InitializeTile()
        {
            var dictionary = new Dictionary<int, Tile>();
            foreach (GroundTiles tileType in groundTilesTypes) 
            {
                if (tileType.groundTileType == 0) continue;
                Tile tile = CreateTile(tileType.color, tileType.texture);     // scriptableobject.createinstance<tile>();
                dictionary.Add((int)tileType.groundTileType, tile);
            }

            foreach (ObjectTiles tileType in objectTilesTypes)
            {
                if (tileType.objectTileType == 0) continue;

                Tile tile = CreateTile(tileType.color, tileType.texture);

                dictionary.Add((int)tileType.objectTileType, tile);
            }

            foreach (GrassTiles tileType in grassTilesTypes)
            {
                if (tileType.objectTileType == 0) continue;

                Tile Createtile = CreateTile(tileType.color, tileType.texture);
                dictionary.Add((int)tileType.objectTileType, Createtile);
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
