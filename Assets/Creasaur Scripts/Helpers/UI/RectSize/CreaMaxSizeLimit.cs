using UnityEngine;

namespace Helpers.UI.RectSize
{
    public class CreaMaxSizeLimit : MonoBehaviour
    {
        public Vector2 MaxSizeDelta;

        private void OnEnable()
        {
            if(!CreaUtils.IsTablet())
                return;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = MaxSizeDelta;
        }   
    }
}