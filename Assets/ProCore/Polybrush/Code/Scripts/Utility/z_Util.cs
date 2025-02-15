using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Polybrush
{
	/**
	 *	General static helper functions.
	 */
	public static class z_Util
	{
		/**
		 *	Returns a new array initialized with the @count and @value.
		 */
		public static T[] Fill<T>(T value, int count)
		{
			T[] arr = new T[count];
			for(int i = 0; i < count; i++)
				arr[i] = value;
			return arr;
		}

		/**
		 *	Returns a new array initialized with the @count and @value.
		 */
		public static T[] Fill<T>(System.Func<int, T> constructor, int count)
		{
			T[] arr = new T[count];
			for(int i = 0; i < count; i++)
				arr[i] = constructor(i);
			return arr;
		}

		/**
		 *	Returns a new dictionary initialized with the @count and @value.
		 */
		public static Dictionary<K, V> InitDictionary<K, V>(System.Func<int, K> keyFunc, System.Func<int, V> valueFunc, int count)
		{
			Dictionary<K, V> dic = new Dictionary<K, V>(count);

			for(int i = 0; i < count; i++)
			{
				dic.Add( keyFunc(i), valueFunc(i) );
			}

			return dic;
		}

		public static string ToString<T>(this IEnumerable<T> enumerable, string delim)
		{
			return string.Join(delim, enumerable.Select(x => x.ToString()).ToArray());
		}

		public static string ToString<K,V>(this Dictionary<K,V> dictionary, string delim)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach(var kvp in dictionary)
			{
				sb.AppendLine(kvp.Key.ToString() + ": " + kvp.Value.ToString());
			}
			return sb.ToString();
		}

		/**
		 *	Returns a new dictionary with a key for every value and it's value set to the key that it came from.  Ex:
		 * {
		 * 	{ 0, {1, 2, 3} },
		 *  { 1, {4, 5, 6} }
		 * }
		 * becomes
		 * { 
		 *	 {1, 0},
		 *	 {2, 0},
		 *	 {3, 0},
		 *	 {4, 1},
		 *	 {5, 1},
		 *	 {6, 1},
		 * }
		 */
		public static Dictionary<K, T> SetValuesAsKey<T, K>(this Dictionary<T, IEnumerable<K>> dic)
		{
			Dictionary<K, T> lookup = new Dictionary<K, T>();

			foreach(var kvp in dic)
			{
				foreach(K val in kvp.Value)
				{
					lookup.Add(val, kvp.Key);
				}
			}

			return lookup;
		}

		/**
		 *	Similar to SetValuesAsKey except that instead of assigning the value to key, the value is instead the index.
		 */
		public static Dictionary<T, int> GetCommonLookup<T>(this List<List<T>> lists)
		{
			Dictionary<T, int> lookup = new Dictionary<T, int>();

			int index = 0;

			foreach(var kvp in lists)
			{
				foreach(var val in kvp)
				{
					lookup.Add(val, index);
				}

				index++;
			}

			return lookup;
		}

		/**
		 *	Lerp between 2 colors.
		 */
		public static Color Lerp(Color lhs, Color rhs, float alpha)
		{
			return new Color(	lhs.r * (1f-alpha) + rhs.r * alpha,
								lhs.g * (1f-alpha) + rhs.g * alpha,
								lhs.b * (1f-alpha) + rhs.b * alpha,
								lhs.a * (1f-alpha) + rhs.a * alpha );
		}

		/**
		 *	Clamp an animation curve's first and last keys.
		 */
		public static AnimationCurve ClampAnimationKeys(	AnimationCurve curve,
															float firstKeyTime,
															float firstKeyValue,
															float secondKeyTime,
															float secondKeyValue)
		{
			Keyframe[] keys = curve.keys;
			int len = curve.length - 1;

			keys[0].time = firstKeyTime;
			keys[0].value = firstKeyValue;
			keys[len].time = secondKeyTime;
			keys[len].value = secondKeyValue;

			curve.keys = keys;
			return new AnimationCurve(keys);
		}

		public static System.Enum Next(this System.Enum value)
		{
			int max = System.Enum.GetNames(value.GetType()).Length;
			return (System.Enum) System.Enum.ToObject(value.GetType(), (System.Convert.ToInt32(value) + 1) % max);
		}

		/**
		 *	True if object is non-null and valid.
		 */
		public static bool IsValid<T>(this T target) where T : z_IValid
		{
			return target != null && target.valid;
		}

		/**
		 *	Returns a new name with incremented prefix.
		 */
		internal static string IncrementPrefix(string prefix, string name)
		{
			string str = name;

			Regex regex = new Regex("^(" + prefix + "[0-9]*_)");
			Match match = regex.Match(name);

			if( match.Success )
			{
				string iteration = match.Value.Replace(prefix, "").Replace("_", "");
				int val = 0;

				if(int.TryParse(iteration, out val))
				{
					str = name.Replace(match.Value, prefix + (val+1) + "_");
				}
				else
					str = prefix + "0_" + name;
			}
			else
			{
				str = prefix + "0_" + name;
			}

			return str;
		}

		/**
		 *	Get the mesh in use by either MeshFilter or SkinnedMeshRenderer
		 */
		public static Mesh GetMesh(this GameObject gameObject)
		{
			MeshFilter mf = gameObject.GetComponent<MeshFilter>();

			if(mf != null && mf.sharedMesh != null)
				return mf.sharedMesh;
				
			SkinnedMeshRenderer smr = gameObject.GetComponent<SkinnedMeshRenderer>();

			if(smr != null && smr.sharedMesh != null)
				return smr.sharedMesh;
			else
				return null;
		}

		/**
		 * Sets the MeshFilter.sharedMesh or SkinnedMeshRenderer.sharedMesh to @m.
		 */
		public static void SetMesh(this GameObject gameObject, Mesh mesh)
		{
			if( gameObject != null )
			{
				MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

				if(meshFilter != null)
				{
					meshFilter.sharedMesh = mesh;
				}
				else
				{
					SkinnedMeshRenderer mr = gameObject.GetComponent<SkinnedMeshRenderer>();

					if(mr != null)
						mr.sharedMesh = mesh;
				}
			}
		}

		/**
		 *	Convert a list of IGrouping values to a dictionary.
		 */
		public static Dictionary<T, List<K>> ToDictionary<T, K>(this IEnumerable<IGrouping<T, K>> groups)
		{
			Dictionary<T, List<K>> dic = new Dictionary<T, List<K>>();

			foreach(IGrouping<T, K> g in groups)
				dic.Add(g.Key, g.ToList());

			return dic;
		}
	}
}
