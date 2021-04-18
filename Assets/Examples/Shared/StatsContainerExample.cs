using CleverCrow.Fluid.StatsSystem.StatsContainers;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem.Editors {
    public class StatsContainerExample : MonoBehaviour {
        public StatsContainer original;
        public StatsContainer copy;

        void Awake () {
            copy = original.CreateRuntimeCopy();
        }
    }
}
