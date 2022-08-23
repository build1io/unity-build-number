#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Build1.UnityBuildNumber.Editor
{
    public sealed class BuildNumberWindow : EditorWindow
    {
        private const int Width   = 340;
        private const int Height  = 60;
        private const int Padding = 10;

        private void OnGUI()
        {
            var changed = false;
            var enabled = !Application.isPlaying;
            
            if (GUI.enabled != enabled)
            {
                GUI.enabled = enabled;
                changed = true;
            }

            GUILayout.BeginVertical();
            GUILayout.Space(Padding);
                
            GUILayout.BeginHorizontal();
            GUILayout.Space(6);

            GUILayout.Label("Build Number:", GUILayout.Width(83));
            
            var style = new GUIStyle(EditorStyles.textField)
            {
                alignment = TextAnchor.MiddleRight
            };
            GUILayout.Label(BuildNumberProcessor.Number.ToString(), style);

            if (GUILayout.Button("Increment", GUILayout.Width(85), GUILayout.Height(18)))
                BuildNumberProcessor.Increment();

            if (GUILayout.Button("Decrement", GUILayout.Width(85), GUILayout.Height(18)))
                BuildNumberProcessor.Decrement();

            GUILayout.Space(6);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(3);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(Padding);
            GUILayout.Space(83);
            
            var autoIncrementEnabled = BuildNumberProcessor.GetAutoIncrementEnabled();
            var autoIncrementEnabledNew = GUILayout.Toggle(autoIncrementEnabled, "Auto increment");
            if (autoIncrementEnabled != autoIncrementEnabledNew)
                BuildNumberProcessor.SetAutoIncrementEnabled(autoIncrementEnabledNew);

            GUILayout.FlexibleSpace();
            
            GUILayout.Space(Padding);
            GUILayout.EndHorizontal();

            GUILayout.Space(Padding);
            GUILayout.EndVertical();
            
            if (changed)
                GUI.enabled = !enabled;
        }
        
        /*
         * Static.
         */

        public static void Open()
        {
            var main = EditorGUIUtility.GetMainWindowPosition();
            var centerWidth = (main.width - Width) * 0.5f;
            var centerHeight = (main.height - Height) * 0.5f;
            
            var window = GetWindow<BuildNumberWindow>(false, "Build Number", true);
            window.position = new Rect(main.x + centerWidth, main.y + centerHeight, Width, Height);
            window.minSize = new Vector2(Width, Height);
        }
    }
}

#endif