using System;
using System.Collections.Generic;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a principal in a security system model.
    /// </summary>
    public class Principal : IEquatable<Principal>
    {
        private string name;
        private HashSet<Principal> subordinates;

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
            this.subordinates = new HashSet<Principal>();
        }

        public override int GetHashCode() => name.GetHashCode();

        public override bool Equals(object obj) => obj is Principal ? Equals(obj as Principal) : false;
        public bool Equals(Principal other) => ReferenceEquals(this, other);

        /// <summary>
        /// Gets the name of the principal.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the list of principals that the principal can act for.
        /// </summary>
        public Principal[] Subordinates
        {
            get
            {
                var resultArray = new Principal[subordinates.Count];
                subordinates.CopyTo(resultArray, 0);
                return resultArray;
            }
        }

        /// <summary>
        /// Adds the <see cref="Principal"/> to the list of subordinates.
        /// </summary>
        /// <param name="principal">The subordinate to be added.</param>
        /// <returns>True if the subordinate was added, false if it is already present.</returns>
        public bool AddSubordinate(Principal principal)
        {
            return subordinates.Add(principal);
        }

        /// <summary>
        /// Recursively gets the list of actual principals that the principal can act for.
        /// </summary>
        public Principal[] ActualSubordinates
        {
            get
            {
                var resultHash = new HashSet<Principal>();
                actualSubordinatesRec(this, resultHash);

                Principal[] resultArray = new Principal[resultHash.Count];
                resultHash.CopyTo(resultArray);
                return resultArray;
            }
        }

        private void actualSubordinatesRec(Principal current, HashSet<Principal> result)
        {
            foreach (var s in current.Subordinates)
            {
                result.Add(s);
                actualSubordinatesRec(s, result);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Principal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="Principal"/>.
        /// </returns>
        public override string ToString() => name;
    }
}
