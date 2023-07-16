using UnityEngine;

public class GroupVfx2d : BaseVfx2d
{
    public ParticleSystem[] systems;

    void Start()
    {
        if (systems == null || systems.Length == 0)
        {
            systems = GetComponentsInChildren<ParticleSystem>();
        }

        foreach (var system in systems)
        {
            //if (system.isPlaying)
            {
                system.Stop();
            }
        }
    }

    public override void Play()
    {
        foreach (var system in systems)
        {
            system.Play();
        }
    }

    public override void Stop()
    {
        foreach (var system in systems)
        {
            system.Stop();
        }
    }

    /*public void SetScale(float scale)
    {
        foreach (var system in systems)
        {
            system.GetComponent<UIParticle>().scale = scale;
        }
    }*/
}