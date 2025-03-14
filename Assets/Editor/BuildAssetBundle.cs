using UnityEditor;

namespace EditorTools
{
    public static class BuildAssetBundle
    {
        [MenuItem("Assets/Build AssetBundle")]
        static void Build()
        {
            string assetBundleDirectory = "Assets/AssetBundles";

            if (!FSOperations.DirectoryExists(assetBundleDirectory))
            {
                FSOperations.CreateDirectory(assetBundleDirectory);
            }

            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }
    }
}