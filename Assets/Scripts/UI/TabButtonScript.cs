using UnityEngine;

public class TabButtonScript : MonoBehaviour
{
   [SerializeField] private TabTypeEnum tabType;

   public void OnButtonClick()
   {
      TabService.SetTabType(tabType);
   }
}
