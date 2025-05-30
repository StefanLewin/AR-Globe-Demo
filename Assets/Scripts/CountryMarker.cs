using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryMarker : MonoBehaviour
{
    public Country country;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMarker()
    {
        this.gameObject.SetActive(true);
    }

    public void HideMarker()
    {
        this.gameObject.SetActive(false);
    }
}

public enum Country{
    ESTONIA,
    LITHUANIA,
    ITALY,
    BELGIUM,
    GUINEA,
    ARMENIA,
    AUSTRIA,
    BULGARIA,
    GERMANY,
    HUNGARY,
    FRANCE
}
