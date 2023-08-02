using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Commands
{
    public class Clear : BaseCommand
    {
        public override string Execute()
        {
            Terminal _terminal = GameObject.FindGameObjectWithTag("terminal").GetComponent<Terminal>();
            _terminal.ClearTerminal();
            return "clearing...";
        }

        public override void ProcessArguments(string cmd)
        {
        }
    }
}
