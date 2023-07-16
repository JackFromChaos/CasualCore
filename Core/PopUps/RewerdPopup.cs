using TMPro;

public class RewerdPopup : PopUpAnimatedController
{
    public TextMeshProUGUI rewardText;
    protected override void Init(ShowPopUpMsg request)
    {
        base.Init(request);
        RewardPopupData data=request.popUpData as RewardPopupData;
        if (data != null)
        {
            int count = data.count;
            if (count > 1)
            {
                rewardText.text = $"+ {count} hints";
            }
            else
            {
                rewardText.text = $"+ {count} hint";
            }
            
        }
        
    }
}
public class RewardPopupData
{
    public int count;
}
