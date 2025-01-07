using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Static methods for working with JSON data.
/// </summary>
public static class JsonStatics
{
    /// <summary>
    /// Converts a Vector3 to a JToken.
    /// </summary>
    /// <param name="vector">The Vector3 to convert.</param>
    /// <returns>A JToken representing the Vector3.</returns>
    public static JToken ToToken(this Vector3 vector)
    {
        JObject state = new JObject();
        IDictionary<string, JToken> stateDict = state;
        stateDict["x"] = vector.x;
        stateDict["y"] = vector.y;
        stateDict["z"] = vector.z;
        return state;
    }

    /// <summary>
    /// Converts a JToken to a Vector3.
    /// </summary>
    /// <param name="state">The JToken to convert.</param>
    /// <returns>A Vector3 representing the JToken.</returns>
    public static Vector3 ToVector3(this JToken state)
    {
        Vector3 vector = new Vector3();
        if (state is JObject jObject)
        {
            IDictionary<string, JToken> stateDict = jObject;

            if (stateDict.TryGetValue("x", out JToken x))
            {
                vector.x = x.ToObject<float>();
            }

            if (stateDict.TryGetValue("y", out JToken y))
            {
                vector.y = y.ToObject<float>();
            }

            if (stateDict.TryGetValue("z", out JToken z))
            {
                vector.z = z.ToObject<float>();
            }
        }
        return vector;
    }
}
