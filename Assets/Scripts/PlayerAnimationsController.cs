using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationsController : MonoBehaviour
{
    public Transform rightHandNotSkelet;
    public Animator notSkeletPlayerAnimator;
    public GameObject notSkeletHands;
    public Image aimCircle;
    public AnimationEvent animationEvents;
    public bool isInHandItemBlocked;
    public void PlayUsingAnimation(ItemData itemData)
    {




            notSkeletPlayerAnimator.SetTrigger(itemData.animationName);
            GameObject newObject = Instantiate(itemData.prefab, rightHandNotSkelet);
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;

       
        
        //newObject.transform.localScale = Vector3.one;
    }
    public void StopAnimation()
    {  
        if(rightHandNotSkelet.childCount > 0)
            Destroy(rightHandNotSkelet.GetChild(0).gameObject);
    }
    public void SwitchAnimation()
    {
        notSkeletPlayerAnimator.SetTrigger("Hide");
    }
}
