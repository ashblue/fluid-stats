using UnityEngine;

namespace Adnc.StatsSystem {
    [CreateAssetMenu(fileName = "StatsSettings", menuName = "ADNC/Stats/Settings", order = 1)]
    public class StatsSettings : ScriptableObject {
        private static StatsSettings _current;
        private const string RESOURCE_PATH = "StatsSettings";

        private StatDefinitionCollection _emptyStats;
        private OrderOfOperations _emptyOrderOfOperations;
        private StatDefinitionsCompiled _definitionsCompiled;

        [Tooltip("Default stats that will be included in every StatsData reference (will be overridden if re-added)")]
        [SerializeField]
        private StatDefinitionCollection _defaultStats;

        [Tooltip("Override the default order of operations being -> Add, subtract, multiply, divide")]
        [SerializeField]
        private OrderOfOperations _orderOfOperations;

        public static StatsSettings Current {
            get {
                if (_current == null) {
                    _current = Resources.Load<StatsSettings>(RESOURCE_PATH);
                    Debug.AssertFormat(
                        _current != null,
                        "Could not load {1}. Please verify a {1} object is at `Resources/{0}'. If not please create one.",
                        RESOURCE_PATH,
                        typeof(StatsSettings).FullName);
                }

                return _current;
            }

            set => _current = value;
        }

        /// <summary>
        /// Runtime only method. Not safe to use this in the editor due to caching
        /// </summary>
        public StatDefinitionsCompiled DefinitionsCompiled {
            get {
                if (_definitionsCompiled == null) {
                    _definitionsCompiled = new StatDefinitionsCompiled();
                }

                return _definitionsCompiled;
            }
        }

        public StatDefinitionCollection DefaultStats {
            get {
                if (_defaultStats == null) {
                    if (_emptyStats == null) _emptyStats = CreateInstance<StatDefinitionCollection>();
                    return _emptyStats;
                }

                return _defaultStats;
            }
        }

        public OrderOfOperations OrderOfOperations {
            get {
                if (_orderOfOperations == null) {
                    if (_emptyOrderOfOperations == null) _emptyOrderOfOperations = CreateInstance<OrderOfOperations>();
                    return _emptyOrderOfOperations;
                }

                return _orderOfOperations;
            }

            set => _orderOfOperations = value;
        }
    }
}
