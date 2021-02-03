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
        public int Octaves; // aka Num of Oct. more it has more effect the rest of parameter.
        [Range(0, 1)]
        public float Persistance; // smaller the value, everything became simpler.
        public float Lacunarity; // smaller the value, everything become smoother.
        public float NoiseScale; // Size. proportion to Width and Height.
        public Vector2 Offset; // where the noise should be generate?
        public bool ApplyIslandGradient;

        [Serializable]
        class NoiseValue
        {
            [Range(0f, 1f)]
            public float Height; // 0 = black, 1 = white
            public TileTypeEnum tileTypeEnum;
        }

        [SerializeField]
        NoiseValue[] tileTypes;

        public override void Apply(TilemapStructure tilemap)
        {
            // Make sure TileType ordered from small to high height
            tileTypes = tileTypes.OrderBy(a => a.Height).ToArray();

            float[] noiseMap = Noise.GenerateNoiseMap(tilemap.Width, tilemap.Height, tilemap.Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

            if(ApplyIslandGradient)
            {
                float[] islandGradient = Noise.GenerateIslandGradientMap(tilemap.Width, tilemap.Height);
                for(int x = 0, y; x<tilemap.Width; x++)
                {
                    for(y =0; y<tilemap.Height;y++)
                    {
                        float subtractedValue = noiseMap[y * tilemap.Width + x] - islandGradient[y * tilemap.Width + x];
                        // make sure Clamp to 0f~ 1f;
                        noiseMap[y * tilemap.Width + x] = Mathf.Clamp01(subtractedValue);
                    }
                }
            }

            for(int x=0; x< tilemap.Width; x++)
            {
                for(int y=0; y< tilemap.Height; y++)
                {
                    float height = noiseMap[y * tilemap.Width + x];

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
