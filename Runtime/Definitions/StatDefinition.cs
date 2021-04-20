using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.StatsSystem {
	[CreateAssetMenu(fileName = "StatDefinition", menuName = "Fluid/Stats/Definitions/Default")]
	public class StatDefinition : StatDefinitionBase {
        private const int MIN_SORT_INDEX = 0;
        private const int MAX_SORT_INDEX = 1000;

		[Tooltip("User friendly lookup ID")]
		[SerializeField]
        private string _id;

		public string Id => _id;

        [Header("Value")]

		[SerializeField]
		private StatValueSelector _value = new StatValueSelector();

		public StatValueSelector Value => _value;

        [Tooltip("Each time a modifier is set, it will be rounded to the nearest whole number. Note that" +
                 " the corresponding operator in Settings must have rounding enabled in order for this to work." +
                 " Does not round the total result")]
		[SerializeField]
		private bool _roundModifiers = true;

		public bool RoundModifiers => _roundModifiers;

        [Tooltip("Calculated total with modifiers will always be a rounded number")]
		[SerializeField]
        private bool _roundResult;

		public bool RoundResult => _roundResult;

        [Header("Display")]

		[Tooltip("User friendly display name")]
		[SerializeField]
		string _displayName = "Untitled";

		public string DisplayName => _displayName;

        [Tooltip("Hidden stats will be ignored for display in interfaces")]
		[SerializeField]
		bool _hidden;

		public bool Hidden => _hidden;

        [Tooltip("If a float value, should it be displayed as a percentile?")]
		[SerializeField]
		bool _percentile;

		[Tooltip("Priority when displayed in the inspector and menus. Higher number equals more likely to be above the fold")]
		[SerializeField]
		[Range(MIN_SORT_INDEX, MAX_SORT_INDEX)]
		int _sortIndex = 500;

		public int SortIndex => _sortIndex;

        [Tooltip("Descriptive text for this stat")]
		[TextArea]
		[SerializeField]
		string _description;

		public string Description => _description;

        public bool IsPercentile => _value.IsFloat && _percentile;

        /// <summary>
		/// Used by collections to recursively search through definitions quickly
		/// </summary>
		/// <param name="visited"></param>
		/// <returns></returns>
		public override List<StatDefinition> GetDefinitions (HashSet<StatDefinitionBase> visited) {
			if (visited == null || visited.Contains(this)) {
				if (Application.isPlaying) {
					Debug.LogWarningFormat("Duplicate StatDefinition detected {0}", name);
				}

				return new List<StatDefinition>();
			}

			visited.Add(this);

			return new List<StatDefinition> { this };
		}
	}
}
