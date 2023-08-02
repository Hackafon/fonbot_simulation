using System.Collections;
using System.Collections.Generic;
using Fonbot.Commands;
using UnityEngine;

public class CommandInterpreter : MonoBehaviour
{
    [SerializeField] private CommandsList _commands;

    public string Interpret(string cmd)
    {
        string cmdName = cmd.Split(' ')[0];
        foreach (var _command in _commands.Commands)
        {
            if (_command.CommandName == cmdName)
            {
                _command.CommandAction.ProcessArguments(cmd);
                return _command.CommandAction.Execute();
            }
        }

        return $"{cmdName}: command not found";
    }
}
