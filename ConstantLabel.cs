using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label that is unknown, yet constant.
    /// </summary>
    public class ConstantLabel : Label, IEquatable<ConstantLabel>
    {
        private string name;

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

        protected internal override bool LessRestrictiveThan(PolicyLabel label) => false;
        protected internal override bool LessRestrictiveThan(ConstantLabel label) => this == label;

        protected internal override bool LessRestrictiveThan(JoinLabel label) => this <= label.Label1 || this <= label.Label2;

        public override bool Equals(Label label) => label is ConstantLabel ? Equals(label as ConstantLabel) : false;
        public bool Equals(ConstantLabel label) => name.Equals(label.name);

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="ConstantLabel" />
        /// </summary>
        public override Label NoVariables => this;

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
