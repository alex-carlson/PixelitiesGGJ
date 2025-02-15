using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Polybrush
{

	/**
	 * Geometry math and Array extensions.
	 */
	public static class z_Math
	{
#region Geometry

		/**
		 * Returns true if a raycast intersects a triangle.
		 * http://en.wikipedia.org/wiki/M%C3%B6ller%E2%80%93Trumbore_intersection_algorithm
		 * http://www.cs.virginia.edu/~gfx/Courses/2003/ImageSynthesis/papers/Acceleration/Fast%20MinimumStorage%20RayTriangle%20Intersection.pdf
		 */
		public static bool RayIntersectsTriangle(Ray InRay, Vector3 InTriangleA,  Vector3 InTriangleB,  Vector3 InTriangleC, out float OutDistance, out Vector3 OutPoint)
		{
			OutDistance = 0f;
			OutPoint = Vector3.zero;
			
			Vector3 e1, e2;  //Edge1, Edge2
			Vector3 P, Q, T;
			float det, inv_det, u, v;
			float t;

			//Find vectors for two edges sharing V1
			e1 = InTriangleB - InTriangleA;
			e2 = InTriangleC - InTriangleA;

			//Begin calculating determinant - also used to calculate `u` parameter
			P = Vector3.Cross(InRay.direction, e2);
			
			//if determinant is near zero, ray lies in plane of triangle
			det = Vector3.Dot(e1, P);

			// Non-culling branch
			// {
				if(det > -Mathf.Epsilon && det < Mathf.Epsilon)
					return false;

				inv_det = 1f / det;

				//calculate distance from V1 to ray origin
				T = InRay.origin - InTriangleA;

				// Calculate u parameter and test bound
				u = Vector3.Dot(T, P) * inv_det;

				//The intersection lies outside of the triangle
				if(u < 0f || u > 1f)
					return false;

				//Prepare to test v parameter
				Q = Vector3.Cross(T, e1);

				//Calculate V parameter and test bound
				v = Vector3.Dot(InRay.direction, Q) * inv_det;

				//The intersection lies outside of the triangle
				if(v < 0f || u + v  > 1f)
					return false;

				t = Vector3.Dot(e2, Q) * inv_det;
			// }

			if(t > Mathf.Epsilon)
			{ 
				//ray intersection
				OutDistance = t;

				OutPoint.x = (u * InTriangleB.x + v * InTriangleC.x + (1-(u+v)) * InTriangleA.x);
				OutPoint.y = (u * InTriangleB.y + v * InTriangleC.y + (1-(u+v)) * InTriangleA.y);
				OutPoint.z = (u * InTriangleB.z + v * InTriangleC.z + (1-(u+v)) * InTriangleA.z);

				return true;
			}

			return false;
		}
#endregion

#region Normal and Tangents

		/**
		 * Calculate the unit vector normal of 3 points:  B-A x C-A
		 */
		public static Vector3 Normal(Vector3 p0, Vector3 p1, Vector3 p2)
		{
			Vector3 cross = Vector3.Cross(p1 - p0, p2 - p0);
			if (cross.magnitude < Mathf.Epsilon)
				return new Vector3(0f, 0f, 0f); // bad triangle
			else
			{
				return cross.normalized;
			}
		}
		
		/**
		 * If p.Length % 3 == 0, finds the normal of each triangle in a face and returns the average.
		 * Otherwise return the normal of the first three points.
		 */
		public static Vector3 Normal(Vector3[] p)
		{
			if(p.Length < 3)
				return Vector3.zero;

			if(p.Length % 3 == 0)
			{
				Vector3 nrm = Vector3.zero;

				for(int i = 0; i < p.Length; i+=3)
					nrm += Normal(	p[i+0], 
									p[i+1], 
									p[i+2]);

				return nrm / (p.Length/3f);
			}
			else
			{
				Vector3 cross = Vector3.Cross(p[1] - p[0], p[2] - p[0]);
				if (cross.magnitude < Mathf.Epsilon)
					return new Vector3(0f, 0f, 0f); // bad triangle
				else
				{
					return cross.normalized;
				}
			}
		}

        /**
		 * Returns the first normal, tangent, and bitangent for this face, using the first triangle available for tangent and bitangent.
		 * Does not rely on mesh.msh for normal or uv information - uses mesh.vertices & mesh.uv.
		 */
		public static void NormalTangentBitangent(Vector3[] vertices, Vector2[] uv, int[] tri, out Vector3 normal, out Vector3 tangent, out Vector3 bitangent)
		{
			normal = Normal(vertices[tri[0]], vertices[tri[1]], vertices[tri[2]]);

			Vector3 tan1 = Vector3.zero;
			Vector3 tan2 = Vector3.zero;
			Vector4 tan = new Vector4(0f,0f,0f,1f);

			long i1 = tri[0];
			long i2 = tri[1];
			long i3 = tri[2];

			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];

			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float r = 1.0f / (s1 * t2 - s2 * t1);

			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1 += sdir;
			tan2 += tdir;

			Vector3 n = normal;

			Vector3.OrthoNormalize(ref n, ref tan1);

			tan.x = tan1.x;
			tan.y = tan1.y;
			tan.z = tan1.z;

			tan.w = (Vector3.Dot(Vector3.Cross(n, tan1), tan2) < 0.0f) ? -1.0f : 1.0f;

			tangent = ((Vector3)tan) * tan.w;
			bitangent = Vector3.Cross(normal, tangent);
		}
#endregion

#region Algebra

		/**
		 *	Average of a Vector3[].
		 */
		public static Vector3 Average(Vector3[] array, IEnumerable<int> indices)
		{
			Vector3 avg = Vector3.zero;
			int count = 0;

			foreach(int i in indices)
			{
				avg.x += array[i].x;
				avg.y += array[i].y;
				avg.z += array[i].z;

				count++;
			}

			return avg / count;
		}

		/**
		 *	Returns a weighted average from values @array, @indices, and a lookup table of index weights.
		 */
		public static Vector3 WeightedAverage(Vector3[] array, IList<int> indices, Dictionary<int, float> weightLookup)
		{
			float sum = 0f;
			Vector3 avg = Vector3.zero;

			for(int i = 0; i < indices.Count; i++)
			{
				float weight = weightLookup[indices[i]];
				avg.x += array[indices[i]].x * weight;
				avg.y += array[indices[i]].y * weight;
				avg.z += array[indices[i]].z * weight;
				sum += weight;
			}

			return sum > Mathf.Epsilon ? avg /= sum : Vector3.zero;
		}

		/**
		 *	True if all elements of a vector are equal.
		 */
		public static bool VectorIsUniform(Vector3 vec)
		{
			return Mathf.Abs(vec.x - vec.y) < Mathf.Epsilon && Mathf.Abs(vec.x - vec.z) < Mathf.Epsilon;
		}
#endregion
	}
}
