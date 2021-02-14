using System;
using System.Linq;
using Assets.Tilemaps;
using UnityEngine;

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
            [Range(0,100)]
            public int SpawnChancePerCell;
        }

        // Create Random Tile
        public override void Apply(TilemapStructure tilemap)
        {
            TilemapStructure groundTilemap = tilemap.grid.TilemapType[TilemapType.Ground];
            var random = new System.Random(tilemap.grid.Seed);

            for(int x= 0; x< tilemap.Width; x++)
            {
                for(int y=0; y< tilemap.Height;y++)
                {
                    foreach(var spawnThing in spawnConfigulations)
                    {
                        int groundTile = groundTilemap.GetTile(x, y);

                        if(spawnThing.spawnTileType.Any(tile => (int)tile == groundTile)) // 타일 중 하나라도 groundTile이랑 같다면
                        {
                            if(random.Next(0,100) <= spawnThing.SpawnChancePerCell)
                            {
                                tilemap.SetTile(x, y, (int)spawnThing.objectType);
                            }
                        }
                    }
                }
            }
        }
    }
}
