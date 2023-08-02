using System.Collections;
using System.Collections.Generic;
using Fonbot.Commands;
using UnityEngine;

namespace Fonbot.Commands
{
    public class Exit : BaseCommand
    {
        public override string Execute()
        {
            Terminal _terminal = GameObject.FindGameObjectWithTag("terminal").GetComponent<Terminal>();
            _terminal.ExitTerminal();
            return "exiting...";
        }

        public override void ProcessArguments(string cmd)
        {
        }
    }
}
