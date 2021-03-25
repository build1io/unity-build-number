#if UNITY_EDITOR

using UnityEditor;

namespace Build1.BuildNumber.Editor
{
    internal static class BuildNumberMenu
    {
        private const string AutoIncrementMenuItem = "Build1/Build Number/Auto Increment";
        
        static BuildNumberMenu()
        {
            EditorApplication.delayCall += UpdateMenu; 
        }
        
        [MenuItem(AutoIncrementMenuItem, false, 20)]
        public static void AutoIncrement()
        {
            if (BuildNumberProcessor.SetAutoIncrementEnabled(!BuildNumberProcessor.GetAutoIncrementEnabled()))
                UpdateMenu();
        }
        
        [MenuItem("Build1/Build Number/Increment", false, 70)]
        public static void Increment()
        {
            BuildNumberProcessor.Increment();
        }
        
        [MenuItem("Build1/Build Number/Decrement", false, 71)]
        public static void Decrement()
        {
            BuildNumberProcessor.Decrement();
        }

        [MenuItem("Build1/Build Number/Reset", false, 120)]
        public static void Reset()
        {
            var currentBuildNumber = BuildNumber.Get();
            var result = EditorUtility.DisplayDialog("Reset Build Number?",
                                        $"Are you sure you want to reset build number?\n\nCurrent build number: {currentBuildNumber}",
                                        "Reset",
                                        "Cancel");
            if (result)
                BuildNumberProcessor.Reset();
        }
        
        /*
         * Private.
         */

        private static void UpdateMenu()
        {
            Menu.SetChecked(AutoIncrementMenuItem, BuildNumberProcessor.GetAutoIncrementEnabled());
        }
    }
}

#endif