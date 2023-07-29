using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Sensors
{
    [CreateAssetMenu(fileName = "Ultrasonic options", menuName = "Fonbot/Ultrasonic options")]
    public class UltrasonicOptions : ScriptableObject
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _whatToHit;

        public float MaxDistance => _maxDistance;
        public LayerMask WhatToHit => _whatToHit;
    }
}