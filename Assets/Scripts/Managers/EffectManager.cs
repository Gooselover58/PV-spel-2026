using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;

    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectManager>();
            }
            return instance;
        }
    }

    private Transform particleHolder;
    private Dictionary<string, ParticleSystem> particles = new Dictionary<string, ParticleSystem>();

    private void Awake()
    {
        LoadParticles();
    }

    private void LoadParticles()
    {
        particleHolder = GameObject.FindGameObjectWithTag("Particles").transform;
        foreach (Transform particle in particleHolder)
        {
            ParticleSystem parts = particle.GetComponent<ParticleSystem>();
            if (parts != null)
            {
                particles.Add(parts.name, parts);
            }
        }
    }

    public void PlayParticles(string key, Vector3 pos, int count, Color color = new Color())
    {
        if (!particles.ContainsKey(key))
        {
            return;
        }

        ParticleSystem parts = particles[key];
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;
        emitParams.position = pos;
        if (color != new Color(0, 0, 0, 0))
        {
            emitParams.startColor = color;
        }

        parts.Emit(emitParams, count);
    }
}
