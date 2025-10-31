using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backgrounds;

        public void Init()
        {
            foreach (var background in _backgrounds)
            {
                background.SetActive(false);
            }
        }
        
        public void Set(int index)
        {
            if (index < 0 || index >= _backgrounds.Count)
                return;

            for (var i = 0; i < _backgrounds.Count; i++)
            {
                var background = _backgrounds[i];
                
                if (background.activeSelf)
                    background.SetActive(false);

                if (i == index && !background.activeSelf)
                    background.SetActive(true);
            }
        }
    }
}