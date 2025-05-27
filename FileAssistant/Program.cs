
using System.Threading;

namespace FileAssistant
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-*** File Assistant ***-");
                Console.WriteLine("-*1. Search Word In File *-");
                Console.WriteLine("-*2. Copy And Replace word *-");
                Console.WriteLine("-*3. Find Classes And Interfaces *-");
                Console.WriteLine("-*0. Exit *-");
                Console.Write("Choose activity ): ");

                string choice = Console.ReadLine();

                if (choice == "0") break;

                using var cts = new CancellationTokenSource();

                // Запуск задачі, що чекатиме натискання Escape для скасування
                _ = Task.Run(() =>
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        cts.Cancel();
                });

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter path to folder: ");
                            string searchDir = Console.ReadLine()!;
                            if (!Directory.Exists(searchDir))
                            {
                                Console.WriteLine("Directory does not exist. Please check the path and try again.");
                                continue;
                            }

                            Console.Write("enter word for search: ");
                            string word = Console.ReadLine()!;
                            await FileSearcher.SearchWordInFileAsync(searchDir, word, cts.Token);
                            break;

                        case "2":
                            Console.Write("Enter path to folder for copy: ");
                            string copyDir = Console.ReadLine()!;
                            Console.Write("Enter word for replace: ");
                            string searchWord = Console.ReadLine()!;
                            Console.Write("Enter word for replace on: ");
                            string replaceWord = Console.ReadLine()!;
                            await FileCopier.CopyAndReplaceAsync(copyDir, searchWord, replaceWord, cts.Token);
                            break;

                        case "3":
                            Console.Write("Enter the path to the folder for analyzing .cs files: ");
                            string csDir = Console.ReadLine()!;
                            await CSharpAnalyzer.FindClassesAndInterfacesAsync(csDir, cts.Token);
                            break;

                        default:
                            Console.WriteLine("False option. Try again.");
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation canceled.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred.: {ex.Message}");
                }

                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }


    }
}

