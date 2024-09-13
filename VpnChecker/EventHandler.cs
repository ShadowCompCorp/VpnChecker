using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.Handlers;
using MEC;
using Exiled.API.Features.Roles;
using VpnChecker.VoiceCrasherProtect;
using Exiled.API.Features;
namespace VpnChecker
{
    public class EventHandler
    {

        public void RegisterEvents()
        {
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
            var subnetmask =VpnChecker.GetSubnetRange(ev.Player.IPAddress);
            if (VpnChecker.IsVpn(ev.Player.IPAddress))
            {
                Timing.CallDelayed(10f, () =>
                {
                    WebHook.SendDiscordWebhook(Plugin.__instance.Config.WebHookUrl, null, "Detected VPN!", $"**Antecheat Detected Players VPN**\nPlayer {ev.Player.Nickname} Was Detected For Using VPN.\nServerIp: {Exiled.API.Features.Server.IpAddress}:{Exiled.API.Features.Server.Port}\nIp: {ev.Player.IPAddress} VpnRange (Start: {subnetmask.StartIp}, End: {subnetmask.EndIp})", Color: 0x2929ff);
                });
                
            }
            

        }
        public void OnWaitingForPlayers()
        {
            var task = Task.Run(async () => await VpnChecker.UpdateListAsync());
            Log.Info("Getting VPN Ip Ranges!");

        }
    }
}
