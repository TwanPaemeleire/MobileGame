using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    int Testing = 5;
}

public class SaveLoadHandler : MonoSingleton<SaveLoadHandler>
{
    private SaveFile _saveFile;
    public SaveFile SaveFile {  get { return _saveFile; } set { _saveFile = value; } }

    private Task _saveTask = null;
    public Task SaveTask => _saveTask;

    private Task<string> _loadTask = null;
    public Task LoadTask => _loadTask;

    string _saveFileName = "SaveFile.json";

    protected override void Init()
    {
        //RequestSave();
        RequestLoad();
    }

    public async void RequestSave()
    {
        await Save();
    }

    private async Task Save()
    {
        string saveFileJson = JsonUtility.ToJson(SaveFile);
        _saveTask = File.WriteAllTextAsync(Application.persistentDataPath + _saveFileName, saveFileJson);
        await SaveTask;
        _saveTask = null;
    }

    public async void RequestLoad()
    {
        await Load();
    }

    private async Task Load()
    {
        if (_loadTask != null) return;
        _loadTask = File.ReadAllTextAsync(Application.persistentDataPath + _saveFileName);
        await _loadTask;
        string saveFileJson = _loadTask.Result;
        _loadTask = null;
        _saveFile = JsonUtility.FromJson<SaveFile>(saveFileJson);
        OnLoadFinished();
    }

    private void OnLoadFinished()
    {
       
    }
}
