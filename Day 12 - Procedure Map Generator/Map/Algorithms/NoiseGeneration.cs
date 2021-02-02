using Assets.Helper;
using Assets.Tilemaps;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName ="NoiseGeneration",menuName = "Algorithms/NoiseGeneration")]
    public class NoiseGeneration : AlgorithmBase
    {
        [Header("Noise Setting")]
        public int Octaves;
        [Range(0, 1)]
        public float Persistance;
        public float Lacunarity;
        public float NoiseScale;
        public Vector2 Offset;

        [Serializable]
        class NoiseValue
        {
            [Range(0f, 1f)]
            public float Height;
            public TileTypeEnum tileTypeEnum;
        }

        [SerializeField]
        NoiseValue[] tileTypes;

        public override void Apply(TilemapStructure tilemap)
        {
            // Make sure TileType ordered from small to high height
            tileTypes = tileTypes.OrderBy(a => a.Height).ToArray();

            var noiseMap = Noise.GenerateNoiseMap(tilemap.Width, tilemap.Height, tilemap.Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

            for(int x=0; x< tilemap.Width; x++)
            {
                for(int y=0; y< tilemap.Height; y++)
                {
                    var height = noiseMap[y * tilemap.Width + x];

                    for(int i =0; i< tileTypes.Length; i++)
                    {
                        if(height <= tileTypes[i].Height)
                        {
                            tilemap.SetTile(x, y, (int)tileTypes[i].tileTypeEnum);
                            break;
                        }
                    }
                }
            }
        }

    }
}
