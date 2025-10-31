using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine
{
    public class EpisodeManager : MonoBehaviour
    {
        [SerializeField] private List<Episode> _episodes;

        public bool TryRun(int id, int stepIndex = 0)
        {
            var episode = _episodes.FirstOrDefault(x => x.Id == id);
            if (episode == null)
                return false;
            
            episode.Run(stepIndex);
            return true;
        }
    }
}