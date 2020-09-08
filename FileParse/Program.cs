using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FileParse
{
    class Program
    {
        static void Main(string[] args)
        {
            // Enter a file path + file to be Processed.
            Console.WriteLine("Please enter a file path and file: ");
            string filePath = Console.ReadLine();

            bool exists = File.Exists(filePath);
            while (!exists)
            {
                Console.WriteLine("Please enter a file path and file: ");
                filePath = Console.ReadLine();
                exists = File.Exists(filePath);
            }

            var entries = ParseFile(filePath);
            var Ids = ParseJsonIds(entries);
            var sum = CalculateSum(Ids);
            Console.WriteLine("Total Sum of IDs: {0}", sum);
            Console.ReadKey();
        }

        /// <summary>
        /// Parses a file to see if the name and path exist.
        /// Then opens the for reading.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>List of JSON Objects</returns>
        public static List<Root> ParseFile(string fileName)
        {
            
            List<Root> entries = null;
            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine("File Found!");
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        string json = reader.ReadToEnd();
                        entries = JsonConvert.DeserializeObject<List<Root>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Input File: {0} does not exist Exception: {1}", fileName, ex);
            }
            return entries;
        }

        /// <summary>
        /// Parses the JSON ID's out from the JSON object list.
        /// </summary>
        /// <param name="jsonEntries"></param>
        /// <returns>List of Integers</returns>
        public static List<int> ParseJsonIds(List<Root> jsonEntries)
        {
            List<int> ids = new List<int>();
            try
            {
                if (jsonEntries != null || jsonEntries.Count != 0)
                {
                    foreach (var entry in jsonEntries)
                    {
                        foreach (var item in entry.menu.items)
                        {
                            if (item != null && item.label != null)
                                ids.Add(item.id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ParseJsonIds Exception: {0} ", ex);
            }
            return ids;
        }

        /// <summary>
        /// Calculates the sum of the ID's.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>Sum of Integers</returns>
        public static int CalculateSum(List<int> ids)
        {
            int sum = 0;
            foreach (var item in ids)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// JSON Objects
        /// Root
        ///     Menu:{...}
        ///     Menu:{...}
        /// </summary>
        public class Root
        {
            public Menu menu { get; set; }
        }

        /// <summary>
        /// Menu
        ///     Header:""
        ///     Items:[...]
        /// </summary>
        public class Menu
        {
            public string header { get; set; }
            public List<Items> items { get; set; }
        }

        /// <summary>
        /// Items
        ///     Id:0
        ///     Label:""
        /// </summary>
        public class Items
        {
            public int id { get; set; }
            public string label { get; set; }
        }
    }
}
