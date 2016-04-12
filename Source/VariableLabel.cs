using System;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a label that can be modified when solving inference.
    /// </summary>
    public class VariableLabel : Label, IEquatable<VariableLabel>
    {
        private readonly string name;
        private Label currentUpperBound;

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
            this.currentUpperBound = UpperBoundLabel.Singleton;
        }

        /// <summary>
        /// Creates a new <see cref="VariableLabel"/> with the same name as this label and a copy of its current upper bound.
        /// </summary>
        /// <returns>A copy of this <see cref="VariableLabel"/>.</returns>
        public override Label Clone() => new VariableLabel(name) { currentUpperBound = currentUpperBound.Clone() };

        public override bool Equals(Label label) => label is VariableLabel ? Equals(label as VariableLabel) : false;
        public bool Equals(VariableLabel label) => name.Equals(label.name);

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="VariableLabel" />
        /// </summary>
        public override Label NoVariables => currentUpperBound;
        /// <summary>
        /// Returns this label; see <see cref="Label.ReplaceConstant(ConstantLabel, Label)"/>.
        /// </summary>
        /// <param name="constant">The constant label that should be replaced.</param>
        /// <param name="replacement">The replacement label.</param>
        /// <returns>The result of the label replacement.</returns>
        public override Label ReplaceConstant(ConstantLabel constant, Label replacement) => this;

        /// <summary>
        /// Gets the name associated with the label.
        /// </summary>
        public string Name => name;
        /// <summary>
        /// Gets or sets the current upper bound of this variable label.
        /// </summary>
        public Label CurrentUpperBound
        {
            get { return currentUpperBound; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                currentUpperBound = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="VariableLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="VariableLabel"/>.
        /// </returns>
        public override string ToString() => name;
    }
}
