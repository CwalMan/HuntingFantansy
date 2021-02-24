using System;
using Assets.Tilemaps;
public class RandomDataGenerator : IWorldGenerator
{
    public void Apply(TilemapStructure tilemap)
    {
        var validEnumValues = (GroundTileType[])Enum.GetValues(typeof(GroundTileType));

        for(int x=0; x< tilemap.Width; x++)
        {
            for(int y=0; y<tilemap.Height;y++)
            {
                var randomValue = (int)validEnumValues[UnityEngine.Random.Range(0, validEnumValues.Length)];

                tilemap.SetTileEnumNumber(x, y, randomValue);
            }
        }
    }
}
