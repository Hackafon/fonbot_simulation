using System.Collections;
using System.Collections.Generic;
using Fonbot.Commands;
using UnityEngine;

namespace Fonbot.Commands
{
    [CreateAssetMenu(fileName = "CommandsList", menuName = "Fonbot/CommandsList")]
    public class CommandsList : ScriptableObject
    {
        [SerializeField] private List<CommandInfo> _commandsList;

        public List<CommandInfo> Commands => _commandsList;
    }
}