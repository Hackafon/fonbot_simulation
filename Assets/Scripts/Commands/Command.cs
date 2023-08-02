using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fonbot.Common
{
    [CreateAssetMenu(fileName = "Command", menuName = "Fonbot/Command")]
    public class Command : ScriptableObject
    {
        [SerializeField] private string _commandName;
        [SerializeField] private string _commandDescription;
        [SerializeField] private UnityEvent _commandAction;

        public string CommandName => _commandName;
        public string CommandDescription => _commandDescription;
        public UnityEvent CommandAction => _commandAction;
    }
}