using UnityEngine;
using System.Collections.Generic;

namespace Polybrush
{
	/**
	 *	A set of Prefabs.
	 */
	[CreateAssetMenuAttribute(menuName = "Polybrush/Prefab Palette", fileName = "Prefab Palette", order = 802)]
	[System.Serializable]
	public class z_PrefabPalette : ScriptableObject, z_IHasDefault
	{
		public List<GameObject> prefabs;

		public void SetDefaultValues()
		{
			prefabs = new List<GameObject>()
			{
				null
			};
		}
	}
}
