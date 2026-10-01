using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class FontReplacer : EditorWindow
{
    private Font newUIFont;
    private TMP_FontAsset newTMPFont;

    [MenuItem("Tools/Font Replacer (Scene + Prefabs)")]
    public static void ShowWindow()
    {
        GetWindow<FontReplacer>("Font Replacer");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Fonts in Scene and Prefabs", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        newUIFont = (Font)EditorGUILayout.ObjectField("New UI Font (Text)", newUIFont, typeof(Font), false);
        newTMPFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newTMPFont, typeof(TMP_FontAsset), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Replace Fonts in Current Scene"))
        {
            ReplaceFontsInScene();
        }

        if (GUILayout.Button("Replace Fonts in All Prefabs"))
        {
            ReplaceFontsInPrefabs();
        }

        if (GUILayout.Button("Replace Fonts in Scene + Prefabs"))
        {
            ReplaceFontsInScene();
            ReplaceFontsInPrefabs();
        }
    }

    void ReplaceFontsInScene()
    {
        int count = 0;

        // UI.Text
        Text[] uiTexts = FindObjectsOfType<Text>(true);
        foreach (Text t in uiTexts)
        {
            Undo.RecordObject(t, "Replace UI Font");
            t.font = newUIFont;
            EditorUtility.SetDirty(t);
            count++;
        }

        // TMP
        TextMeshProUGUI[] tmpTexts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI t in tmpTexts)
        {
            Undo.RecordObject(t, "Replace TMP Font");
            t.font = newTMPFont;
            EditorUtility.SetDirty(t);
            count++;
        }

        Debug.Log($"[Scene] Font replaced on {count} text components.");
    }

    void ReplaceFontsInPrefabs()
    {
        string[] allPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in allPrefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            bool changed = false;

            // Load prefab contents safely
            GameObject root = PrefabUtility.LoadPrefabContents(path);

            // Replace UI.Text
            Text[] uiTexts = root.GetComponentsInChildren<Text>(true);
            foreach (Text t in uiTexts)
            {
                t.font = newUIFont;
                EditorUtility.SetDirty(t);
                changed = true;
                count++;
            }

            // Replace TMP
            TextMeshProUGUI[] tmpTexts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI t in tmpTexts)
            {
                t.font = newTMPFont;
                EditorUtility.SetDirty(t);
                changed = true;
                count++;
            }

            if (changed)
            {
                PrefabUtility.SaveAsPrefabAsset(root, path);
                Debug.Log($"[Prefab] Font updated: {path}");
            }

            PrefabUtility.UnloadPrefabContents(root);
        }

        Debug.Log($"[Prefabs] Font replaced on {count} text components across prefabs.");
    }
}
