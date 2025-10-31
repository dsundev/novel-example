using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private List<Character> _characters;

        public void Show(int index, CharacterPosition position, int variant)
        {
            if (index < 0 || index >= _characters.Count)
                return;
            
            Hide(index);
            _characters[index].Show(position, variant);
        }
        
        public void Hide(int index)
        {
            if (index < 0 || index >= _characters.Count)
                return;
            
            _characters[index].Hide();
        }

        public void HideAll()
        {
            foreach (var character in _characters)
            {
                character.Hide();
            }
        }
    }
}