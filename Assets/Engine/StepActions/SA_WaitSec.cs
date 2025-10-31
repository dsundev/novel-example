using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Engine.StepActions
{
    public class SA_WaitSec : MonoBehaviour
    {
        [SerializeField] private float _seconds;
        [SerializeField] private UnityEvent _onComplete;

        private Coroutine _waitCoroutine;
        
        public void RunAction()
        {
            if (_waitCoroutine != null)
            {
                Debug.LogError("Fail to start waiting. Coroutine already started.");
                return;
            }

            _waitCoroutine = StartCoroutine(WaitCoroutine(_seconds));
        }

        private IEnumerator WaitCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _waitCoroutine = null;
            _onComplete?.Invoke();
        }
    }
}