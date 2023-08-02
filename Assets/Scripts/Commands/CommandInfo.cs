using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Fonbot.Commands
{
    [CreateAssetMenu(fileName = "Command", menuName = "Fonbot/Command")]
    public class CommandInfo : ScriptableObject
    {
        [SerializeField] private string _commandName;
        [SerializeField] private string _commandDescription;
        [SerializeField] private BaseCommand _commandAction;

        public string CommandName => _commandName;
        public string CommandDescription => _commandDescription;
        public BaseCommand CommandAction => _commandAction;
    }
}