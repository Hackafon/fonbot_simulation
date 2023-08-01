using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fonbot.Common
{
    public class ObjectPool : MonoBehaviour
    {
        private List<RectTransform> _pool;

        public void InitializePool(RectTransform objectToPool, int count)
        {
            _pool = new List<RectTransform>();

            for (int i = 0; i < count; i++)
            {
                var _obj = Instantiate(objectToPool, transform);
                _obj.gameObject.SetActive(false);
                _pool.Add(_obj);
            }
        }

        public RectTransform GetPooledObject()
        {
            foreach (var _obj in _pool)
            {
                if (!_obj.gameObject.activeSelf)
                {
                    _obj.gameObject.SetActive(true);
                    return _obj;
                }
            }

            return null;
        }

        public void ReturnObjectToPool(RectTransform obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}