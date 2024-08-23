using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnChecker
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "VpnChecker1.0";
        public override string Author => "mariki";


        public override void OnEnabled()
        {
            new EventHandler().RegisterEvents();
            base.OnEnabled();
        }
    }
}
