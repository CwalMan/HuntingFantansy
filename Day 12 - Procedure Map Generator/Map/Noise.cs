using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helper
{
    public class Noise : MonoBehaviour
    {
        public static float[] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[] noiseMap = new float[mapWidth * mapHeight];

            var random = new System.Random(seed);

            if(octaves <1)
            {
                octaves = 1;
            }
            if (scale <= 0f)
            {
                scale = 0.0001f;
            }
            Vector2[] octaveOffsets = new Vector2[octaves];
            int HThousand = 100000;
            for(int i=0; i< octaves; i++)
            {
                float offsetX = random.Next(-HThousand, HThousand) + offset.x;
                float offsetY = random.Next(-HThousand, HThousand) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            for(int x=0,y;x<mapWidth;x++)
            {
                for(y=0;y<mapHeight;y++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for(int i =0; i< octaves;i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;
                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                        noiseHeight += perlinValue * amplitude;
                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                    noiseMap[y * mapWidth + x] = noiseHeight;
                }
            }

            for(int x=0,y;x<mapWidth;x++)
            {
                for(y=0;y<mapHeight;y++)
                {
                    noiseMap[y * mapWidth + x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y * mapWidth + x]);
                }
            }
            return noiseMap;
        }
    }
}

