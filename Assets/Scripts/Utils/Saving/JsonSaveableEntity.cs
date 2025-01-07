using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

[ExecuteAlways]
public class JsonSaveableEntity : MonoBehaviour
{
    [SerializeField] private string uniqueIdentifier = "";
    
    private static Dictionary<string, JsonSaveableEntity> globalLookup = new Dictionary<string, JsonSaveableEntity>();

    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    /// <returns>Unique identifier string.</returns>
    public string GetUniqueIdentifier()
    {
        return uniqueIdentifier;
    }

    /// <summary>
    /// Generates a new unique identifier for this entity.
    /// </summary>
    public void GenerateUniqueIdentifier()
    {
        uniqueIdentifier = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Captures the state of all IJsonSaveable components attached to this entity
    /// and returns it as a JObject.
    /// </summary>
    /// <returns>A JToken representing the captured state of the entity.</returns>
    public JToken CaptureAsJtoken()
    {
        JObject state = new JObject();
        IDictionary<string, JToken> stateDict = state;

        // Capture state from all IJsonSaveable components
        foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
        {
            JToken token = jsonSaveable.CaptureAsJToken();
            string componentType = jsonSaveable.GetType().ToString();
            Debug.Log($"{name} Capture {componentType} = {token.ToString()}");
            stateDict[componentType] = token;
        }

        return state;
    }

    /// <summary>
    /// Restores the state of this entity from the provided JToken. 
    /// The entity will attempt to restore from each relevant IJsonSaveable component.
    /// </summary>
    /// <param name="stateToken">A JToken representing the saved state of the entity.</param>
    public void RestoreFromJToken(JToken stateToken)
    {
        JObject state = stateToken.ToObject<JObject>();
        IDictionary<string, JToken> stateDict = state;

        // Restore state for each IJsonSaveable component based on the state data
        foreach (IJsonSaveable jsonSaveable in GetComponents<IJsonSaveable>())
        {
            string componentType = jsonSaveable.GetType().ToString();
            if (stateDict.ContainsKey(componentType))
            {
                Debug.Log($"{name} Restore {componentType} => {stateDict[componentType].ToString()}");
                jsonSaveable.RestoreFromJToken(stateDict[componentType]);
            }
        }
    }

    /// <summary>
    /// Checks whether the provided unique identifier is unique within the global lookup.
    /// </summary>
    /// <param name="candidate">The unique identifier to check.</param>
    /// <returns>True if the identifier is unique; otherwise, false.</returns>
    private bool IsUnique(string candidate)
    {
        if (!globalLookup.ContainsKey(candidate)) return true;

        JsonSaveableEntity existingEntity = globalLookup[candidate];

        // Ensure we only return true for unique identifiers
        if (existingEntity == this) return true;  // This is the current entity, so it's valid.
        if (existingEntity == null || existingEntity.GetUniqueIdentifier() != candidate)
        {
            // Clean up removed entries and ensure uniqueness
            globalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
}