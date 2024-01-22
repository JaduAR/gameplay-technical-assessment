using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ParticleManager : Singleton<ParticleManager> {
    private Dictionary<string, Queue<ParticleSystem>> particlePool = new Dictionary<string, Queue<ParticleSystem>>();

    public ParticleSystem SpawnParticle(GameObject particle, Transform t)  {
        ParticleSystem particleInstance;
        string particleName = particle.name;

        if (!particlePool.ContainsKey(particle.name)) {
            particlePool[particleName] = new Queue<ParticleSystem>();
        }

        if (particlePool[particle.name].Count > 0) {
            particleInstance = particlePool[particle.name].Dequeue();
            particleInstance.transform.position = t.position;
            particleInstance.transform.rotation = t.rotation;
            particleInstance.Play();
        }
        else {
            particleInstance = Instantiate(particle, t.position, t.rotation).GetComponent<ParticleSystem>();
            particleInstance.name = particleInstance.name.Replace("(Clone)", "");
        }

        StartCoroutine(ParticleLifespan(particleInstance));

        return particleInstance;
    }

    private IEnumerator ParticleLifespan(ParticleSystem particle) {
        while (true) {
            yield return new WaitForSeconds(0.2f);

            if (!particle.IsAlive(true)) {
                particlePool[particle.name].Enqueue(particle);
                break;
            }
        }
    }
}
