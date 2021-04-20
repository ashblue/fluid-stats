using System.Collections.Generic;

namespace CleverCrow.Fluid.StatsSystem {
    public class StatRecordCollection {
        private Dictionary<string, StatRecord> _recordsById = new Dictionary<string, StatRecord>();
        private Dictionary<StatDefinition, StatRecord> _recordsByDefinition = new Dictionary<StatDefinition, StatRecord>();
        public List<StatRecord> records = new List<StatRecord>();

        public StatRecord Get (string id) {
            if (string.IsNullOrEmpty(id)) {
                return null;
            }

            StatRecord result = null;
            _recordsById.TryGetValue(id, out result);

            return result;
        }

        public StatRecord Get (StatDefinition definition) {
            if (definition == null) return null;

            StatRecord result = null;
            _recordsByDefinition.TryGetValue(definition, out result);

            return result;
        }

        public bool Has (string id) {
            if (string.IsNullOrEmpty(id)) return false;
            return _recordsById.ContainsKey(id);
        }

        public bool Has (StatDefinition definition) {
            if (definition == null) return false;
            return _recordsByDefinition.ContainsKey(definition);
        }

        public void Set (StatRecord record) {
            if (record == null || record.Definition == null || Has(record.Definition)) {
                return;
            }

            if (!string.IsNullOrEmpty(record.Definition.Id)) {
                _recordsById[record.Definition.Id] = record;
            }

            _recordsByDefinition[record.Definition] = record;
            records.Add(record);
        }
    }
}
