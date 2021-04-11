using System.Collections.Generic;
using UnityEngine;

namespace Adnc.StatsSystem {
    public abstract class StatDefinitionBase : ScriptableObject {
        public List<StatDefinition> GetDefinitions () {
            return GetDefinitions(new HashSet<StatDefinitionBase>());
        }
        
        public abstract List<StatDefinition> GetDefinitions (HashSet<StatDefinitionBase> visited);
    }
}