using System.Diagnostics;
using System.IO;

class Program {
    private static List<string>  foundList = new List<string>();
    public static void Main(string[] args) {
        string[] directories;
        if(args.Length < 1) {
            Console.WriteLine("No directory specified, using current path");
            directories = [Directory.GetCurrentDirectory()];
        } else {
            directories = args;
        }
        int filesSearched = 0;
        foreach(string path in directories) {
            if (Directory.Exists(path)) {
                filesSearched += ProcessDirectory(path);
            } else if (File.Exists(path)) {
                ProcessFile(path);
                filesSearched++;
            }
        }
        Console.WriteLine("\nSearched " + filesSearched +" files, found " + foundList.Count + " zero-byte filled files:");
        foreach(string s in foundList) {
            Console.WriteLine(s);
        }
        Console.WriteLine("Press enter to close window");
        Console.ReadLine();
    }

    private static int ProcessDirectory(string path) {
        if(!Directory.Exists(path)) {
            throw new FileNotFoundException("Could not find the directory " + path);
        }
        int fileCount = 0;
        string[] files = Directory.GetFiles(path);
        string[] subdirectories = Directory.GetDirectories(path);
        foreach(string s in files) {
            //Console.WriteLine(s);
            ProcessFile(s);
            fileCount++;
        }
        foreach(string subPath in subdirectories) {
            fileCount += ProcessDirectory(subPath);
        }
        return(fileCount);
    }

    private static bool ProcessFile(string path) {
        if(!File.Exists(path)) {
            throw new FileNotFoundException("Could not find file " + path);
        }
        Console.WriteLine("Checking file at " + path);
        StreamReader sr = File.OpenText(path);
        do{
            int i = sr.Read();
            if(i!=0) {
                return(false);
            }
        } while(!sr.EndOfStream);
        foundList.Add(path);
        //Console.WriteLine("Found a file with only zero-bytes: " + path);
        return(true);
    }
}