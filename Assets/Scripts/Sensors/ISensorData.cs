using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Sensors
{
    public interface ISensorData
    {
        void PublishToTopic();
    }
}