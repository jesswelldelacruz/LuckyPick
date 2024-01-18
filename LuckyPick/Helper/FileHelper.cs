using System.IO;

namespace LuckyPick.Helper
{
    public class FileHelper
    {
        public static void Export(string game, string input)
        {
            string path = $@"C:\Lotto";
            string gamePath = path + $@"\{game}.txt";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(gamePath))
            {
                using (StreamWriter sw = File.CreateText(gamePath))
                {
                    sw.WriteLine(input);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(gamePath))
                {
                    sw.WriteLine(input);
                }
            }
        }
    }
}
