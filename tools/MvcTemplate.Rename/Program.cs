using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcTemplate.Rename
{
    public class Program
    {
        private const String TemplateDbName = "Mvc6Template";
        private const String TemplateName = "MvcTemplate";
        private static String Project { get; set; }
        private static String Company { get; set; }

        public static void Main()
        {
            Console.WriteLine("Enter root namespace name: ");
            while ((Project = Console.ReadLine().Trim()) == "")
            { }

            Console.WriteLine("Enter company name: ");
            Company = Console.ReadLine().Trim();

            String[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories);
            Regex authors = new Regex("^  \"authors\": \\[ \"NonFactors\" \\]");
            Regex version = new Regex("^  \"version\": \"\\d+\\.\\d\\.\\d+\"");
            Regex newLine = new Regex("(?<!\\r)\\n");

            Console.WriteLine();

            for (Int32 i = 0; i < files.Length; i++)
            {
                Console.CursorTop -= 1;
                Console.WriteLine(String.Format("Renaming content...     {0}%", ((Int32)(((i + 1) / files.Length) * 100)).ToString().PadLeft(3)));

                String extension = Path.GetExtension(files[i]);
                if (extension == ".cs" ||
                    extension == ".cshtml" ||
                    extension == ".config" ||
                    extension == ".gitignore" ||
                    extension == ".sln" ||
                    extension == ".xproj" ||
                    extension == ".json")
                {
                    String content = File.ReadAllText(files[i]);
                    content = content.Replace(TemplateName, Project);
                    content = content.Replace(TemplateDbName, Project);
                    content = newLine.Replace(content, Environment.NewLine);
                    content = version.Replace(content, "  \"version\": \"0.1.0\"");
                    content = authors.Replace(content, "  \"authors\": [ \"" + Company + "\" ]");

                    File.WriteAllText(files[i], content, Encoding.UTF8);
                }
            }

            Console.WriteLine();

            String[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*" + TemplateName + "*", SearchOption.AllDirectories);
            for (Int32 i = 0; i < directories.Length; i++)
            {
                Console.CursorLeft = 0;
                Console.Write(String.Format("Renaming directories... {0}%", ((Int32)(((i + 1) / directories.Length) * 100)).ToString().PadLeft(3)));

                String projectDir = Path.Combine(Directory.GetParent(directories[i]).FullName, directories[i].Split('\\').Last().Replace(TemplateName, Project));
                Directory.Move(directories[i], projectDir);
            }

            Console.WriteLine();

            files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + TemplateName + "*", SearchOption.AllDirectories);
            files = files.Where(file => !file.Contains($"{TemplateName}.Rename.cmd")).ToArray();
            for (Int32 i = 0; i < files.Length; i++)
            {
                Console.CursorLeft = 0;
                Console.Write(String.Format("Renaming files...       {0}%", ((Int32)(((i + 1) / files.Length) * 100)).ToString().PadLeft(3)));

                String projectFile = Path.Combine(Directory.GetParent(files[i]).FullName, files[i].Split('\\').Last().Replace(TemplateName, Project));
                File.Move(files[i], projectFile);
            }

            Console.WriteLine();

            if (Directory.Exists("tools"))
                Directory.Delete("tools", true);

            if (File.Exists("CONTRIBUTING.md"))
                File.Delete("CONTRIBUTING.md");

            if (File.Exists("LICENSE.txt"))
                File.Delete("LICENSE.txt");

            if (File.Exists("README.md"))
                File.WriteAllText("README.MD", "");

            if (File.Exists($"{TemplateName}.Rename.cmd"))
                File.Delete($"{TemplateName}.Rename.cmd");
        }
    }
}
