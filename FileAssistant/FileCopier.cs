using FileAssistant.Models;
using System.Text.RegularExpressions;

namespace FileAssistant;
//C:\Users\Admin\Desktop\Exam

public static class FileCopier
{
    public static async Task CopyAndReplaceAsync(string directory, string word, string replacement, CancellationToken token)
    {
        var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        int totalFiles = files.Length;
        int filesProcessed = 0;
        int totalReplaced = 0; 

        var stats = new List<FileStats>();
        string outputDir = Path.Combine("Output", "Modified");
        Directory.CreateDirectory(outputDir);


        foreach ( var file in files )
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("The operation was canceled by the user.");
                break;
            }
            try
            {
                string content = await File.ReadAllTextAsync(file);
                int count = Regex.Matches(content, Regex.Escape(word)).Count;
                if (count > 0)
                {
                    string modifiedContent = content.Replace(word, replacement);
                    string relativePath = Path.GetRelativePath(directory, file);
                    string newPath = Path.Combine(outputDir, relativePath);

                    Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                    await File.WriteAllTextAsync(newPath, modifiedContent, token);

                    stats.Add(new FileStats { FilePath = newPath, WordCount = count });
                    totalReplaced += count;
                }
                filesProcessed++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File error {file}: {ex.Message}");
            }
        }

        Console.WriteLine($"Completed. Files replaced: {stats.Count}, Total substitutions: {totalReplaced}");

        StatisticsWriter.WriteStatistics(stats, "CopyReplaceResult");
    }

    

}
