using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label constraint on the form &lt;left&gt; ⊑ &lt;right&gt;.
    /// </summary>
    public class Constraint
    {
        private readonly Label left;
        private readonly Label right;

        /// <summary>
        /// Initializes a new instance of the <see cref="Constraint"/> class.
        /// </summary>
        /// <param name="left">The left label in the &lt;left&gt; ⊑ &lt;right&gt; equation.</param>
        /// <param name="right">The right label in the &lt;left&gt; ⊑ &lt;right&gt; equation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public Constraint(Label left, Label right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            this.left = left;
            this.right = right;
        }
        
        /// <summary>
        /// Gets the "least" restrictive label.
        /// </summary>
        public Label Left => left;
        /// <summary>
        /// Gets the "most" restrictive label.
        /// </summary>
        public Label Right => right;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Constraint"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="Constraint"/>.
        /// </returns>
        public override string ToString() => $"{left} \u2291 {right}";
    }
}
