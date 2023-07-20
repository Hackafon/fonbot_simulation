using ROS2;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UltrasonicData : MonoBehaviour
{
    private List<RaycastHit> _hitInfo;
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _whatToHit;
    [SerializeField] private List<Transform> _rayOrigins;

    private ROS2UnityComponent _ros2Unity;
    private ROS2Node _ros2Node;
    private IPublisher<std_msgs.msg.Float32> _driverPub;

    private void Start()
    {
        _hitInfo = new List<RaycastHit>();
        for (int i = 0; i < _rayOrigins.Count; i++)
        {
            _hitInfo.Add(new RaycastHit());
        }

        _ros2Unity = GameObject.FindGameObjectWithTag("ROS2Manager").GetComponent<ROS2UnityComponent>();
    }

    void CastRays()
    {
        int index = 0;
        foreach (Transform origin in _rayOrigins)
        {
            RaycastHit hit;
            bool _hitDetect = Physics.Raycast(origin.position, origin.forward, out hit, _maxDistance, _whatToHit);
            _hitInfo[index] = hit;

            if (_hitDetect)
            {
                //Debug.Log("Hit: " + _hitInfo[index].collider.name);
            }

            index++;

            if (index >= _rayOrigins.Count)
            {
                index = 0;
            }
        }
    }

    void PublishToTopic()
    {
        if (!_ros2Unity.Ok())
        {
            return;
        }

        if (_ros2Node == null)
        {
            _ros2Node = _ros2Unity.CreateNode("ROS2UnityTalkerNode");
            _driverPub = _ros2Node.CreatePublisher<std_msgs.msg.Float32>("ultrasonic_driver");
        }

        float minDist = _maxDistance;
        foreach (RaycastHit hit in _hitInfo)
        {
            if (hit.collider != null && hit.distance < minDist)
            {
                minDist = hit.distance;
            }
        }

        std_msgs.msg.Float32 distance = new std_msgs.msg.Float32();
        distance.Data = minDist;
        _driverPub.Publish(distance);
    }

    private void Update()
    {
        CastRays();
        PublishToTopic();
    }

    void OnDrawGizmos()
    {
        int index = 0;
        foreach (Transform origin in _rayOrigins)
        {
            Gizmos.color = Color.red;
            if (origin != null)
            {
                Gizmos.DrawRay(origin.position, origin.forward * _maxDistance);
            }

            Gizmos.color = Color.green;

            if (_hitInfo != null && _hitInfo[index].collider != null)
            {
                Gizmos.DrawRay(origin.position, origin.forward * _hitInfo[index].distance);
            }

            index++;

            if (index >= _rayOrigins.Count)
            {
                index = 0;
            }
        }
    }


}
