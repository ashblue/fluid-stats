using System.Collections.Generic;
using UnityEngine.Events;

namespace CleverCrow.Fluid.StatsSystem.StatsContainers {
    public interface IStatsContainer {
        StatsContainer CreateRuntimeCopy ();

        StatRecord GetRecord (string definitionId);
        StatRecord GetRecord (StatDefinition definition);

        bool HasRecord (string definitionId);
        bool HasRecord (StatDefinition definition);

        float GetStat (StatRecord record, float index = 0);
        float GetStat (StatDefinition definition, float index = 0);
        float GetStat (string definitionId, float index = 0);

        int GetStatInt (StatRecord record, float index = 0);
        int GetStatInt (StatDefinition definition, float index = 0);
        int GetStatInt (string definitionId, float index = 0);

        void SetModifier (OperatorType operation, StatRecord record, string modifierId, float value);
        void SetModifier (OperatorType operation, StatDefinition definition, string modifierId, float value);
        void SetModifier (OperatorType operation, string definitionId, string modifierId, float value);

        float GetModifier (OperatorType operation, string definitionId, string modifierId);
        float GetModifier (OperatorType operation, StatDefinition definition, string modifierId);

        bool RemoveModifier (OperatorType operation, StatRecord record, string modifierId);
        bool RemoveModifier (OperatorType operation, StatDefinition definition, string modifierId);
        bool RemoveModifier (OperatorType operation, string definitionId, string modifierId);

        void ClearAllModifiers (StatRecord record, string modifierId);
        void ClearAllModifiers (string modifierId);
        void ClearAllModifiers (List<string> modifierIds);

        void OnStatChangeSubscribe (StatDefinition definition, UnityAction<StatRecord> callback);
        void OnStatChangeSubscribe (string definitionId, UnityAction<StatRecord> callback);
        void OnStatChangeSubscribe (StatRecord record, UnityAction<StatRecord> callback);

        void OnStatChangeUnsubscribe (StatRecord record, UnityAction<StatRecord> callback);
        void OnStatChangeUnsubscribe (StatDefinition definition, UnityAction<StatRecord> callback);
        void OnStatChangeUnsubscribe (string definitionId, UnityAction<StatRecord> callback);
    }
}
