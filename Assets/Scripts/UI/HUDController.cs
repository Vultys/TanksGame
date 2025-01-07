using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private JsonSavingSystem _savingSystem;

    [SerializeField] private string _defaultSaveName = "save";

    public void SaveGame()
    {
        _savingSystem.Save(_defaultSaveName);
    }   

    public void LoadGame()
    {
        _savingSystem.Load(_defaultSaveName);
    } 
}
