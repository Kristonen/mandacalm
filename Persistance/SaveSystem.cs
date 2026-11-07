using System.Text.Json;
using System.Text.Json.Serialization;
using Mandacalm.Core;

namespace Mandacalm.Persistance;

public sealed class SaveSystem
{
    private readonly string _path;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter()},
    };
    public SaveSystem()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MandaCalm"
        );
        Directory.CreateDirectory(folder);
        _path = Path.Combine(folder, "save.json");
    }

    public void Save(GameSnapShot shot)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(shot, Options);
            string tempPath = _path + ".tmp";
            File.WriteAllText(tempPath, jsonString);
            File.Move(tempPath, _path, true);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Save failed: {e.Message}");
        }
    }

    public GameSnapShot? Load()
    {
        if (!File.Exists(_path))
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<GameSnapShot>(json, Options);
        }
        catch (Exception e) when (e is IOException or JsonException or UnauthorizedAccessException)
        {
            Console.WriteLine($"Load failed: {e.Message}");
            return null;
        }
    }
}