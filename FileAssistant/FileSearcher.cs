using FileAssistant.Models;
using System.Text.RegularExpressions;

namespace FileAssistant;

public class FileSearcher
{
    public static async Task SearchWordInFileAsync(string directory, string word, CancellationToken token)
    {
        var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        int totalFiles = files.Length;
        int filesProcessed = 0;
        int totalMatches = 0;

        var stats = new List<FileStats>();

        foreach ( var file in files )
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("The operation was canceled by the user.");
                break;
            }
            try
            {
                string content = await File.ReadAllTextAsync(file, token);
                int count = Regex.Matches(content, Regex.Escape(word)).Count;
                if (count > 0)
                {
                    stats.Add(new FileStats { FilePath = file, WordCount = count });
                    totalMatches += count;
                }
                filesProcessed++;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation canceled.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {file}: {ex.Message}");
            }


            
        }
        Console.WriteLine($"{stats.Count}");
        StatisticsWriter.WriteStatistics(stats, "SearchResult");

    }
}
