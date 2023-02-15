#if UNITY_EDITOR

using UnityEditor;

namespace Build1.UnityBuildNumber.Editor
{
    internal static class BuildNumberMenu
    {
        private const string AutoIncrementEnabledMenuItem = "Tools/Build1/Build Number/Auto Increment/Enable";
        private const string AutoIncrementDisableMenuItem = "Tools/Build1/Build Number/Auto Increment/Disable";
        
        static BuildNumberMenu()
        {
            EditorApplication.delayCall += UpdateMenu; 
        }
        
        [MenuItem(AutoIncrementEnabledMenuItem, false, 1020)]
        public static void AutoIncrementEnabled()
        {
            if (BuildNumberProcessor.SetAutoIncrementEnabled(true))
                UpdateMenu();
        }

        [MenuItem(AutoIncrementEnabledMenuItem, true, 1021)]
        public static bool AutoIncrementEnabledValidation()
        {
            return !BuildNumberProcessor.GetAutoIncrementEnabled();
        }
        
        [MenuItem(AutoIncrementDisableMenuItem, false, 1021)]
        public static void AutoIncrementDisabled()
        {
            if (BuildNumberProcessor.SetAutoIncrementEnabled(false))
                UpdateMenu();
        }
        
        [MenuItem(AutoIncrementDisableMenuItem, true, 1021)]
        public static bool AutoIncrementDisabledValidation()
        {
            return BuildNumberProcessor.GetAutoIncrementEnabled();
        }

        [MenuItem("Tools/Build1/Build Number/Auto Increment/Info", false, 1050)]
        public static void AutoIncrementInfo()
        {
            EditorUtility.DisplayDialog("Auto Increment",
                                        "When Enabled, increments build number and updates Player Settings if applicable for current platform.", 
                                        "Got it!");
        }
        
        [MenuItem("Tools/Build1/Build Number/Increment", false, 1070)]
        public static void Increment()
        {
            BuildNumberProcessor.Increment();
        }
        
        [MenuItem("Tools/Build1/Build Number/Decrement", false, 1071)]
        public static void Decrement()
        {
            BuildNumberProcessor.Decrement();
        }

        [MenuItem("Tools/Build1/Build Number/Reset", false, 1120)]
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
        
        [MenuItem("Tools/Build1/Build Number/Tool Window...", false, 1150)]
        public static void ToolsWindow()
        {
            BuildNumberWindow.Open();
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