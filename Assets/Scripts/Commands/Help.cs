using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Fonbot.Commands
{
    public class Help : BaseCommand
    {
        [SerializeField] private CommandsList _commands;

        public override string Execute()
        {
            StringBuilder _stringBuilder = new StringBuilder();
            foreach (var cmd in _commands.Commands)
            {
                _stringBuilder.Append($"{cmd.CommandName}: {cmd.CommandDescription}\n");
            }

            return _stringBuilder.ToString();
        }

        public override void ProcessArguments(string cmd)
        {
        }
    }
}