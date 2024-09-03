using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MapExtras.Common {
	public static class Util {
		public static Transform GetChildByPath(Transform parent, string path) {
			Transform currentTransform = parent;
			string[] pathSteps = path.Split('/');
			foreach (string pathStep in pathSteps) {
				currentTransform = GetChildByName(currentTransform, pathStep);

				if (currentTransform == null) {
					return null;
				}
			}

			return currentTransform;
		}

		public static Transform GetChildByName(Transform parent, string name) {
			if (parent == null) {
				foreach (Transform root in SceneRoots()) {
					if (root.name == name) {
						return root;
					}
				}
			}
			else {
				for (int i = 0; i < parent.childCount; i++) {
					Transform child = parent.GetChild(i);
					if (child.name == name) {
						return child;
					}
				}
			}

			return null;
		}
		
		public static IEnumerable<Transform> SceneRoots() {
			/*var property = new HierarchyProperty(HierarchyType.GameObjects);
			var expanded = new int[0];
			while (property.Next(expanded)) {
				GameObject gameObject = property.pptrValue as GameObject;
				
				if (gameObject != null) {
					yield return gameObject.transform;
				}
			}*/
			yield return null;
		}
	}
}