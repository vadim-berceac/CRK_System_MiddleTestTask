using UnityEngine;
using Zenject;

public class TabButtonScript : MonoBehaviour
{
   [SerializeField] private SoundSettings audioClip;
   [SerializeField] private TabTypeEnum tabType;
   
   [Inject] private AudioSource _audioSource;

   public void OnButtonClick()
   {
      TabService.SetTabType(tabType);
      
      if(audioClip == null || audioClip.Clip == null) return;
      audioClip.Play(_audioSource);
   }
}
