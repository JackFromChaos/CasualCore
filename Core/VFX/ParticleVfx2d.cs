using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleVfx2d : BaseVfx2d
{
    public ParticleSystem system;
    public bool withChildren=true;
    void Start()
    {
        if (system == null )
        {
            system = GetComponent<ParticleSystem>();
        }
        system.Stop(withChildren);
    }
    public override void Play()
    {

        system.Play(withChildren);
    }
    public override void Stop()
    {
        system.Stop(withChildren);
    }

}