using UnityEngine;

public class Ocean : FluidVolume
{
	public override float WaveFunction(Vector3 worldPoint)
	{
		// Use a custom wave function
		
		return Mathf.Sin(worldPoint.x * 0.5f + Time.time) * 0.8f + Mathf.Sin(worldPoint.z * 0.5f + Time.time) * 0.2f;
	}
	
	public void FixedUpdate()
	{
		// Update the mesh using FluidVolume.ProjectPointOntoSurface()
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		
		for (int i = 0; i < vertices.Length; i++)
		{
			// Transform the vertex to worldspace
			Vector3 worldVertex = transform.TransformPoint(vertices[i]);
			
			// Project the worldspace vertex onto the ocean surface
			Vector3 projectedWorldVertex = ProjectPointOntoSurface(worldVertex);
			
			// Transform the projected worldspace vertex to localspace
			Vector3 projectedLocalVertex = transform.InverseTransformPoint(projectedWorldVertex);
			
			// Store the new vertex position
			vertices[i] = projectedLocalVertex;
		}
		
		mesh.vertices = vertices;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}