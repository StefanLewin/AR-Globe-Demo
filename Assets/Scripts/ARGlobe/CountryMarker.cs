using UnityEngine;

namespace ARGlobe
{
    public class CountryMarker : MonoBehaviour
    {
        public Country country;

        public void ShowMarker()
        {
            this.gameObject.SetActive(true);
        }

        public void HideMarker()
        {
            this.gameObject.SetActive(false);
        }
    }
}


