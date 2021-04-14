using System.Collections.Generic;

namespace CleverCrow.Fluid.StatsSystem {
    [System.Serializable]
    public class StatDefinitionOverrideCollection {
        private Dictionary<StatDefinition, StatDefinitionOverride> _overridesByDefinition;
        public List<StatDefinitionOverride> overrides = new List<StatDefinitionOverride>();

        public Dictionary<StatDefinition, StatDefinitionOverride> OverridesByDefinition {
            get {
                if (_overridesByDefinition == null) {
                    Clean();
                }

                return _overridesByDefinition;
            }
        }

        public void Add (StatDefinitionOverride @override) {
            if (@override == null || Has(@override) || !@override.IsValid) {
                return;
            }

            overrides.Add(@override);
            OverridesByDefinition[@override.definition] = @override;
        }

        public StatDefinitionOverride Get (StatDefinitionOverride @override) {
            if (@override == null) return null;

            return Get(@override.definition);
        }

        public StatDefinitionOverride Get (StatDefinition def) {
            if (def == null) return null;

            StatDefinitionOverride o;
            OverridesByDefinition.TryGetValue(def, out o);

            return o;
        }

        public bool Has (StatDefinitionOverride @override) {
            if (@override == null) {
                return false;
            }

            return Has(@override.definition);
        }

        public bool Has (StatDefinition def) {
            if (def == null) {
                return false;
            }

            return OverridesByDefinition.ContainsKey(def);
        }

        public bool Remove (StatDefinition def) {
            if (def == null) return false;

            var o = Get(def);
            if (o == null) return false;

            overrides.Remove(o);
            _overridesByDefinition.Remove(def);

            return true;
        }

        /// <summary>
        /// Wipe all null definition keys
        /// </summary>
        public void Clean () {
            _overridesByDefinition = new Dictionary<StatDefinition, StatDefinitionOverride>();

            var cleanedOverrides = new List<StatDefinitionOverride>();
            foreach (var def in overrides) {
                if (!def.IsValid) continue;
                cleanedOverrides.Add(def);
                _overridesByDefinition[def.definition] = def;
            }

            overrides = cleanedOverrides;
        }
    }
}
