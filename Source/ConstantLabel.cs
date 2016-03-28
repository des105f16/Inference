using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label that is unknown, yet constant.
    /// </summary>
    public class ConstantLabel : Label, IEquatable<ConstantLabel>
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantLabel"/> class.
        /// </summary>
        /// <param name="name">The name associated with the label.</param>
        public ConstantLabel(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            name = name.Trim();
            if (name.Length == 0)
                throw new ArgumentException("The name of a label cannot be the empty string.", nameof(name));

            this.name = name;
        }

        public override bool Equals(Label label) => label is ConstantLabel ? Equals(label as ConstantLabel) : false;
        public bool Equals(ConstantLabel label) => name.Equals(label.name);

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="ConstantLabel" />
        /// </summary>
        public override Label NoVariables => this;
        /// <summary>
        /// Replaces this <see cref="ConstantLabel"/> with another label if it matches the <paramref name="constant"/> argument.
        /// </summary>
        /// <param name="constant">The constant label that should be replaced.</param>
        /// <param name="replacement">The replacement label.</param>
        /// <returns>The result of the label replacement.</returns>
        public override Label ReplaceConstant(ConstantLabel constant, Label replacement) => constant == this ? replacement : this;

        /// <summary>
        /// Gets the name associated with the label.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="ConstantLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="ConstantLabel"/>.
        /// </returns>
        public override string ToString() => $"[{name}]";
    }
}
