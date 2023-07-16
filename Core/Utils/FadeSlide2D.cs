using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSlide2D : MonoBehaviour
{
    public List<Transform> list;

    public float delay;
    public float fadeTime;
    public float hideTime;
    public float slideTime;
    public float waitBeforeLast;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimationCor());
        foreach (var transform1 in list)
        {
            transform1.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimationCor()
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        //foreach (var transform1 in list)
        for(int i=0;i<list.Count; i++)
        {
            var transform1 = list[i];
            bool isLast=i==list.Count-1;
            transform1.gameObject.SetActive(true);
            transform1.UISetAlpha(0);
            transform1.UIFadeAlpha(1,fadeTime);
            
            if (isLast)
            {
                yield return new WaitForSeconds(waitBeforeLast);
                if (hideTime > 0)
                    transform1.UIFadeAlpha(0, hideTime);
            }
            else
            {
                yield return new WaitForSeconds(slideTime);
                if (hideTime > 0)
                    transform1.UIFadeAlpha(0, hideTime);
            }
            
        }
        //Transmitter.Send(new SceneEndMsg());
    }
}

public class SceneEndMsg
{

}