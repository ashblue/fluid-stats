using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.Fluid.StatsSystem.StatsContainers {
    [CreateAssetMenu(fileName = "StatsContainer", menuName = "Fluid/Stats/Container")]
    public class StatsContainer : ScriptableObject, IStatsContainer {
        private class StatRecordEvent : UnityEvent<StatRecord> {}
        private Dictionary<StatRecord, StatRecordEvent> _events = new Dictionary<StatRecord, StatRecordEvent>();

        [Tooltip("This collection will be combined with the default stats from settings")]
        public StatDefinitionCollection collection;

        public StatRecordCollection records = new StatRecordCollection();

        public StatDefinitionOverrideCollection overrides = new StatDefinitionOverrideCollection();

        public bool IsSetup { get; private set; }

        /// <summary>
        /// Run initialization on the stats system
        /// </summary>
        public void Setup () {
            if (IsSetup) return;

            List<StatDefinition> definitions;

            if (collection == null) {
                definitions = StatsSettings.Current.DefinitionsCompiled.GetDefaults();
            } else {
                definitions = StatsSettings.Current.DefinitionsCompiled.Get(collection);
            }

            if (definitions == null) {
                Debug.Assert(!Application.isPlaying,
                    "Collection is required to initialize a StatsData component. Aborting setup.");
                return;
            }

            foreach (var statDefinition in definitions) {
                var @override = overrides.Get(statDefinition);
                var val = @override == null ? null : @override.value;

                records.Set(new StatRecord(statDefinition, val));
            }

            IsSetup = true;
        }

        /// <summary>
        /// Retrieve a record by ID
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        public StatRecord GetRecord (string definitionId) {
            return records.Get(definitionId);
        }

        public StatRecord GetRecord (StatDefinition definition) {
            return records.Get(definition);
        }

        /// <summary>
        /// Check if a record exists by ID
        /// </summary>
        /// <param name="definitionId"></param>
        /// <returns></returns>
        public bool HasRecord (string definitionId) {
            return records.Has(definitionId);
        }

        public bool HasRecord (StatDefinition definition) {
            return records.Has(definition);
        }

        /// <summary>
        /// Retrieve a float value by record with an optional index
        /// </summary>
        /// <param name="record"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetStat (StatRecord record, float index = 0) {
            if (record == null) return 0;
            return record.GetValue(index);
        }

        public float GetStat (StatDefinition definition, float index = 0) {
            return GetStat(GetRecord(definition), index);
        }

        public float GetStat (string definitionId, float index = 0) {
            return GetStat(GetRecord(definitionId), index);
        }

        /// <summary>
        /// Retrieve an int value by record with an optional index
        /// </summary>
        /// <param name="record"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetStatInt (StatRecord record, float index = 0) {
            if (record == null) return 0;
            return Mathf.RoundToInt(record.GetValue(index));
        }

        public int GetStatInt (StatDefinition definition, float index = 0) {
            return GetStatInt(GetRecord(definition), index);
        }

        public int GetStatInt (string definitionId, float index = 0) {
            return GetStatInt(GetRecord(definitionId), index);
        }

        /// <summary>
        /// Set a modifier by ID and value
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="record"></param>
        /// <param name="modifierId"></param>
        /// <param name="value"></param>
        public void SetModifier (OperatorType operation, StatRecord record, string modifierId, float value) {
            if (record == null) return;
            record.GetModifier(operation).Set(modifierId, value);
            TriggerEvent(record);
        }

        public void SetModifier (OperatorType operation, StatDefinition definition, string modifierId, float value) {
            SetModifier(operation, GetRecord(definition), modifierId, value);
        }

        public void SetModifier (OperatorType operation, string definitionId, string modifierId, float value) {
            SetModifier(operation, GetRecord(definitionId), modifierId, value);
        }

        /// <summary>
        /// Retrieve the value of a modifier
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="record"></param>
        /// <param name="modifierId"></param>
        /// <returns></returns>
        public float GetModifier (OperatorType operation, StatRecord record, string modifierId) {
            if (record == null) return 0;

            var modifier = record.GetModifier(operation).Get(modifierId);
            if (modifier == null) return 0;

            return modifier.value;
        }

        public float GetModifier (OperatorType operation, StatDefinition definition, string modifierId) {
            return GetModifier(operation, GetRecord(definition), modifierId);
        }

        public float GetModifier (OperatorType operation, string definitionId, string modifierId) {
            return GetModifier(operation, GetRecord(definitionId), modifierId);
        }

        /// <summary>
        /// Remove a modifier by ID
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="record"></param>
        /// <param name="modifierId"></param>
        /// <returns></returns>
        public bool RemoveModifier (OperatorType operation, StatRecord record, string modifierId) {
            if (record == null) return false;

            var result = record.GetModifier(operation).Remove(modifierId);
            TriggerEvent(record);

            return result;
        }

        public bool RemoveModifier (OperatorType operation, StatDefinition definition, string modifierId) {
            return RemoveModifier(operation, GetRecord(definition), modifierId);
        }

        public bool RemoveModifier (OperatorType operation, string definitionId, string modifierId) {
            return RemoveModifier(operation, GetRecord(definitionId), modifierId);
        }

        /// <summary>
        /// Wipe all modifiers (add, sub, mult, div) on a particular record
        /// </summary>
        /// <param name="record"></param>
        /// <param name="modifierId"></param>
        public void ClearAllModifiers (StatRecord record, string modifierId) {
            RemoveModifier(OperatorType.Add, record, modifierId);
            RemoveModifier(OperatorType.Subtract, record, modifierId);
            RemoveModifier(OperatorType.Multiply, record, modifierId);
            RemoveModifier(OperatorType.Divide, record, modifierId);

            TriggerEvent(record);
        }

        /// <summary>
        /// Wipe all modifiers associated with the ID on every record
        /// </summary>
        /// <param name="modifierId"></param>
        public void ClearAllModifiers (string modifierId) {
            records.records.ForEach(r => ClearAllModifiers(r, modifierId));
        }

        /// <summary>
        /// Alias for ClearAllModifiers, except it wipes an array of strings
        /// </summary>
        /// <param name="modifierIds">Modifier identifiers.</param>
        public void ClearAllModifiers (List<string> modifierIds) {
            if (modifierIds == null) return;
            modifierIds.ForEach(m => ClearAllModifiers(m));
        }

        /// <summary>
        /// Does not copy over events.
        /// </summary>
        /// <returns></returns>
        public StatsContainer CreateRuntimeCopy () {
            var copy = Instantiate(this);
            copy.Setup();

            return copy;
        }

        private void TriggerEvent (StatRecord record) {
            if (record != null && _events.TryGetValue(record, out var @event)) {
                @event.Invoke(record);
            }
        }

        public void OnStatChangeSubscribe (StatRecord record, UnityAction<StatRecord> callback) {
            if (!_events.TryGetValue(record, out var @event)) {
                @event = new StatRecordEvent();
                _events[record] = @event;
            }

            @event.AddListener(callback);
        }

        public void OnStatChangeSubscribe (StatDefinition definition, UnityAction<StatRecord> callback) {
            var record = GetRecord(definition);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeSubscribe (string definitionId, UnityAction<StatRecord> callback) {
            var record = GetRecord(definitionId);
            OnStatChangeSubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe (StatRecord record, UnityAction<StatRecord> callback) {
            _events[record].RemoveListener(callback);
        }

        public void OnStatChangeUnsubscribe (StatDefinition definition, UnityAction<StatRecord> callback) {
             var record = GetRecord(definition);
             OnStatChangeUnsubscribe(record, callback);
        }

        public void OnStatChangeUnsubscribe (string definitionId, UnityAction<StatRecord> callback) {
             var record = GetRecord(definitionId);
             OnStatChangeUnsubscribe(record, callback);
        }
    }
}
