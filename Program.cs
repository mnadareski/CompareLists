namespace CompareLists
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Check for proper arguments
            if (args == null || args.Length < 2 || args.Length > 3)
            {
                DisplayHelp();
                return;
            }

            // Set local variables
            string listfile1 = args[0];
            string listfile2 = args[1];
            string? outlistfile = args.Length == 3 ? args[2] : null;

            // If either of the files doesn't exist
            if (!File.Exists(listfile1))
            {
                Console.WriteLine($"{listfile1} does not exist!");
                return;
            }
            else if (!File.Exists(listfile2))
            {
                Console.WriteLine($"{listfile1} does not exist!");
                return;
            }

            // Get the matches from the input files, if possible
            long matchcount = GetMatches(listfile1, listfile2, outlistfile);
            Console.WriteLine($"{matchcount} matching items found!");
        }

        /// <summary>
        /// Display commandline help
        /// </summary>
        private static void DisplayHelp()
        {
            Console.WriteLine("CompareLists Usage:");
            Console.WriteLine("CompareLists.exe <listfile1> <listfile2> [outlistfile]");
            Console.WriteLine();
            Console.WriteLine("Each value in the listfiles should be on its own line.");
            Console.WriteLine("If outlistfile is provided, the items in both lists will be written there.");
        }

        /// <summary>
        /// Get the matches between two listfiles
        /// </summary>
        private static long GetMatches(string listfile1, string listfile2, string? outlistfile = null)
        {
            var listfile1Contents = File.ReadAllLines(listfile1);
            var listfile2Contents = File.ReadAllLines(listfile2);
            var matchedContents = listfile1Contents.Intersect(listfile2Contents);

            // If we're writing out for a file
            if (outlistfile != null)
            {
                // Santize the outlistfile path and create directories
                outlistfile = Path.GetFullPath(outlistfile);
                Directory.CreateDirectory(Path.GetDirectoryName(outlistfile));

                // Prepare output for writing
                using var outlistfileStream = File.OpenWrite(outlistfile);
                using var outlistfileWriter = new StreamWriter(outlistfileStream);

                // Write the list, one item per row
                foreach (string content in matchedContents)
                {
                    outlistfileWriter.WriteLine(content);
                }
            }

            return matchedContents.LongCount();
        }
    }
}