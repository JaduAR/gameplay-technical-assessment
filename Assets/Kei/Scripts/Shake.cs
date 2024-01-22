
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shake
    {
        public static IEnumerator Shaker(Transform _t, float _magnitude, float _duration)
            {
                float time = 0.0f;
                Vector3 newPos = Vector3.zero;

                while (time < _duration)
                    {
                        float perc = time / _duration;
                        float str = 1.0f - perc;
                        time += Time.deltaTime;
                        newPos.x = Random.Range(-1.0f, 1.0f) * _magnitude * str;
                        newPos.y = Random.Range(-1.0f, 1.0f) * _magnitude * str;
                        newPos.z = Random.Range(-1.0f, 1.0f) * _magnitude * str;
                        _t.localPosition = newPos;
                        yield return null;
                    }

                _t.localPosition = Vector3.zero;
                _t.localEulerAngles = Vector3.zero;
            }
    }
