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
        private static String Password { get; set; }
        private static String Project { get; set; }

        public static void Main()
        {
            Console.Write("Enter admin password (32 symbols max): ");
            while ((Password = Console.ReadLine().Trim()) == "") { }

            Console.WriteLine("Enter root namespace name: ");
            while ((Project = Console.ReadLine().Trim()) == "") { }

            Int32 port = new Random().Next(1000, 19175);
            String passhash = BCrypt.Net.BCrypt.HashPassword(Password.Length <= 32 ? Password : Password.Substring(0, 32), 13);

            String[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories);
            Regex adminPassword = new Regex("Passhash = \"\\$2b\\$.*\", // Will be generated on project rename");
            Regex iisPort = new Regex("(\"(applicationUrl|launchUrl)\": .*:)\\d+(.*\")");
            Regex version = new Regex("<Version>\\d+\\.\\d+\\.\\d+</Version>");
            Regex newLine = new Regex("\\r?\\n");

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
                    extension == ".csproj" ||
                    extension == ".json")
                {
                    String content = File.ReadAllText(files[i]);
                    content = content.Replace(TemplateName, Project);
                    content = content.Replace(TemplateDbName, Project);
                    content = newLine.Replace(content, Environment.NewLine);
                    content = iisPort.Replace(content, "${1}" + port + "${3}");
                    content = version.Replace(content, "<Version>0.1.0</Version>");
                    content = adminPassword.Replace(content, "Passhash = \"" + passhash + "\",");

                    File.WriteAllText(files[i], content, Encoding.UTF8);
                }
            }

            Console.WriteLine();

            String[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory(), "*" + TemplateName + "*", SearchOption.AllDirectories);
            directories = directories.Where(directory => !directory.StartsWith(Path.Combine(Directory.GetCurrentDirectory(), "tools"))).ToArray();
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
        }
    }
}
