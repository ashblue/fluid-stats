using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
    [CreateAssetMenu(fileName = "StatDefinitionCollection", menuName = "Fluid/Stats/Definitions/Collection")]
    public class StatDefinitionCollection : StatDefinitionBase {
        [Tooltip("List of definitions and definition collections")]
        public List<StatDefinitionBase> definitions = new List<StatDefinitionBase>();

        public override List<StatDefinition> GetDefinitions (HashSet<StatDefinitionBase> visited) {
            if (visited == null || visited.Contains(this)) {
                if (Application.isPlaying) {
                    Debug.LogWarningFormat("Duplicate StatDefinitionCollection detected {0}", name);
                }

                return new List<StatDefinition>();
            }

            visited.Add(this);

            return definitions.SelectMany(d => {
                if (d == null) return new List<StatDefinition>();
                return d.GetDefinitions(visited);
            }).ToList();
        }

        // @TODO Helper method for getting all compiled StatsRecord objects from compiled definitions
    }
}
