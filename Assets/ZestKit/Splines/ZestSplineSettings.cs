using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Prime31.ZestKit
{
    [CreateAssetMenu(fileName = "New ZestSpline", menuName = "ZestKit/Create New ZestSplineSettings Asset")]
    public class ZestSplineSettings : ScriptableObject
    {
        public List<Vector3> Nodes;

        // From: https://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        public static void CreateAsset(string path, List<Vector3> nodes)
        {
            ZestSplineSettings asset = ScriptableObject.CreateInstance<ZestSplineSettings>();
            asset.Nodes = nodes;

            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(path));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
