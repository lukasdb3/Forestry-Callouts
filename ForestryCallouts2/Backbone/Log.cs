#region Refrences
//Rage
using System;
using ForestryCallouts2.Backbone.Functions;
using Rage;
//LSPDFR
using LSPD_First_Response.Mod.Callouts;
//ForestryCallouts2
using ForestryCallouts2.Backbone.IniConfiguration;
using ForestryCallouts2.Backbone.Menu;
using ForestryCallouts2.Backbone.SpawnSystem;

#endregion

namespace ForestryCallouts2.Backbone
{
    internal static class Log
    {
        internal static void CallDebug(Callout call, string m)
        {
            Game.LogTrivial("-!!- ForestryCallouts - [DEBUG - "+call+"] >> "+m+"");
        }

        internal static void Debug(string clas, string m)
        {
            Game.LogTrivial("-!!- ForestryCallouts - [DEBUG - "+ clas.ToUpper() + "] >> " + m +"");
        }

        internal static void Error(string clas, string e, string m)
        {
            Game.LogTrivial("--- ! FORESTRY CALLOUTS WARNING ! ---");
            Game.LogTrivial("ERROR: "+e);
            Game.LogTrivial("-- ERROR INFO --");
            Game.LogTrivial("Error From: "+clas.ToUpper());
            Game.LogTrivial(m);
            Game.LogTrivial("Please send your log to Unofficial LSPDFR Discord Support Server (ULSS)");
            Game.LogTrivial("https://discord.gg/ULSS");
            Game.LogTrivial("--- ! FORESTRY CALLOUTS WARNING ! ---");
        }
    }
}