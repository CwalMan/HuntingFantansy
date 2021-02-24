using Assets.Tilemaps;
using System.Linq;
using UnityEngine;

namespace Assets.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellularAutomata", menuName = "Algorithms/CellularAutomata")]
    public class CellularAutomata : AlgorithmBase
    {
        public int minAlive, Reptitions;

        [Tooltip("If this is checked, Replacedby will have no effect")]
        public bool ReplaceByDominantTile;

        public ObjectTileType TargetTile, ReplaceBy;

        /// <summary>
        /// 모래 타일, 물 타일, 풀 타일 뭐 등등 그 중에 풀 타일을 골라
        ///  10x10 그리드에  각 셀 하나하나 그 타일의 8개의 이웃타일에게 얼마나 많이 같은 타입이 있는지 세고
        ///  그 숫자를 통해 타일들을 같은 타입으로 두냐 아니냐를 선택한다.
        /// </summary>
        /// <param name="tilemap"></param>


        public override void Apply(TilemapStructure tilemap)
        {
            int targetTileId = (int)TargetTile;
            int replaceTileId = (int)ReplaceBy;
            for(int i =0; i< Reptitions; i++)
            {
                for(int x=0; x<tilemap.Width; x++)
                {
                    for(int y=0; y< tilemap.Height; y++)
                    {
                        var tile = tilemap.GetTileEnumNumber(x, y);
                        if(tile == targetTileId)
                        {
                            var neighbors = tilemap.GetNeighbors(x, y);

                            int targetTilesCount = neighbors.Count(a => a.Value == targetTileId);
                            

                            // 만약 minAlive 숫자가 도달하지 못하면 타일을 교체한다
                            if(targetTilesCount < minAlive)
                            {
                                if(ReplaceByDominantTile)
                                {

                                    int dominantTile = neighbors
                                        .GroupBy(a => a.Value)
                                        .OrderByDescending(a => a.Count())
                                        .Select(a => a.Key)
                                        .First();
                                    tilemap.SetTileEnumNumber(x, y, dominantTile);
                                }
                                else
                                {
                                    tilemap.SetTileEnumNumber(x, y, replaceTileId);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
