using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Prime31.ZestKit
{
    [CreateAssetMenu(fileName = "New ZestSpline", menuName = "ZestKit/Create New ZestSplineSettings Asset")]
    public class ZestSplineSettings : ScriptableObject
    {
        public List<Vector3> Nodes;

#if UNITY_EDITOR
        // From: https://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        public static void CreateAsset(string path, List<Vector3> nodes)
        {
            ZestSplineSettings asset = AssetDatabase.LoadAssetAtPath(path.Replace(Application.dataPath, "Assets"), typeof(ZestSplineSettings)) as ZestSplineSettings;
            if (asset != null)
            {
                asset.Nodes = nodes;
            }
            else
            {
                asset = ScriptableObject.CreateInstance<ZestSplineSettings>();
                asset.Nodes = nodes;
                AssetDatabase.CreateAsset(asset, path);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
#endif
    }
}
