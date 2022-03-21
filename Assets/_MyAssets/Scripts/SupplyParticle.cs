using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SupplyParticle : MonoBehaviour
{
    public UnityEvent Event;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    ParticleSystem ps;

    List<ParticleSystem.Particle> list = new List<ParticleSystem.Particle>();
    int numEnter = 0;
    private void OnParticleTrigger()
    {
        numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, list);
        if (numEnter != 0)
            Event.Invoke();
    }

}
