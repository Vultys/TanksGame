using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private JsonSavingSystem _savingSystem;

    [SerializeField] private string _defaultSaveName = "save";

    /// <summary>
    /// Saves the current game state to the default save file.
    /// </summary>
    public void SaveGame()
    {
        _savingSystem.Save(_defaultSaveName);
    }   

    /// <summary>
    /// Loads the game state from the default save file.
    /// </summary>
    public void LoadGame()
    {
        _savingSystem.Load(_defaultSaveName);
    } 
}
