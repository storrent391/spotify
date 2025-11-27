using Spotify.Repository;
using Spotify.Model;
using Spotify.Services;
using TagLib;

namespace Spotify.Services;

public class MediaService
{
    private readonly string _uploadsFolder;

    // public MediaService()
    // {
    //     _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

    //     if (!Directory.Exists(_uploadsFolder))
    //         Directory.CreateDirectory(_uploadsFolder);
    // }

    public async Task ProcessAndInsertUploadedMedia(DatabaseConnection dbConn, Guid songId, IFormFile file)
    {

        String filePath = await SaveImage(songId, file);
        try
        {
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

            Media media = new Media
            {
                Id = Guid.NewGuid(),
                Song_Id = songId,
                Url = filePath
            };
            MediaADO.Insert(dbConn, media);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processant fitxer: {ex.Message}");
        }
    }

    private static async Task<string> SaveImage(Guid id, IFormFile image)
    {
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = $"{id}_{Path.GetFileName(image.FileName)}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create)) 
        {
            await image.CopyToAsync(stream);
        }

        return filePath;
    }
}
