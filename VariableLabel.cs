using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label that can be modified when solving inference.
    /// </summary>
    public class VariableLabel : Label
    {
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableLabel"/> class.
        /// </summary>
        /// <param name="name">The name associated with the label.</param>
        public VariableLabel(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            name = name.Trim();
            if (name.Length == 0)
                throw new ArgumentException("The name of a label cannot be the empty string.", nameof(name));

            this.name = name;
        }

        /// <summary>
        /// Gets the name associated with the label.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="VariableLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="VariableLabel"/>.
        /// </returns>
        public override string ToString() => $"[{name}]";
    }
}
