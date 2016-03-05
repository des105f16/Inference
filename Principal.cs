using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a principal in a security system model.
    /// </summary>
    public class Principal : IEquatable<Principal>
    {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Principal"/> class.
        /// </summary>
        /// <param name="name">The name of the principal.</param>
        public Principal(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            name = name.Trim();
            if (name.Length == 0)
                throw new ArgumentException("The name of a principal cannot be the empty string.", nameof(name));

            this.name = name;
        }

        public override int GetHashCode() => name.GetHashCode();

        public override bool Equals(object obj) => obj is Principal ? Equals(obj as Principal) : false;
        public bool Equals(Principal other) => ReferenceEquals(this, other);

        /// <summary>
        /// Gets the name of the principal.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Principal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="Principal"/>.
        /// </returns>
        public override string ToString() => name;
    }
}
