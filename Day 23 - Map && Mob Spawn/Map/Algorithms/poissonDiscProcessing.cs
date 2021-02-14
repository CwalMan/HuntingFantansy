using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poissonDiscProcessing : MonoBehaviour
{
	public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
	{
		List<Vector2> points = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		float cellSize = radius / Mathf.Sqrt(2); // so  each grid cell will contain at most one sample

		int cols = Mathf.FloorToInt(sampleRegionSize.x / cellSize);
		int rows = Mathf.FloorToInt(sampleRegionSize.y / cellSize);

		Vector2[] grid = new Vector2[cols * rows];

		int x = Mathf.FloorToInt((sampleRegionSize.x /2 ) / cellSize);
		int y = Mathf.FloorToInt((sampleRegionSize.y /2 ) / cellSize);
		Vector2 pos = new Vector2(sampleRegionSize.x / 2, sampleRegionSize.y / 2);

		grid[x + y * cols] = pos;

		while (spawnPoints.Count > 0)
		{
			int randomIndex = Random.Range(0, spawnPoints.Count);
			Vector2 randomPos = spawnPoints[randomIndex];
			bool elected = false;
			for(int i = 0; i < numSamplesBeforeRejection; i++)
			{

			}
		}
		{

		}
			return points;
	}


}
