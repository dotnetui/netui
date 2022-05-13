namespace Net.Essentials;

public class Persistent<T> where T : class, new()
{
    public static Func<string> DefaultBasePath = () => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static Action<string> DefaultLogAction = (message) => Debug.WriteLine(message);

    public Action<string>? LogAction = null;
    public void Log(string message)
    {
        if (LogAction == null) DefaultLogAction?.Invoke(message);
        else LogAction.Invoke(message);
    }

    volatile T? _data;
    public T? Data
    {
        get => _data;
        set
        {
            Log($"Data is being replaced for {typeof(T)}");
            _data = value;
        }
    }

    public string FilePath { get; private set; }


    public Persistent(string bucket = "Default", string? basePath = null, string? fileName = null, JsonSerializerSettings? serializerSettings = null)
    {
        this.SerializerSettings = serializerSettings ?? this.SerializerSettings;

        basePath ??= DefaultBasePath();

        var type = typeof(T);
        var typeFileName = fileName ?? type.FullName?.Split(' ')[0]?.ToFileNameHash() ?? "anonymous";
        this.FilePath = Path
            .Combine(basePath, "Persistent", bucket, typeFileName)
            .CreateDirectoryAndReturn();

        Load();
    }

    void Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                Data = JsonConvert.DeserializeObject<T>(json, SerializerSettings);
            }
        }
        finally
        {
            if (Data == null)
            {
                Data = new T();
                Save();
            }
        }
    }

    readonly object saveLock = new object();

    volatile bool isSaving = false;
    public void SaveAsync()
    {
        Task.Run(async () =>
        {
            if (isSaving) return;
            isSaving = true;
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1)); //Buffer writes
                Save();
            }
            finally
            {
                isSaving = false;
            }
        });
    }

    public void Save()
    {
        lock (saveLock)
        {
            var json = JsonConvert.SerializeObject(Data, SerializerSettings);
            File.WriteAllText(FilePath, json);
        }
    }

    public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings();
}