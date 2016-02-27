using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label as the composition of a set of policies.
    /// </summary>
    public class PolicyLabel : Label
    {
        private Policy[] policies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyLabel"/> class with no policies.
        /// This corresponds to the least restrictive label.
        /// </summary>
        public PolicyLabel()
        {
            policies = new Policy[0];
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyLabel"/> class.
        /// </summary>
        /// <param name="policies">The policies that should define the label.
        /// If <paramref name="policies"/> contains no values, the label will correspond to the least restrictive label.</param>
        public PolicyLabel(params Policy[] policies)
        {
            this.policies = policies ?? new Policy[0];
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyLabel"/> class.
        /// </summary>
        /// <param name="policies">The policies that should define the label.
        /// If <paramref name="policies"/> contains no values, the label will correspond to the least restrictive label.</param>
        public PolicyLabel(IEnumerable<Policy> policies)
            : this(policies.ToArray())
        {
        }

        /// <summary>
        /// Gets the number of policies in the label.
        /// </summary>
        public int Count => policies.Length;
        /// <summary>
        /// Gets the <see cref="Policy"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the policy, see <see cref="Count"/>.</param>
        public Policy this[int index] => policies[index];

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="PolicyLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="PolicyLabel"/>.
        /// </returns>
        public override string ToString()
        {
            if (policies.Length == 0)
                return "\u22a5";
            else
                return "{" + policies.JoinString("; ") + "}";
        }
    }
}
