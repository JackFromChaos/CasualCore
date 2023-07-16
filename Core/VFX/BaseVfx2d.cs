using UnityEngine;


public abstract class BaseVfx2d : MonoBehaviour
{
    public abstract void Play();
    public abstract void Stop();

    public virtual bool IsActive()
    {
        if (gameObject.activeInHierarchy)
        {
            var list = GetComponentsInChildren<ParticleSystem>();
            foreach (var system in list)
            {
                if (system.IsAlive(true))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public virtual void SetOrder(int deafultOrderIndex)
    {
        var list = GetComponentsInChildren<ParticleSystem>();
        foreach (var system in list)
        {
            var vfxRenderer = system.GetComponent<Renderer>();
            if (vfxRenderer)
            {
                vfxRenderer.sortingOrder = deafultOrderIndex;
                deafultOrderIndex++;
                //vfxRenderer.sortingLayerName = layer;
            }
        }
    }
}