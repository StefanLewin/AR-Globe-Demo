using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ARGlobe
{
    public class CardArea : MonoBehaviour
    {
        [SerializeField] private Image m_cardImage;
        [SerializeField] private InputHandling m_inputHandling;
        [SerializeField] private CubeController m_cubeController;
        [SerializeField]private bool _inputFinished;
        [SerializeField]private bool _checkInProgress;
        [SerializeField]private bool _checkFinished;

        private void OnEnable()
        {
            m_inputHandling.OnInputFinished += RegisterInputFinish;
            m_inputHandling.OnInputStarted += RegisterInputStart;
        }

        private void OnDisable()
        {
            m_inputHandling.OnInputFinished -= RegisterInputFinish;
            m_inputHandling.OnInputStarted -= RegisterInputStart;
        }

        private void RegisterInputFinish(object sender, EventArgs e)
        {
            _inputFinished = true;
        }
    
        private void RegisterInputStart(object sender, EventArgs e)
        {
            _inputFinished = false;
            _checkFinished = false;
            _checkInProgress = false;
        }

        private void Update()
        {
            if (m_cubeController.GetNumberOfEnteredCubes() == 3)
            {
                StartCoroutine(InitCubeCheck());
            } else if (m_cubeController.GetNumberOfEnteredCubes() == 0)
            {
                m_cardImage.fillAmount = 0;
                m_cardImage.color = Color.white;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            m_cubeController.CompareCubes(other.gameObject, true);
        }
    
        private void OnTriggerExit(Collider other)
        {
            m_cubeController.CompareCubes(other.gameObject, false);
        }

        private IEnumerator InitCubeCheck()
        {
            if (!_inputFinished || _checkFinished || _checkInProgress)
            {
                yield return null;
            }
            else
            {
                yield return FillImage();

                yield return null;
            }
        }

        private IEnumerator FillImage()
        {
            if (_checkInProgress) yield return null;
            _checkInProgress = true;
        
            m_cardImage.fillAmount = 0;
            m_cardImage.color = Color.white;
        
            yield return new WaitForSeconds(0.5f);
            while (m_cardImage.fillAmount < 1)
            {
                if (!_inputFinished)
                {
                    m_cardImage.fillAmount = 0;
                    yield return null;
                }
            
                m_cardImage.fillAmount = Mathf.MoveTowards(m_cardImage.fillAmount, 1, 0.01f);
                yield return new WaitForSeconds(0.02f);
            }
        
            if (m_cubeController.ValidateColors())
            {
                m_cardImage.fillAmount = 1;
                m_cardImage.color = Color.green;
            }
            else
            {
                m_cardImage.fillAmount = 1;
                m_cardImage.color = Color.red;
            }
            yield return null;
        }

        public void CancelCheck()
        {
            _inputFinished = false;
            _checkFinished = false;
            _checkInProgress = false;
            StopAllCoroutines();
        }
    }
}
