using FileAssistant.Models;

namespace FileAssistant;

public static class StatisticsWriter
{
    public static void WriteStatistics(List<FileStats> stats, string fileName)
    {
        Directory.CreateDirectory("Statistics");
        string path = Path.Combine("Statistics", $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

        var lines = stats.Select(s => $"{s.FilePath} — {s.WordCount} occurrences");
        File.WriteAllLines(path, lines);
    }

}
