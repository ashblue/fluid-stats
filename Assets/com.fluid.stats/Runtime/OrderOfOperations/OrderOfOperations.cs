using System.Collections.Generic;
using UnityEngine;

namespace Adnc.StatsSystem {
    [CreateAssetMenu(fileName = "OrderOfOperations", menuName = "ADNC/Stats/Order Of Operations", order = 1)]
    public class OrderOfOperations : ScriptableObject {
        [Tooltip("The order of operations")]
        [SerializeField]
        private List<Operator> _operators = new List<Operator> {
            new Operator(OperatorType.Add, true),
            new Operator(OperatorType.Subtract, true),
            new Operator(OperatorType.Multiply, false),
            new Operator(OperatorType.Divide, false)
        };

        public List<Operator> Operators => _operators;
    }
}
