using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace TheCrew2Fix;
class Program
{
    enum Modes : int
    {
        None = 0,
        Borderless = 1,
        Fullscreen = 2
    }
    static async Task<int> Main(string[] args)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+ "\\The Crew 2\\PCScalability.xml";
        if (!File.Exists(path))
        {
            Console.WriteLine("Config file (" + path + ") not exist!");
            AnyKeyExit(-2);
            return -2;
        }
        XmlDocument doc = new();
        doc.Load(path);
        Console.WriteLine("Current mode: "+ GetCurrentAppearance(doc));
        doc.DocumentElement.SetAttribute("Appearance", "1");
        Console.WriteLine("Set value to Borderless.");
        doc.Save(path);
        Console.WriteLine("Config saved.");
        Console.WriteLine("Launching Game...");
        Process.Start(new ProcessStartInfo("uplay://launch/2855/0") { UseShellExecute = true });
        Console.WriteLine("Game started. Exit in 3 seconds.");
        await Task.Delay(3000);
        return 0;
    }


    static void AnyKeyExit(int code)
    {
        Console.WriteLine("Press any key to quit...");
        Console.ReadKey();
        Environment.Exit(code);
    }

    static string? GetCurrentAppearance(XmlDocument doc)
    {
        try
        {
            Modes mode = (Modes)Int32.Parse(doc.DocumentElement.GetAttribute("Appearance"));
            if (mode == Modes.None)
            {
                return "Windowed";
            }
            else if (mode == Modes.Borderless)
            {
                return "Borderless";
            }
            else if (mode == Modes.Fullscreen) { 
                return "Fullscreen";
            }
            else
            {
                return "Unknown";
            }

        } catch (Exception ex) {
            Console.WriteLine("Failed to parse config.");
            Console.WriteLine(ex.Message);
            AnyKeyExit(-1);
            return null;
        }
    }
}