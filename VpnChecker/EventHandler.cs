using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using MEC;
using Log = Exiled.API.Features.Log;
using Exiled.API.Features.Roles;
namespace VpnChecker
{
    public class EventHandler
    {

        public void RegisterEvents()
        {
            Log.Info("...........Registering Events.........");
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Joined += OnPlayerConnected;
        }

        public void UnRegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Joined -= OnPlayerConnected;
        }

        public void OnPlayerConnected(JoinedEventArgs ev)
        {
            Log.Info(ev.Player.IPAddress);
            Log.Info(VpnChecker.IsVpn(ev.Player.IPAddress));
            

        }
        public void OnWaitingForPlayers()
        {
            var task = Task.Run(async () => await VpnChecker.UpdateListAsync());
            Log.Info("Getting VPN Ip Ranges!");

        }
    }
}
