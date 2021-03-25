using UnityEngine;

namespace Build1.BuildNumber
{
    public static class BuildNumber
    {
        internal const string BuildNumberFileName = "build-number";
        
        public static int Get()
        {
            var text = Resources.Load<TextAsset>(BuildNumberFileName);
            if (text != null && int.TryParse(text.text, out var buildNumber))
                return buildNumber;
            return 0;
        }
    }
}