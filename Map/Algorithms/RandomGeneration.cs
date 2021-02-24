using System;
using System.Linq;
using Assets.Tilemaps;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName ="RandomGeneration",menuName ="Algorithms/RandomGeneration")]
    public class RandomGeneration : AlgorithmBase
    {
        [SerializeField]
        SpawnConfigulation[] spawnConfigulations;

        [Serializable]
        class SpawnConfigulation
        {
            public ObjectTileType objectType;
            public GroundTileType[] spawnTileType;
            public ObjectTileType[] dontSpawnTileType;
            [Range(0,100)]
            public int SpawnChancePerCell;
        }

        // Create Random Tile
        public override void Apply(TilemapStructure tilemapStruct)
        {
            TilemapStructure groundTileStruct = tilemapStruct.grid.TilemapType[TilemapType.Ground];
            TilemapStructure objectTileStruct = tilemapStruct.grid.TilemapType[TilemapType.Object];

            var random = new System.Random(tilemapStruct.grid.Seed);

            for(int x= 0; x< tilemapStruct.Width; x++)
            {
                for(int y=0; y< tilemapStruct.Height;y++)
                {
                    foreach(SpawnConfigulation spawnThing in spawnConfigulations)
                    {
                        int groundTile = groundTileStruct.GetTileEnumNumber(x, y); // get tile's tileType, from tilePosition ex) 1 water, 2 sand, 4, grass
                        int objectTile = objectTileStruct.GetTileEnumNumber(x, y); // 0 none, 2000 tree, 

                        if(spawnThing.dontSpawnTileType.Length != 0)
                        {
                            for (int i = 0; i < spawnThing.dontSpawnTileType.Length; i++)
                            {
                                if (objectTile != (int)spawnThing.dontSpawnTileType[i])
                                {
                                    setTile(x, y, spawnThing, random, tilemapStruct, groundTile);
                                }
                            }
                        }
                        else
                        {
                            setTile(x, y, spawnThing, random, tilemapStruct, groundTile);
                        }

                    }
                }
            }
        }

        void setTile(int x, int y, SpawnConfigulation spawnThing, System.Random random, TilemapStructure tilemapStruct, int groundTile)
        {
            if (spawnThing.spawnTileType.Any(tile => (int)tile == groundTile)) // 타일 중 하나라도 groundTile이랑 같다면
            {
                if (random.Next(0, 100) <= spawnThing.SpawnChancePerCell)
                {
                    tilemapStruct.SetTileEnumNumber(x, y, (int)spawnThing.objectType);
                }
            }
        }
    }
}
