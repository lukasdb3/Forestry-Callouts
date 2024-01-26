using System.IO;
using Rage;

namespace ForestryCallouts2.Backbone;

internal static class DependencyChecks
{
    internal static bool Abort;

    private static bool DagDialogueSystem()
    {
        return File.Exists("DAGDialogueSystem.dll");
    }

    private static bool GrammarPoliceAudio()
    {
        return Directory.Exists(Directory.GetCurrentDirectory()+@"\lspdfr\audio\scanner\GrammarPolice Audio");
    }

    private static bool RageNativeUi()
    {
        return File.Exists("RAGENativeUI.dll");
    }

    internal static void CheckDependencies()
    {
        if (!DagDialogueSystem())
        {
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            Game.Console.Print("MISSING DEPENDENCY!");
            Game.Console.Print("Please download and install DAGDialogueSystem.dll!");
            Game.Console.Print("If you need further assistance please send your log to https://discord.gg/ULSS");
            Game.Console.Print("FORESTRY CALLOUTS ABORTING!");
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FC FATAL ERROR",
                "~g~MISSING DEPENDENCY", 
                "DAGDialogueSystem.dll. See log for more details!");
            Abort = true;
        }
        
        if (!GrammarPoliceAudio())
        {
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            Game.Console.Print("MISSING DEPENDENCY!");
            Game.Console.Print("Please download and install Grammar Police Audio!");
            Game.Console.Print("If you need further assistance please send your log to https://discord.gg/ULSS");
            Game.Console.Print("FORESTRY CALLOUTS ABORTING!");
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FC FATAL ERROR",
                "~g~MISSING DEPENDENCY", 
                "Grammar Police Audio. See log for more details!");
            Abort = true;
        }

        if (!RageNativeUi())
        {
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            Game.Console.Print("MISSING DEPENDENCY!");
            Game.Console.Print("Please download and install RAGENativeUI!");
            Game.Console.Print("If you need further assistance please send your log to https://discord.gg/ULSS");
            Game.Console.Print("FORESTRY CALLOUTS ABORTING!");
            Game.Console.Print("--- ! FORESTRY CALLOUTS FATAL ERROR ! ---");
            
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FC FATAL ERROR",
                "~g~MISSING DEPENDENCY", 
                "RAGENativeUI. See log for more details!");
            Abort = true;
        }

        if (Abort)
        {
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~g~FORESTRY CALLOUTS ABORTED",
                "~g~FATAL ERROR OCCURED!", 
                "");
        }
    }
}