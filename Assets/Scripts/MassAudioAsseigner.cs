#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

/// @class MassAudioAssigner
/// @brief A helper class for assigning AudioMixerGroups to all prefabs
public class MassAudioAssigner : EditorWindow
{
    [MenuItem("Tools/Audio/Assign Master Group to Project Prefabs")]
    public static void AssignToPrefabs()
    {

        AudioMixer mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Sounds/NewAudioMixer.mixer");
        if (mixer == null) { Debug.LogError("Nie znaleziono miksera!"); return; }

        AudioMixerGroup masterGroup = mixer.FindMatchingGroups("sfx")[0];


        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in allPrefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            AudioSource[] sources = prefab.GetComponentsInChildren<AudioSource>(true);

            if (sources.Length > 0)
            {

                using (var editingScope = new PrefabUtility.EditPrefabContentsScope(path))
                {
                    var root = editingScope.prefabContentsRoot;
                    var sourcesInScope = root.GetComponentsInChildren<AudioSource>(true);

                    foreach (var source in sourcesInScope)
                    {
                        source.outputAudioMixerGroup = masterGroup;
                    }
                }
                count += sources.Length;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Uko�czono! Zaktualizowano {count} komponent�w AudioSource w prefabach.");
    }
}
#endif