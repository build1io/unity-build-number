#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Build1.UnityBuildNumber.Editor
{
    internal abstract class BuildNumberProcessor
    {
        public const string AutoIncrementEnabledKey = "Build1_BuildNumber_AutoIncrementEnabled";

        private const string BuildNumberFileFullName = BuildNumber.FileName + ".txt";
        private const string BuildNumberFolderPath   = "/Resources";
        private const string BuildNumberFilePath     = BuildNumberFolderPath + "/" + BuildNumberFileFullName;

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

            buildNumber = Mathf.Max(buildNumber + 1, 0);
            Set(buildNumber);
        }

        public static void Decrement()
        {
            if (!TryGetBuildNumberFromPlayerSettings(out var buildNumber))
                buildNumber = ReadBuildNumberFromFile();

            buildNumber = Mathf.Max(buildNumber - 1, 0);
            Set(buildNumber);
        }

        public static void Set(int buildNumber)
        {
            TrySetBuildNumberToPlayerSettings(buildNumber);
            WriteBuildNumberToFile(buildNumber);
        }

        public static void Reset()
        {
            TrySetBuildNumberToPlayerSettings(0);
            WriteBuildNumberToFile(0);
        }

        public static bool TryUpdateBuildNumberFromProjectSettings()
        {
            if (!TryGetBuildNumberFromPlayerSettings(out var buildNumber)) 
                return false;
            WriteBuildNumberToFile(buildNumber);
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
            var target = EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = buildNumber.ToString();
                    return true;

                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = buildNumber;
                    return true;

                // WebGL doesn't have build number in PlayerSettings.
                case BuildTarget.WebGL:
                    return false;

                // Exception for future platforms.
                default:
                    throw new Exception($"BuildNumber: Not implemented for build target [{target}]");
            }
        }

        private static int ReadBuildNumberFromFile()
        {
            return BuildNumber.Get();
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

            Debug.Log($"BuildNumber: Set to {buildNumber}");
        }
    }
}

#endif