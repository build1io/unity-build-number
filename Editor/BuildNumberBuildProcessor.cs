#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Build1.UnityBuildNumber.Editor
{
    internal sealed class BuildNumberBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!BuildNumberProcessor.GetAutoIncrementEnabled())
                return;
            
            Debug.Log("BuildNumber: Auto Increment");
            
            BuildNumberProcessor.Increment();
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (!BuildNumberProcessor.GetAutoIncrementEnabled())
                return;
            
            switch (report.summary.result)
            {
                case BuildResult.Failed:
                    EditorApplication.delayCall += () =>
                    {
                        Debug.Log("BuildNumber: Reverting build number due to build error...");    
                        BuildNumberProcessor.Decrement();
                    };
                    break;
                    
                case BuildResult.Cancelled:
                    EditorApplication.delayCall += () =>
                    {
                        Debug.Log("BuildNumber: Reverting build number as build was cancelled...");    
                        BuildNumberProcessor.Decrement();
                    };
                    break;
            }
        }
    }
}

#endif