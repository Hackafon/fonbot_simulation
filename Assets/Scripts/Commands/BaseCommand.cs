using UnityEngine;

namespace Fonbot.Commands
{
    public abstract class BaseCommand:MonoBehaviour
    {
        public abstract string Execute();
        public abstract void ProcessArguments(string cmd);
    }
}
