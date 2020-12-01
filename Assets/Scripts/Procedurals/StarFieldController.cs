using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class StarFieldController : MonoBehaviour
{
    [SerializeField]
    private int numOfStars;
	[SerializeField]
	private int dst = 10;
	[SerializeField]
	private int numVertsPerStar = 5;
	[SerializeField]
	private Material mat;

	Mesh mesh;
	Camera cam;

    private void Start()
    {
        Generate();
    }
    private void Generate()
    {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Star Fields";

		var tris = new List<int>();
        var verts = new List<Vector3>();
        var uvs = new List<Vector2>();

        for (int starIndex = 0; starIndex < numOfStars; starIndex++)
        {
            var dir = Random.onUnitSphere;
            var (circleVerts, circleTris, circleUvs) = GenerateCircle(dir, verts.Count);
            verts.AddRange(circleVerts);
            tris.AddRange(circleTris);
            uvs.AddRange(circleUvs);
        }

		mesh.SetVertices(verts);
		mesh.SetTriangles(tris, 0, true);
		mesh.SetUVs(0, uvs);
		var meshRenderer = GetComponent<MeshRenderer>();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		meshRenderer.sharedMaterial = mat;
		meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
	}
	(Vector3[] verts, int[] tris, Vector2[] uvs) GenerateCircle(Vector3 dir, int indexOffset)
	{
		float size = Random.Range(2, 5);

		var axisA = Vector3.Cross(dir, Vector3.up).normalized;
		if (axisA == Vector3.zero)
		{
			axisA = Vector3.Cross(dir, Vector3.forward).normalized;
		}
		var axisB = Vector3.Cross(dir, axisA);
		var centre = dir * dst;

		Vector3[] verts = new Vector3[numVertsPerStar + 1];
		Vector2[] uvs = new Vector2[numVertsPerStar + 1];
		int[] tris = new int[numVertsPerStar * 3];

		verts[0] = centre;
		uvs[0] = new Vector2(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));

		for (int vertIndex = 0; vertIndex < numVertsPerStar; vertIndex++)
		{
			float currAngle = (vertIndex / (float)(numVertsPerStar)) * Mathf.PI * 2;
			var vert = centre + (axisA * Mathf.Sin(currAngle) + axisB * Mathf.Cos(currAngle)) * size;
			verts[vertIndex + 1] = vert;
			uvs[vertIndex + 1] = new Vector2(0, Random.Range(0.1f, 1f));

			if (vertIndex < numVertsPerStar)
			{
				tris[vertIndex * 3 + 0] = 0 + indexOffset;
				tris[vertIndex * 3 + 1] = (vertIndex + 1) + indexOffset;
				tris[vertIndex * 3 + 2] = ((vertIndex + 1) % (numVertsPerStar) + 1) + indexOffset;
			}
		}

		return (verts, tris, uvs);
	}
}
