using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vfx2dController : MonoBehaviour, IListener<Vfx2dMsg>
{
    public TypeFxMap fxMap;
    public TypePrefabMap prefabMap;
    public TypeFxMap fxInstanceMap;
    public InstanceFxMap fxMultyInstanceMap;
    public int deafultOrderIndex = 500;
    public bool forceOrderIndex = true;
    public bool disableCache = false;

    public Transform fxContainer;
    [Serializable] public class TypeFxMap : SerializableDictionary<Vfx2dTypes, BaseVfx2d> { }
    [Serializable] public class TypePrefabMap : SerializableDictionary<Vfx2dTypes, GameObject> { }
    [Serializable] public class InstanceFxMap : SerializableDictionary<Vfx2dTypes, List<BaseVfx2d>> { }

    public void Handle(Vfx2dMsg ev)
    {
        StartCoroutine(HandlePlayFxMsg(ev));
    }

    private BaseVfx2d CreateInstance(Vfx2dTypes type)
    {
        BaseVfx2d result = null;
        if (fxMap.TryGetValue(type, out var fx))
        {
            result = Instantiate(fx, fxContainer);

            //return result;
        }
        else if (prefabMap.TryGetValue(type, out var particle))
        {
            var instance = Instantiate(particle, fxContainer);
            result = instance.GetComponent<BaseVfx2d>();
            if (result == null)
            {
                result = instance.AddComponent<ParticleVfx2d>();
            }
        }

        if (result != null)
        {
            if (forceOrderIndex)
            {
                result.SetOrder(deafultOrderIndex);
            }
        }
        return result;
    }
    public BaseVfx2d GetFxInstance(Vfx2dTypes type, bool multyInstance)
    {
        if (!fxMap.Contains(type) && !prefabMap.Contains(type))
        {
            return null;
        }

        if (disableCache)
        {
            return CreateInstance(type);
        }
        BaseVfx2d result = null;
        if (multyInstance)
        {
            if (!fxMultyInstanceMap.TryGetValue(type, out var list))
            {
                result = CreateInstance(type);
                if (result != null)
                {
                    list = new List<BaseVfx2d>();
                    list.Add(result);
                    fxMultyInstanceMap.Add(type, list);
                }
            }
            else
            {
                foreach (var fx in list)
                {
                    if (!fx.IsActive())
                    {
                        result = fx;
                        break;
                    }
                }

                if (result == null)
                {
                    result = CreateInstance(type);
                    if (result != null)
                    {
                        list.Add(result);
                    }

                }
            }

        }
        else
        {
            if (!fxInstanceMap.TryGetValue(type, out result))
            {
                result = CreateInstance(type);
                if (result != null)
                {
                    fxInstanceMap.Add(type, result);
                }
            }
        }


        return result;
    }
    public IEnumerator HandlePlayFxMsg(Vfx2dMsg ev)
    {

        var fx = GetFxInstance(ev.type, ev.mutlyInstance);
        if (fx != null && fx.gameObject != null)
        {
            fx.transform.parent = fxContainer;
            fx.transform.position = ev.position;
            fx.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
            if (fx.gameObject != null)
                fx.Play();
            yield return new WaitForSeconds(ev.time);
            if (fx.gameObject != null)
                fx.Stop();
            if (disableCache)
            {
                if (fx.gameObject != null)
                    Destroy(fx.gameObject, 1f);
            }
        }

    }
}
public enum Vfx2dTypes
{
    MagicPoof,
    TakeStar,
    HitC,
    Hint,
    FindObject
}
public class Vfx2dMsg
{
    public Vector3 position;
    public Vfx2dTypes type;
    public float time = 1f;
    public bool mutlyInstance = false;
}