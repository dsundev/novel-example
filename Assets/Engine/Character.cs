using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private List<CharacterPositionConfiguration> _configurations;

        public void Show(CharacterPosition position, int variant)
        {
            Hide();
            
            var configuration = _configurations.FirstOrDefault(x => x.Position == position);
            if (configuration == null)
                return;

            var containerTr = configuration.VariantsContainer.transform;
            if (variant < 0 || variant >= containerTr.childCount)
                return;
            
            containerTr.GetChild(variant).gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            foreach (var configuration in _configurations)
            {
                for (var i = 0; i < configuration.VariantsContainer.transform.childCount; i++)
                {
                    configuration.VariantsContainer.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public enum CharacterPosition
    {
        Left,
        Right
    }

    [Serializable]
    public class CharacterPositionConfiguration
    {
        public CharacterPosition Position;
        public GameObject VariantsContainer;
    }
}