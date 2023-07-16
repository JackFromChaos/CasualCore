using UnityEngine;

public class EndSceneTimer : MonoBehaviour
{
    public float time;

    void Start()
    {
        this.DelayCall(SceneEnd,time);
    }

    void SceneEnd()
    {
        Transmitter.Send(new SceneEndMsg());
    }
}