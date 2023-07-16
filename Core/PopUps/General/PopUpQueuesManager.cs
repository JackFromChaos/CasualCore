using System.Collections.Generic;
using UnityEngine;
[AutoBind()]
public class PopUpQueuesManager : MonoBehaviour, IListener<ClosePopUpMsg>, IListener<ShowQueuePopup>
{
    public Dictionary<string, PopUpQueuesGroup> queues = new Dictionary<string, PopUpQueuesGroup>();
    public void ShowPopUp(string group,ShowPopUpMsg msg)
    {
        if (!queues.ContainsKey(group))
        {
            queues.Add(group, new PopUpQueuesGroup());
            queues[group].groupName = group;
        }

        queues[group].ShowPopup(msg);
    }

    public void Handle(ClosePopUpMsg ev)
    {
        foreach (var group in queues)
        {
            group.Value.OnClose(ev);
        }
    }

    public void Handle(ShowQueuePopup ev)
    {
        ShowPopUp(ev.groupName,ev.popup);
    }
}
[System.Serializable]
public class PopUpQueuesGroup
{
    public string groupName;
    public List<ShowPopUpMsg> queue = new List<ShowPopUpMsg>();
    public ShowPopUpMsg current;

    public void ShowPopup(ShowPopUpMsg msg)
    {
        queue.Add(msg);
        if(current==null||current.isClosed)
            ShowNextPopup();
    }

    public void OnClose(ClosePopUpMsg ev)
    {
        if (current == ev.request)
        {
            current=null;
        }

        ShowNextPopup();
    }

    private void ShowNextPopup()
    {
        if (queue.Count > 0)
        {
            current = queue[0];
            queue.RemoveAt(0);
            Transmitter.Send(current);
        }
    }
}
[System.Serializable]
public class ShowQueuePopup
{
    public ShowQueuePopup(string groupName, ShowPopUpMsg popup)
    {
        this.groupName = groupName;
        this.popup = popup;
    }

    public ShowQueuePopup(string groupName, PopUpType popUpType)
    {
        this.groupName = groupName;
        this.popup = new ShowPopUpMsg(popUpType);

    }
    public ShowQueuePopup(string groupName, PopUpType popUpType,object data)
    {
        this.groupName = groupName;
        this.popup = new ShowPopUpMsg(popUpType){popUpData = data};

    }
    public string groupName;
    public ShowPopUpMsg popup;
}