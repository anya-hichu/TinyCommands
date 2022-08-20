namespace TinyCmds.Commands;

using System;
using System.Linq;

using Dalamud.Interface.Windowing;
using Dalamud.Logging;

using TinyCmds.Attributes;
using TinyCmds.Chat;
using TinyCmds.Ui;
using TinyCmds.Utils;


[Command("/tinyhelp")]
[Arguments("command...?")]
[Summary("Displays usage/help for the plugin's commands")]
[Aliases("/thelp", "/tinycmd", "/tcmd")]
[HelpMessage(
	"This command displays the extended usage and help for plugin commands. Run it alone for basic general information.",
	"",
	"You can pass the \"-o\" flag to close all help windows. If you also pass a (partial) command name, that command's help will be displayed after all other windows are closed."
)]
[PluginCommandHelpHandler]
public class DisplayCommandHelp: PluginCommand {
	protected override void Execute(string? command, string rawArguments, FlagMap flags, bool verbose, bool dryRun, ref bool showHelp) {
		if (flags['o']) {
			foreach (Window wnd in this.Plugin.helpWindows.Values)
				wnd.IsOpen = false;
		}
		string[] args = rawArguments.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		PluginLog.Information($"args: #{args.Length}, {string.Join(" ", args.Select(s => $"[{s}]"))}");
		if (args.Length < 1) {
			if (this.Plugin is null)
				PluginLog.Error($"{this.InternalName}.Plugin is null");
			else if (this.Plugin.helpWindows["<PLUGIN>"] is null)
				PluginLog.Error($"Default help window is null");
			else
				this.Plugin.helpWindows["<PLUGIN>"].IsOpen = !flags['o'];
			return;
		}
		foreach (string listing in args) {
			string wanted = listing.TrimStart('/').ToLower();
			foreach (PluginCommand cmd in Plugin.commandManager.commands) {
				if (cmd.CommandComparable.Equals(wanted) || cmd.AliasesComparable.Contains(wanted)) {
					if (this.Plugin.helpWindows.TryGetValue(cmd.CommandComparable, out Window? wnd)) {
						wnd.IsOpen = true;
					}
					else {
						ChatUtil.ShowPrefixedError(
							"An internal error occured - the help window for ",
							ChatColour.HIGHLIGHT_FAILED,
							cmd.Command,
							ChatColour.RESET,
							" could not be found."
						);
					}
					return;
				}
			}
			ChatUtil.ShowPrefixedError($"Couldn't find plugin command '/{wanted}'");
		}
	}
}
