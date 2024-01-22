using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalk : MonoBehaviour
{
    [SerializeField] Controller_StateBases cSB;
    [SerializeField] Vector2 moveDurationRange = new Vector2(1f, 2f);
    [SerializeField] Vector2 stopDurationRange = new Vector2(1f, 2f);
    [SerializeField] Vector2 randomDirectionX = new Vector2(-0.5f, 0.5f);
    [SerializeField] Vector2 randomDirectionZ = new Vector2(-0.5f, 0.5f);
    private void Start()
        {
            StartCoroutine(RandomMoveRoutine());
        }

    private IEnumerator RandomMoveRoutine()
        {
            while (true)
                {
                    Vector2 randomDirection = new Vector2(Random.Range(randomDirectionX.x, randomDirectionX.y), Random.Range(randomDirectionZ.x, randomDirectionZ.y));

                    float moveDuration = Random.Range(moveDurationRange.x, moveDurationRange.y);

                    float timer = 0;
                    while (timer < moveDuration)
                        {
                        cSB.UpdateMovement_Auto(randomDirection);
                        timer += Time.deltaTime;
                        yield return null;
                        }
                    float stopDuration = Random.Range(stopDurationRange.x, stopDurationRange.y);
                    yield return new WaitForSeconds(stopDuration);
                }
        }

    }
