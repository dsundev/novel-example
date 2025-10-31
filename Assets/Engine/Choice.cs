using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using UnityEngine;
using Zenject;

namespace Engine
{
    public class Choice : MonoBehaviour
    {
        [Inject] private ChoicePanel _choicePanel;
        [Inject] private EpisodeManager _episodeManager;

        [SerializeField] private List<ChoiceConfiguration> _choiceConfigurations;

        public void Run()
        {
            _choicePanel.OnSelect -= OnChoiceSelect;
            _choicePanel.OnSelect += OnChoiceSelect;
            
            _choicePanel.Setup(_choiceConfigurations.Select(x => x.Text).ToList());
            _choicePanel.Show();
        }

        private void OnChoiceSelect(int index)
        {
            _choicePanel.OnSelect -= OnChoiceSelect;
            _episodeManager.TryRun(_choiceConfigurations[index].EpisodeId);
        }
    }

    [Serializable]
    public class ChoiceConfiguration
    {
        public string Text;
        public int EpisodeId;
    }
}