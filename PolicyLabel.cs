using System;
using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label as the composition of a set of policies.
    /// </summary>
    public class PolicyLabel : Label, IEquatable<PolicyLabel>
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

        public override bool Equals(Label label) => label is PolicyLabel ? Equals(label as PolicyLabel) : false;
        public bool Equals(PolicyLabel label) => ArrayEquatable.Equals(policies, label.policies);

        protected internal override bool LessRestrictiveThan(PolicyLabel label)
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
        protected internal override bool LessRestrictiveThan(ConstantLabel label) => false;
        protected internal override bool LessRestrictiveThan(JoinLabel label) => this <= label.Label1 || this <= label.Label2;

        /// <summary>
        /// Generates the join of the two labels.
        /// </summary>
        /// <param name="l1">The first label in the join.</param>
        /// <param name="l2">The second label in the join.</param>
        /// <returns>
        /// A new label that is the join of <paramref name="l1"/> and <paramref name="l2"/>.
        /// </returns>
        public static PolicyLabel operator +(PolicyLabel l1, PolicyLabel l2)
        {
            List<Policy> policies = new List<Policy>();

            var owners = l1.Owners().Union(l2.Owners()).ToArray();

            foreach (var o in owners)
            {
                var p1 = l1.policies.Where(x => x.Owner == o).ToArray();
                var p2 = l2.policies.Where(x => x.Owner == o).ToArray();

                if (p1.Length == 0 && p2.Length == 0)
                    policies.Add(new Policy(o));
                else if (p1.Length == 0)
                    policies.Add(new Policy(o, getReaders(p2)));
                else if (p2.Length == 0)
                    policies.Add(new Policy(o, getReaders(p1)));
                else
                    policies.Add(new Policy(o, getReaders(p1).Intersect(getReaders(p2))));
            }

            return new PolicyLabel(policies);
        }
        /// <summary>
        /// Generates the meet of the two labels.
        /// </summary>
        /// <param name="l1">The first label in the meet operation.</param>
        /// <param name="l2">The second label in the meet operation.</param>
        /// <returns>
        /// A new label that is the meet of <paramref name="l1"/> and <paramref name="l2"/>.
        /// </returns>
        public static PolicyLabel operator -(PolicyLabel l1, PolicyLabel l2)
        {
            List<Policy> policies = new List<Policy>();

            var owners = l1.Owners().Intersect(l2.Owners()).ToArray();

            foreach (var o in owners)
            {
                var p1 = l1.policies.Where(x => x.Owner == o).ToArray();
                var p2 = l2.policies.Where(x => x.Owner == o).ToArray();

                policies.Add(new Policy(o, getReaders(p1).Union(getReaders(p2))));
            }

            return new PolicyLabel(policies);
        }

        private static IEnumerable<Principal> getReaders(IEnumerable<Policy> policies)
        {
            foreach (var p in policies)
                for (int i = 0; i < p.ReaderCount; i++)
                    yield return p[i];
        }

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
