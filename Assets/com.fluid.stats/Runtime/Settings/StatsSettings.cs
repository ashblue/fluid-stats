using UnityEngine;

namespace Adnc.StatsSystem {
    [CreateAssetMenu(fileName = "StatsSettings", menuName = "Fluid/Stats/Settings/Default")]
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
                // Force clear to prevent memory issues
                if (Application.isEditor && !Application.isPlaying) _current = null;
                if (_current != null) return _current;

                _current = Resources.Load<StatsSettings>(RESOURCE_PATH);
                if (_current != null) return _current;

                Debug.LogWarning(
                    "No StatsSettings file discovered. Loading Defaults." +
                    " \nPlease create one via \"Project Window\" -> Right Click -> Create -> Fluid -> Stats -> Settings -> Default." +
                    " Place the file in a \"Resources\" folder so it can be properly loaded."
                );

                _current = CreateInstance<StatsSettings>();

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

        private void OnDestroy () {
            _current = null;
        }
    }
}
