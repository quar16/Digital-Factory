using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleParticle : MonoBehaviour
{
    public Transform horizon;
    public ParticleSystem water;
    ParticleSystem.EmissionModule asd;

    // Start is called before the first frame update
    void Start()
    {
        asd = water.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (horizon.position.y > water.transform.position.y && !done)
            asd.enabled = true;
        else
            asd.enabled = false;
    }

    public bool done = false;

    public Transform liquid;
    public void Fill()
    {
        liquid.transform.localScale += new Vector3(0, 0.001f, 0);
        if (liquid.transform.localScale.y > 0.9f)
        {
            asd.enabled = false;
            done = true;
        }
    }
}
