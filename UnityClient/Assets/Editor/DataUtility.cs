using ROIO;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class DataUtility {

    [MenuItem("UnityRO/Utils/Extract Data")]
    static void ExtractData() {
        var config = ConfigurationLoader.Init();
        FileManager.LoadGRF(config.root, config.grf);
        var descriptors = FileManager.GetFileDescriptors();

        var file = 1f;
        foreach (DictionaryEntry entry in descriptors) {
            try {
                var progress = file / descriptors.Count;
                if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Extracting data (this is going to take 40 mins+) - {progress * 100}%", progress)) {
                    break;
                }

                var path = (entry.Key as string).Trim();
                var bytes = FileManager.ReadSync(path).ToArray();
                var filename = Path.GetFileName(path);
                var extension = Path.GetExtension(filename).ToLowerInvariant();
                var dir = path.Substring(0, path.IndexOf(filename)).Replace("/", "\\");

                string assetPath = Path.Combine("Assets", "StreamingAssets", dir);

                Directory.CreateDirectory(assetPath);

                var completePath = Path.Combine(assetPath, filename);
                File.WriteAllBytes(completePath, bytes);

                if (file % 50 == 0) {
                    AssetDatabase.ImportAsset(completePath);
                }

                file++;
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        EditorUtility.ClearProgressBar();
        EditorApplication.ExitPlaymode();
    }

    // WIP
    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs")]
    static void CreateMapPrefabs() {
        var gameManager = Selection.activeGameObject.GetComponent<GameManager>();
        var map = FindMap();

        if (map != null) {
            SaveMap(map, gameManager);
        }
    }

    private static void SaveMap(GameObject mapObject, GameManager gameManager) {
        string mapName = Path.GetFileNameWithoutExtension(mapObject.name);
        string localPath = Path.Combine("Assets", "Resources", "Prefabs", "Data", "Maps", mapName);
        Directory.CreateDirectory(localPath);

        var filters = mapObject.GetComponentsInChildren<MeshFilter>();
        var renderers = mapObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < filters.Length; i++) {
            var filter = filters[i];
            var renderer = renderers[i];

            var progress = i * 1f / filters.Length;
            if (EditorUtility.DisplayCancelableProgressBar("UnityRO", $"Saving meshes - {progress * 100}%", progress)) {
                break;
            }

            var originalTexture = renderer.material.mainTexture as Texture2D;
            var id = filter.mesh.GetInstanceID();

            try {
                var texture = new Texture2D(originalTexture.width, originalTexture.height);
                texture.SetPixels(originalTexture.GetPixels());
                texture.Apply();
                texture.name = $"{id}";
                
                AssetDatabase.CreateAsset(texture, Path.Combine(localPath, id + "_texture.asset"));

                renderer.material.mainTexture = texture;
            } catch {
                Debug.LogError("Error saving " + id + "_texture.asset");
            }

            AssetDatabase.CreateAsset(filter.mesh, Path.Combine(localPath, id + ".asset"));
        }

        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath + ".prefab");
        PrefabUtility.SaveAsPrefabAssetAndConnect(mapObject, localPath, InteractionMode.UserAction);
        EditorUtility.ClearProgressBar();
        EditorApplication.ExitPlaymode();
    }

    private static GameObject FindMap() {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList().Find(go => go.tag == "Map");
    }

    [MenuItem("UnityRO/Utils/Prefabs/Create Maps Prefabs", true)]
    static bool ValidateCreateMapPrefabs() {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.GetComponent<GameManager>() != null;
    }
}
#endif