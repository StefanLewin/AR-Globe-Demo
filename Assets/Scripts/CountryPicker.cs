using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryPicker : MonoBehaviour
{
    [SerializeField] private Countries countries;
    //[SerializeField] private CubeGenerator generator;
    public float duration = 2f; // Total time for the rotation
    
    // Start is called before the first frame update
    void Start()
    {
        StartSpin();
    }
    
    public void StartSpin()
    {
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        float elapsedTime = 0f;
        float startRotation = transform.localEulerAngles.z;
        float targetRotation = startRotation + 360f;
        
        foreach (Transform child in this.transform)
        {
            child.GetComponent<CountryMarker>().HideMarker();
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Ease-out interpolation
            float easedT = 1f - Mathf.Pow(1f - t, 2f);

            float currentRotation = Mathf.Lerp(startRotation, targetRotation, easedT);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentRotation);

            yield return null;
        }

        // Ensure exact final rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetRotation);



        int markerNumber = Random.Range(0, transform.childCount - 1);
        
        this.transform.GetChild(markerNumber).GetComponent<CountryMarker>().ShowMarker();
        
    }
}
