using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Tickify.Services;

namespace Tickify.Modules {
    [RequireOwner]
    public class OwnerModule : ModuleBase {
        private readonly DatabaseService _databaseService;

        public OwnerModule (DatabaseService databaseService) => _databaseService = databaseService;

        [Command("AddServer")]
        private async Task AddServerAsync () {
            await Context.Message.DeleteAsync();
            await _databaseService.AddOwnedServerAsync((SocketGuild) Context.Guild);
        }

        [Command("RemoveServer")]
        private async Task RemoveServerAsync () {
            await Context.Message.DeleteAsync();
            await _databaseService.RemoveOwnedServerAsync((SocketGuild) Context.Guild);
        }
    }
}