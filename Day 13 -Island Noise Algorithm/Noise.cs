using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Helper
{
    public class Noise : MonoBehaviour
    {
        public static float[] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            //Check  NoiseGeneration Script if you want to know parameter.
            float[] noiseMap = new float[mapWidth * mapHeight];

            System.Random random = new System.Random(seed);

            if(octaves <1)
            {
                octaves = 1;
            }
            if (noiseScale <= 0f)
            {
                noiseScale = 0.0001f;
            }
            Vector2[] octaveOffsets = new Vector2[octaves];
            int HThousand = 100000;
            for(int i=0; i< octaves; i++)
            {
                float offsetX = random.Next(-HThousand, HThousand) + offset.x;
                float offsetY = random.Next(-HThousand, HThousand) + offset.y;
                // random.Next = Random.range()
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;
            //before ask on this check out 74 line.
            
            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;
            // The Main Loop, calculate the noise.
            for (int x = 0, y; x < mapWidth; x++)
            {
                for(y = 0; y < mapHeight; y++)
                {
                    float amplitude = 1; // 진폭
                    float frequency = 1; // 주파수
                    float noiseHeight = 0; // 음높이

                    for(int i =0; i< octaves;i++) // 각 octave 마다 noise 계산
                    {
                        float sampleX = (x - halfWidth) / noiseScale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;
                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                        // noiseHeight is final noise, add all octaves together
                        noiseHeight += perlinValue * amplitude;
                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }
                    //  noise가 0f~1f 로 되어지게 만듦
                    if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                    else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                    noiseMap[y * mapWidth + x] = noiseHeight;
                }
            }

            for(int x=0,y;x<mapWidth;x++)
            {
                for(y=0;y<mapHeight;y++)
                {
                    // 0과 1로 된 noisemap값을 반대로 뒤집음
                    // minNoise는 이제 0f고 maxNoise는 이제 1임.
                    noiseMap[y * mapWidth + x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y * mapWidth + x]);
                }
            }
            return noiseMap;
        }

        public static float[] GenerateIslandGradientMap(int mapWidth, int mapHeight)
        {
            float[] map = new float[mapWidth * mapHeight];

            for(int x=0; x< mapWidth; x++)
            {
                for(int y=0; y < mapHeight; y++)
                {
                    // Value (0 ~ 1) where  * 2 -1 makes Value (-1 ~ 0)
                    float i = x / (float)mapWidth * 2 - 1;
                    float j = y / (float)mapHeight * 2 - 1;

                    // Find edge of the map.
                    float value = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j));

                    // Apply curve graph to have more values around 0 on the edge,
                    // and more values >= 3 in the middle.
                    // basically, without applying this it makes a very tiny island.
                    float a = 3;
                    float b = 2.2f;
                    float islandGradientValue = Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));

                    map[y * mapWidth + x] = islandGradientValue;
                }
            }

            return map;
        }
    }
}

