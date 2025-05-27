using FileAssistant.Models;
using System.Text.RegularExpressions;


namespace FileAssistant;

public static class CSharpAnalyzer
{
    public static async Task FindClassesAndInterfacesAsync(string directory, CancellationToken token)
    {
        var files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
        int totalFiles = files.Length;
        int filesProcessed = 0;
        int totalFound = 0;

        var stats = new List<FileStats>();

        var regex = new Regex(@"\b(class|interface)\s+(\w+)", RegexOptions.Compiled);

        foreach (var file in files)
        {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("The operation was canceled by the user.");
                    break;
                }

                try
                {
                    string content = await File.ReadAllTextAsync(file, token);
                    var matches = regex.Matches(content);

                    if (matches.Count > 0)
                    {
                        stats.Add(new FileStats
                        {
                            FilePath = file,
                            WordCount = matches.Count
                        });

                        totalFound += matches.Count;
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("File reading was canceled.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }

                filesProcessed++;
            }

            Console.WriteLine($"Found {totalFound} classes/interfaces in {stats.Count} files.");
            StatisticsWriter.WriteStatistics(stats, "ClassInterfaceSearch");
    }
}
