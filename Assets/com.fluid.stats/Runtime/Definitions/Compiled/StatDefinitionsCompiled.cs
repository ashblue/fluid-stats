using System.Collections.Generic;

namespace CleverCrow.Fluid.StatsSystem {
    /// <summary>
    /// A collection for caching stat defintion collections at runtime
    /// </summary>
    public class StatDefinitionsCompiled {
        private Dictionary<StatDefinitionCollection, List<StatDefinition>> _compiled = new Dictionary<StatDefinitionCollection, List<StatDefinition>>();

        public List<StatDefinition> Get (StatDefinitionCollection id) {
            if (id == null) return null;

            List<StatDefinition> definitions;
            if (_compiled.TryGetValue(id, out definitions)) {
                return definitions;
            }

            definitions = id.GetDefinitions();

            // Append default definitions that haven't already been added
            var defaults = GetDefaults();
            if (defaults != null) {
                foreach (var statDefinition in defaults) {
                    if (definitions.Contains(statDefinition)) continue;
                    definitions.Add(statDefinition);
                }
            }

            _compiled[id] = definitions;

            return definitions;
        }

        public List<StatDefinition> GetDefaults () {
            var id = StatsSettings.Current.DefaultStats;
            if (id == null) return null;

            List<StatDefinition> definitions;
            if (_compiled.TryGetValue(id, out definitions)) {
                return definitions;
            }

            definitions = id.GetDefinitions();

            _compiled[id] = definitions;

            return definitions;
        }

        public static List<StatDefinition> GetDefinitions (StatDefinitionCollection id) {
            if (id == null) return null;
            var definitions = id.GetDefinitions();

            var defaults = GetDefinitionDefaults();
            if (defaults == null) return definitions;

            foreach (var statDefinition in defaults) {
                if (definitions.Contains(statDefinition)) continue;
                definitions.Add(statDefinition);
            }

            return definitions;
        }

        public static List<StatDefinition> GetDefinitionDefaults () {
            var id = StatsSettings.Current.DefaultStats;
            if (id == null) return null;

            return id.GetDefinitions();
        }
    }
}
