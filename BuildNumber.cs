using UnityEngine;

namespace Build1.UnityBuildNumber
{
    public static class BuildNumber
    {
        internal const string FileName = "build-number";
        
        public static int Get()
        {
            var text = Resources.Load<TextAsset>(FileName);
            if (text != null && int.TryParse(text.text, out var buildNumber))
                return buildNumber;
            return 0;
        }
    }
}