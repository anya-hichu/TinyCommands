using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Actors;
using Dalamud.Plugin;

using TinyCmds.Attributes;

namespace TinyCmds {
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Plugin command methods are delegates")]
	public partial class TinyCmdsPlugin: IDalamudPlugin {

		public string Name => "TinyCommands";
		public readonly string Prefix = "TinyCmds";
		public string PluginHelpCommand { get; private set; }

		internal DalamudPluginInterface Interface { get; private set; }
		internal Configuration Config { get; private set; }
		internal TinyCmdPluginCommandManager CommandManager { get; private set; }
		internal PluginUtil Util { get; private set; }

		public void Initialize(DalamudPluginInterface pluginInterface) {
			this.PluginHelpCommand ??= this.GetType().GetMethod(nameof(DisplayPluginCommandHelp)).GetCustomAttribute<CommandAttribute>().Command;
			this.Interface = pluginInterface;
			this.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
			this.Config.Initialize(pluginInterface);
			this.Util = new PluginUtil(this);
			this.CommandManager = new TinyCmdPluginCommandManager(this);
		}

		#region IDisposable Support
		protected virtual void Dispose(bool disposing) {
			if (!disposing) {
				return;
			}
			this.Interface.SavePluginConfig(this.Config);
			this.CommandManager.Dispose();
			this.Util.Dispose();
			this.Interface.Dispose();
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
