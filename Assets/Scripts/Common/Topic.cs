using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Common
{
    [CreateAssetMenu(fileName = "Topic", menuName = "Fonbot/Topic")]
    public class Topic : ScriptableObject
    {
        public string topicName;
    }
}