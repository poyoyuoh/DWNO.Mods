using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DWNOLib;
public class Logger
{
    public enum LogType
    {
        Message,
        Debug,
        Info,
        Warning,
        Error
    }

    public static Dictionary<LogType, ConsoleColor> Colors { get; private set; } = new Dictionary<LogType, ConsoleColor>
    {
        { LogType.Message, ConsoleColor.White },
        { LogType.Debug, ConsoleColor.DarkGray },
        { LogType.Info, ConsoleColor.Green },
        { LogType.Warning, ConsoleColor.Yellow },
        { LogType.Error, ConsoleColor.Red },
    };

    public static void Log(object text, LogType type = LogType.Info)
    {
        ConsoleColor previous_color = Console.BackgroundColor;
        ConsoleManager.SetConsoleColor(GetColor(type));
        string formattedtext = $"[{type,-7}:{DWNOLib.PluginName,10}] " + text.ToString();
        ConsoleManager.ConsoleStream?.Write(formattedtext + "\n");
        ConsoleManager.SetConsoleColor(previous_color);
        TextWriter LogWriter = BepInEx.Logging.Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault().LogWriter;
        LogWriter?.WriteLine(formattedtext);
        LogWriter?.Flush();
    }

    public static void Log(object text, ConsoleColor color, string type = "Log")
    {
        ConsoleColor previous_color = Console.BackgroundColor;
        ConsoleManager.SetConsoleColor(color);
        string formattedtext = $"[{type,-7}:{DWNOLib.PluginName,10}] " + text.ToString();
        ConsoleManager.ConsoleStream?.Write(formattedtext + "\n");
        ConsoleManager.SetConsoleColor(previous_color);
        TextWriter LogWriter = BepInEx.Logging.Logger.Listeners.OfType<DiskLogListener>().FirstOrDefault().LogWriter;
        LogWriter?.WriteLine(formattedtext);
        LogWriter?.Flush();
    }

    public static ConsoleColor GetColor(LogType type)
    {
        ConsoleColor color;
        if (Colors.ContainsKey(type))
            color = Colors[type];
        else
            color = ConsoleColor.DarkGray;

        return color;
    }
}
