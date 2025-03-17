using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public Action OnItemCollected;
    public Action OnGunReload;
    public Action OnLanternToggled;
    public Action OnAnimationEnded;
    private void ItemCollected()
    {
        OnItemCollected.Invoke();
    }
    private void GunReloaded()
    {
        OnGunReload.Invoke();
    }
    private void LanternToggled()
    {
        OnLanternToggled.Invoke();
    }
    private void AnimationEnded()
    {
        if(OnAnimationEnded != null)
            OnAnimationEnded.Invoke();
    }
}
