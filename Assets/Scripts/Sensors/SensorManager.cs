using System;
using System.Collections;
using System.Collections.Generic;
using ROS2;
using UnityEngine;

namespace Fonbot.Sensors
{
    public class SensorManager : MonoBehaviour
    {
        public static SensorManager Instance { get; private set; }
        public ROS2Node ros2Node;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            var _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
            ros2Node = _ros2Unity.CreateNode("UnitySensors");
        }
    }
}