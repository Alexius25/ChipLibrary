using Nautilus.Commands;

namespace ChipLibrary.Debug;

public static class DebugCommands
{
    [ConsoleCommand("listchips")]
    public static string ListChips(int agr = 1)
    {
        string result = "Registered Chips:\n";
        foreach (var kvp in Handler.ChipHandler._registeredChips)
        {
            result += $"- {kvp.Key}: {kvp.Value.Name}\n";
        }
        return result;
    }
}