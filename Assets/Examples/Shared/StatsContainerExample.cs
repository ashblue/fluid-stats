using AdncStats.Scripts.StatsContainer;
using UnityEngine;

namespace Adnc.StatsSystem.Editors {
    public class StatsContainerExample : MonoBehaviour {
        public StatsContainer original;
        public StatsContainer copy;

        void Awake () {
            copy = original.CreateRuntimeCopy();
        }
    }
}
