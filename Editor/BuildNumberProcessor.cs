#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Build1.UnityBuildNumber.Editor
{
    [InitializeOnLoad]
    internal abstract class BuildNumberProcessor
    {
        public const string AutoIncrementEnabledKey = "Build1_BuildNumber_AutoIncrementEnabled";

        private const string BuildNumberFileFullName = BuildNumber.FileName + ".txt";
        private const string BuildNumberFolderPath   = "/Resources";
        private const string BuildNumberFilePath     = BuildNumberFolderPath + "/" + BuildNumberFileFullName;

        static BuildNumberProcessor()
        {
            if (!File.Exists(BuildNumberFilePath))
                WriteBuildNumberToFile(TryGetBuildNumberFromPlayerSettings(out var buildNumber) ? buildNumber : 1);
        }
        
        public static bool GetAutoIncrementEnabled()
        {
            return EditorPrefs.GetBool(AutoIncrementEnabledKey);
        }

        public static bool SetAutoIncrementEnabled(bool enabled)
        {
            if (GetAutoIncrementEnabled() == enabled)
                return false;

            EditorPrefs.SetBool(AutoIncrementEnabledKey, enabled);

            Debug.Log(enabled
                          ? "BuildNumber: Auto Increment enabled."
                          : "BuildNumber: Auto Increment disabled.");

            return true;
        }

        public static void Increment()
        {
            if (!TryGetBuildNumberFromPlayerSettings(out var buildNumber))
                buildNumber = ReadBuildNumberFromFile();    

            buildNumber = Mathf.Max(buildNumber + 1, 1);
            Set(buildNumber);
        }

        public static void Decrement()
        {
            if (!TryGetBuildNumberFromPlayerSettings(out var buildNumber))
                buildNumber = ReadBuildNumberFromFile();

            buildNumber = Mathf.Max(buildNumber - 1, 1);
            Set(buildNumber);
        }

        public static void Set(int buildNumber)
        {
            TrySetBuildNumberToPlayerSettings(buildNumber);
            WriteBuildNumberToFile(buildNumber);
            
            Debug.Log($"BuildNumber: Set to {buildNumber}");
        }

        public static void Reset()
        {
            TrySetBuildNumberToPlayerSettings(1);
            WriteBuildNumberToFile(1);
            
            Debug.Log("BuildNumber: Set to 1");
        }

        public static bool TryUpdateBuildNumberFromProjectSettings()
        {
            if (!TryGetBuildNumberFromPlayerSettings(out var buildNumber)) 
                return false;
            
            WriteBuildNumberToFile(buildNumber);
            
            Debug.Log($"BuildNumber: Set to {buildNumber}");
            
            return true;
        }

        /*
         * Private.
         */

        private static bool TryGetBuildNumberFromPlayerSettings(out int buildNumber)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case BuildTarget.iOS:
                    return int.TryParse(PlayerSettings.iOS.buildNumber, out buildNumber);

                case BuildTarget.Android:
                    buildNumber = PlayerSettings.Android.bundleVersionCode;
                    return true;

                // WebGL doesn't have build number in PlayerSettings.
                case BuildTarget.WebGL:
                    buildNumber = -1;
                    return false;

                // Exception for future platforms.
                default:
                    throw new Exception($"BuildNumber: Not implemented for build target [{target}]");
            }
        }

        private static bool TrySetBuildNumberToPlayerSettings(int buildNumber)
        {
            PlayerSettings.iOS.buildNumber = buildNumber.ToString();
            PlayerSettings.Android.bundleVersionCode = buildNumber;
            
            AssetDatabase.SaveAssets();
            
            return true;
        }
        
        /*
         * File.
         */

        private static int ReadBuildNumberFromFile()
        {
            if (File.Exists(BuildNumberFilePath))
            {
                var content = System.IO.File.ReadAllText(BuildNumberFilePath);
                if (int.TryParse(content, out var buildNumber))
                    return buildNumber;    
            }
            return 0;
        }
        
        private static void WriteBuildNumberToFile(int buildNumber)
        {
            var buildNumberString = buildNumber.ToString();
            var path = Application.dataPath + BuildNumberFilePath;
            if (System.IO.File.Exists(path))
            {
                var buildNumberCurrent = System.IO.File.ReadAllText(path);
                if (buildNumberString == buildNumberCurrent)
                    return;
            }

            var folderPath = Application.dataPath + BuildNumberFolderPath;
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            System.IO.File.WriteAllText(path, buildNumberString);
        }
    }
}

#endif