#if UNITY_EDITOR

using UnityEditor;

namespace Build1.BuildNumber.Editor
{
    internal static class BuildNumberMenu
    {
        private const string AutoIncrementEnabledMenuItem = "Build1/Build Number/Auto Increment/Enable";
        private const string AutoIncrementDisableMenuItem = "Build1/Build Number/Auto Increment/Disable";
        
        static BuildNumberMenu()
        {
            EditorApplication.delayCall += UpdateMenu; 
        }
        
        [MenuItem(AutoIncrementEnabledMenuItem, false, 20)]
        public static void AutoIncrementEnabled()
        {
            if (BuildNumberProcessor.SetAutoIncrementEnabled(true))
                UpdateMenu();
        }

        [MenuItem(AutoIncrementEnabledMenuItem, true, 21)]
        public static bool AutoIncrementEnabledValidation()
        {
            return !BuildNumberProcessor.GetAutoIncrementEnabled();
        }
        
        [MenuItem(AutoIncrementDisableMenuItem, false, 21)]
        public static void AutoIncrementDisabled()
        {
            if (BuildNumberProcessor.SetAutoIncrementEnabled(false))
                UpdateMenu();
        }
        
        [MenuItem(AutoIncrementDisableMenuItem, true, 21)]
        public static bool AutoIncrementDisabledValidation()
        {
            return BuildNumberProcessor.GetAutoIncrementEnabled();
        }

        [MenuItem("Build1/Build Number/Auto Increment/Info", false, 50)]
        public static void AutoIncrementInfo()
        {
            EditorUtility.DisplayDialog("Auto Increment",
                                        "When Enabled, increments build number and updates Player Settings if applicable for current platform.", 
                                        "Got it!");
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
            var enabled = BuildNumberProcessor.GetAutoIncrementEnabled();
            Menu.SetChecked(AutoIncrementEnabledMenuItem, enabled);
            Menu.SetChecked(AutoIncrementDisableMenuItem, !enabled);
        }
    }
}

#endif