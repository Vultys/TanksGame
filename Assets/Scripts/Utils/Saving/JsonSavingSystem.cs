using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MonoBehaviour for saving and loading scenes using JSON.
/// </summary>
public class JsonSavingSystem : MonoBehaviour
{
    private const string extension = ".json";

    /// <summary>
    /// Save the current scene to the provided save file.
    /// </summary>
    /// <param name="saveFile">The save file to save to.</param>
    public void Save(string saveFile)
    {
        JObject state = LoadJsonFromFile(saveFile);
        CaptureAsToken(state);
        SaveFileAsJSon(saveFile, state);
    }

    /// <summary>
    /// Load the scene from the provided save file.
    /// </summary>
    /// <param name="saveFile">The save file to load.</param>
    public void Load(string saveFile)
    {
        RestoreFromToken(LoadJsonFromFile(saveFile));
    }

    /// <summary>
    /// Load the scene from the provided save file.
    /// </summary>
    /// <param name="saveFile">The save file to load.</param>
    /// <returns>A JToken representing the state of the scene.</returns>
    private JObject LoadJsonFromFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        if (!File.Exists(path))
        {
            return new JObject();
        }
        
        using (var textReader = File.OpenText(path))
        {
            using (var reader = new JsonTextReader(textReader))
            {
                reader.FloatParseHandling = FloatParseHandling.Double;

                return JObject.Load(reader);
            }
        }
    }

    /// <summary>
    /// Save the provided state to the provided save file.
    /// </summary>
    /// <param name="saveFile">The save file to save to.</param>
    /// <param name="state">A JToken representing the state of the scene.</param>
    private void SaveFileAsJSon(string saveFile, JObject state)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Saving to " + path);
        using (var textWriter = File.CreateText(path))
        {
            using (var writer = new JsonTextWriter(textWriter))
            {
                writer.Formatting = Formatting.Indented;
                state.WriteTo(writer);
            }
        }
    }

    /// <summary>
    /// Capture the state of all JsonSaveableEntity components in the scene and save it to the provided state.
    /// </summary>
    /// <param name="state">A JObject representing the state of the scene.</param>
    private void CaptureAsToken(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
        {
            stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJtoken();
        }

        stateDict["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// Restore the state of all JsonSaveableEntity components in the scene from the provided state.
    /// </summary>
    /// <param name="state">A JToken representing the state of the scene.</param>
    private void RestoreFromToken(JObject state)
    {
        IDictionary<string, JToken> stateDict = state;
        foreach (JsonSaveableEntity saveable in FindObjectsOfType<JsonSaveableEntity>())
        {
            string id = saveable.GetUniqueIdentifier();
            if (stateDict.ContainsKey(id))
            {
                saveable.RestoreFromJToken(stateDict[id]);
            }
        }
    }

    /// <summary>
    /// Get the path to the save file from the provided save file.
    /// </summary>
    /// <param name="saveFile">The save file to get the path from.</param>
    /// <returns>The path to the save file.</returns>
    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + extension);
    }
}