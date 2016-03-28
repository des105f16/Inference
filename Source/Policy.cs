using System;
using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label policy composed by principals.
    /// </summary>
    public class Policy : IEquatable<Policy>
    {
        private readonly Principal owner;
        private readonly Principal[] readers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> struct.
        /// </summary>
        /// <param name="owner">The owner of the policy.</param>
        /// <param name="readers">The readers allowed by <paramref name="owner"/>.</param>
        public Policy(Principal owner, params Principal[] readers)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            this.owner = owner;
            this.readers = readers ?? new Principal[0];
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> struct.
        /// </summary>
        /// <param name="owner">The owner of the policy.</param>
        /// <param name="readers">The readers allowed by <paramref name="owner"/>.</param>
        public Policy(Principal owner, IEnumerable<Principal> readers)
            : this(owner, readers.ToArray())
        {
        }

        public override int GetHashCode() => owner.GetHashCode();
        public override bool Equals(object obj) => obj is Policy ? Equals(obj as Policy) : false;
        public bool Equals(Policy other) => owner.Equals(other.owner) && ArrayEquatable.Equals(readers, other.readers);

        /// <summary>
        /// Gets the principal owner associated with this policy.
        /// </summary>
        public Principal Owner => owner;

        /// <summary>
        /// Gets the number of readers associated with this policy/owner.
        /// </summary>
        public int ReaderCount => readers.Length;
        /// <summary>
        /// Gets the reader <see cref="Principal"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the reader, see <see cref="ReaderCount"/>.</param>
        public Principal this[int index] => readers[index];

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="Policy"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="Policy"/>.
        /// </returns>
        public override string ToString()
        {
            if (readers.Length == 0)
                return owner + ": \u2205";
            else
                return owner + ": " + readers.JoinString(", ");
        }
    }
}
