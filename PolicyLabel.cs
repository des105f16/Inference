using System;
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
        /// Initializes a new instance of the <see cref="PolicyLabel"/> class.
        /// </summary>
        /// <param name="policies">The policies that should define the label.</param>
        public PolicyLabel(Policy policy, params Policy[] policies)
        {
            this.policies = new Policy[1 + policies?.Length ?? 0];

            this.policies[0] = policy;
            if (policies != null)
                policies.CopyTo(this.policies, 1);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyLabel"/> class.
        /// </summary>
        /// <param name="policies">The policies that should define the label.</param>
        public PolicyLabel(IEnumerable<Policy> policies)
        {
            if (policies == null)
                throw new ArgumentNullException(nameof(policies));

            this.policies = policies.ToArray();

            if (this.policies.Length == 0)
                throw new ArgumentException($"A policy label must have policies. Otherwise see {nameof(LowerBoundLabel)}.", nameof(policies));
        }

        internal override bool LessRestrictiveThan(PolicyLabel label)
        {
            foreach (var o in Owners())
            {
                if (!label.Owners().Contains(o))
                    return false;

                foreach (var r in label.Readers(o))
                    if (!Readers(o).Contains(r))
                        return false;
            }

            return true;
        }
        internal override bool LessRestrictiveThan(ConstantLabel label) => false;
        internal override bool LessRestrictiveThan(JoinLabel label) => this <= label.Label1 || this <= label.Label2;

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="PolicyLabel" />
        /// </summary>
        public override Label NoVariables => this;

        /// <summary>
        /// Gets the number of policies in the label.
        /// </summary>
        public int Count => policies.Length;
        /// <summary>
        /// Gets the <see cref="Policy"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the policy, see <see cref="Count"/>.</param>
        public Policy this[int index] => policies[index];

        public IEnumerable<Principal> Owners() => policies.Select(x => x.Owner);
        public IEnumerable<Principal> Readers(Principal owner)
        {
            if (!policies.Any(x => x.Owner == owner))
                throw new ArgumentException();
            else
            {
                var p = policies.First(x => x.Owner == owner);

                yield return p.Owner;

                for (int i = 0; i < p.ReaderCount; i++)
                    yield return p[i];
            }
        }

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
