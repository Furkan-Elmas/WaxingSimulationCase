using UnityEngine;
using System.Collections;
using WaxingSimulation.Events;
using UnityEngine.SceneManagement;

namespace WaxingSimulation.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject _startScreen;
        [SerializeField] GameObject _finishScreen;


        void OnEnable()
        {
            EventManager.OnComplete += ShowFinishScreen;
        }

        void OnDisable()
        {
            EventManager.OnComplete -= ShowFinishScreen;
        }

        void ShowFinishScreen()
        {
            StartCoroutine(FinishScreenCor());
        }

        IEnumerator FinishScreenCor()
        {
            yield return new WaitForSeconds(1f);
            _finishScreen.SetActive(true);
        }

        public void GoNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}