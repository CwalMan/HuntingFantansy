using Assets.Tilemaps;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "Poisson Spawner", menuName = "Algorithms/Mob Spawner")]
public class PoissonSamp : ScriptableObject
{
    public GameObject mobPrefab;
    public GroundTileType[] spawnTileType;

    [Range(2,100)]
    public float radius = 10; 
    public readonly int RejectionSamples = 30; // if tried 30 time but all have been rejected, quit it and skip to next random Point.

    public List<Vector2> GenerateSpawnPoint(float radius, Vector2 sampleRegionSize, int numSampleBeforeRejection = 30)
    {
        List<Vector2> points = new List<Vector2>(); // 'about to be spawned' List
        List<Vector2> spawnPoints = new List<Vector2>(); // if points is spawned, add to list.

        float cellSize = radius / Mathf.Sqrt(2); //so each grid cell will contain at most one sample

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];


        spawnPoints.Add(sampleRegionSize/2);

        while (spawnPoints.Count > 0) // step 2.
        {
            int randomSpawnIndex = UnityEngine.Random.Range(0,spawnPoints.Count); // first sample is choosen randomly
            Vector2 randomSpawn = spawnPoints[randomSpawnIndex]; // pick randomSample
            bool candidateElected = false;

            for (int i = 0; i < numSampleBeforeRejection; i++) // try 30 time.
            {
                float angle = UnityEngine.Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector2 candidate = randomSpawn + dir * UnityEngine.Random.Range(radius, 2 * radius);
                // candidate should be moved atleast 'radius' unit away from any other sample.

                if (IsFarEnoughToValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateElected = true;
                    break;
                }
            }
            if (!candidateElected)
            {
                spawnPoints.RemoveAt(randomSpawnIndex);
            }
        }
        return points;
    }

    bool IsFarEnoughToValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        // check if candidate is on grid.
        {
            // cellSize = Vector2.x / cellsize
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            // get 
            int searchStartX = Mathf.Max(0, cellX - 2); // -1
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1); // 1
            int searchStartY = Mathf.Max(0, cellY - 2); // -1
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1); // 1

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] -1 ;

                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

}
