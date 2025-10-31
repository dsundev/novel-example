using Controls;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Engine
{
    public class Step : MonoBehaviour
    {
        [Inject] private ControlPanel _controlPanel;
        [Inject] private ChoicePanel _choicePanel;

        [SerializeField] private UnityEvent _onStart;
        
        private Episode _episode;

        public void Setup(Episode episode)
        {
            _episode = episode;

            _controlPanel.OnNextClick -= OnNextButtonClick;
            _controlPanel.OnNextClick += OnNextButtonClick;
            
            _controlPanel.OnBackClick -= OnBackButtonClick;
            _controlPanel.OnBackClick += OnBackButtonClick;
            
            _controlPanel.DisableBackMove();
            if (!_episode.IsFirstStep)
                _controlPanel.EnableBackMove();
        }
        
        public void Run()
        {
            if (_choicePanel.IsVisible)
                _choicePanel.Hide();
            
            _onStart?.Invoke();
        }

        public void Release()
        {
            _episode = null;
            
            if (_controlPanel != null)
            {
                _controlPanel.OnNextClick -= OnNextButtonClick;
                _controlPanel.OnBackClick -= OnBackButtonClick;
            }
        }

        private void OnBackButtonClick()
        {
            if (_controlPanel.IsTyping)
            {
                _controlPanel.ForceCompleteTyping();
            }
            else
            {
                _episode.BackStep();
            }
        }
        
        private void OnNextButtonClick()
        {
            if (_controlPanel.IsTyping)
            {
                _controlPanel.ForceCompleteTyping();
            }
            else
            {
                _episode.NextStep();
            }
        }
    }
}