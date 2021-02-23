using Assets.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Test : MonoBehaviour
{
	[Range(1f,100f)]
	public float radius = 10;
	public Vector2 regionSize = Vector2.one;
	public int rejectionSamples = 30;
	public float displayRadius = 1;

	public List<Vector2> points;
	public PoissonSamp samp;
	public GameObject prefab;



	private void Start()
	{

	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			for (int i = 0; i < points.Count; i++)
			{
				GameObject gameObject = Instantiate(prefab, new Vector3(points[i].x, points[i].y, 0), Quaternion.identity);
				gameObject.transform.parent = this.transform;
			}
		}
	}
	
	void OnValidate()
	{
		points = PoissonSampling.GeneratePoints(radius, regionSize, rejectionSamples);
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(regionSize / 2, regionSize);
		if (points != null)
		{
			foreach (Vector2 point in points)
			{
				Gizmos.DrawWireSphere(point, displayRadius);
			}
		}
	}
}
