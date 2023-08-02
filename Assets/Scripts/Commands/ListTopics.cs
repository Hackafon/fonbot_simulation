using System.Collections;
using System.Collections.Generic;
using System.Text;
using Fonbot.Commands;
using Fonbot.Common;
using UnityEngine;

namespace Fonbot.Commands
{
    public class ListTopics : BaseCommand
    {
        [SerializeField] private Topic[] _topics;

        public override string Execute()
        {
            StringBuilder _stringBuilder = new StringBuilder();
            foreach (var _topic in _topics)
            {
                _stringBuilder.Append($"/{_topic.topicName}\n");
            }

            return _stringBuilder.ToString();
        }

        public override void ProcessArguments(string cmd)
        {
        }
    }
}
