namespace Notes.Models;

internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public void Save()
    {
        File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text); 
    }

    public void Delete()
    {
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));
    }

    public Note()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }


    public static Note Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("Unable to find file on local storage.", filename);
        }

        Note ret = new()
        {
            Filename = Path.GetFileName(filename),
            Text = File.ReadAllText(filename),
            Date = File.GetLastWriteTime(filename)
        };

        return ret;
    }

    public static IEnumerable<Note> LoadAll()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        var ret = Directory
            .EnumerateFiles(appDataPath, "*.notes.txt")
            .Select(filename => Note.Load(Path.GetFileName(filename)))
            .OrderByDescending(note => note.Date);

        return ret;
    }
}