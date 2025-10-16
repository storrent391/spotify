using Spotify.Repository;
using Spotify.Model;
using TagLib;

namespace Spotify.Services;

public class MediaService
{
    private readonly string _uploadsFolder;

    public MediaService()
    {
        _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(_uploadsFolder))
            Directory.CreateDirectory(_uploadsFolder);
    }

    public async Task<Media?> ProcessAndInsertUploadedMedia(DatabaseConnection dbConn, Guid songId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        try
        {
            // 1️⃣ Desa el fitxer físicament
            string filePath = await SaveFile(songId, file);

            // 2️⃣ Llegeix metadades amb TagLib
            var tagFile = TagLib.File.Create(filePath);

            Console.WriteLine("=== Metadades del fitxer ===");
            Console.WriteLine($"Fitxer: {filePath}");
            Console.WriteLine($"Títol: {tagFile.Tag.Title ?? "<sense>"}");
            Console.WriteLine($"Artista(s): {(tagFile.Tag.Performers.Length > 0 ? string.Join(", ", tagFile.Tag.Performers) : "<sense>")}");
            Console.WriteLine($"Àlbum: {tagFile.Tag.Album ?? "<sense>"}");
            Console.WriteLine($"Any: {(tagFile.Tag.Year != 0 ? tagFile.Tag.Year.ToString() : "<sense>")}");
            Console.WriteLine($"Gènere(s): {(tagFile.Tag.Genres.Length > 0 ? string.Join(", ", tagFile.Tag.Genres) : "<sense>")}");
            Console.WriteLine($"Durada: {tagFile.Properties.Duration}");
            Console.WriteLine("============================");

            // 3️⃣ Desa a la BD
            Media media = new Media
            {
                Id = Guid.NewGuid(),
                Song_Id = songId,
                Url = filePath
            };

            MediaADO.Insert(dbConn, media);
            return media;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processant fitxer: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Media>> ProcessAndInsertUploadedMediaRange(DatabaseConnection dbConn, Guid songId, IEnumerable<IFormFile> files)
    {
        var result = new List<Media>();

        foreach (var file in files)
        {
            var added = await ProcessAndInsertUploadedMedia(dbConn, songId, file);
            if (added != null)
                result.Add(added);
        }

        return result;
    }

    private async Task<string> SaveFile(Guid songId, IFormFile file)
    {
        string fileName = $"{songId}_{Path.GetFileName(file.FileName)}";
        string filePath = Path.Combine(_uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }

    public Media? GetMediaById(DatabaseConnection dbConn, Guid id)
    {
        return MediaADO.GetById(dbConn, id);
    }
}
