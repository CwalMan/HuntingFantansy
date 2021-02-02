using System;
using Assets.Tilemaps;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName ="RandomGeneration",menuName ="Algorithms/RandomGeneration")]
    public class RandomGenerationAlgorithm : AlgorithmBase
    {
        public override void Apply(TilemapStructure tilemap)
        {
            TileTypeEnum[] validEnumValues = (TileTypeEnum[])Enum.GetValues(typeof(TileTypeEnum));

            for(int x= 0; x< tilemap.Width; x++)
            {
                for(int y=0; y< tilemap.Height;y++)
                {
                    int randomValue = (int)validEnumValues[UnityEngine.Random.Range(0, validEnumValues.Length)];

                    tilemap.SetTile(x, y, randomValue);
                }
            }
        }
    }
}
