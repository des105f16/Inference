using System;
using System.Collections.Generic;

namespace DLM.Inference
{
    /// <summary>
    /// Represents the meet of two other labels.
    /// </summary>
    public class MeetLabel : Label, IEquatable<MeetLabel>
    {
        private readonly Label l1, l2;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetLabel"/> class.
        /// </summary>
        /// <param name="l1">The first label in the meet.</param>
        /// <param name="l2">The second label in the meet.</param>
        public MeetLabel(Label l1, Label l2)
        {
            this.l1 = l1;
            this.l2 = l2;
        }

        /// <summary>
        /// Produces the meet of deep copies of this labels two components.
        /// </summary>
        /// <returns>A copy of this <see cref="MeetLabel"/>.</returns>
        public override Label Clone() => l1.Clone() - l2.Clone();

        public override bool Equals(Label label) => label is MeetLabel ? Equals(label as MeetLabel) : false;
        public bool Equals(MeetLabel label) => ArrayEquatable.Equals(flatten(), label.flatten());

        private IEnumerable<Label> flatten()
        {
            if (l1 is MeetLabel)
                foreach (var l in (l1 as MeetLabel).flatten())
                    yield return l;
            else
                yield return l1;

            if (l2 is MeetLabel)
                foreach (var l in (l2 as MeetLabel).flatten())
                    yield return l;
            else
                yield return l2;
        }

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="MeetLabel" />
        /// </summary>
        public override Label NoVariables => l1.NoVariables - l2.NoVariables;
        /// <summary>
        /// Replaces a <see cref="ConstantLabel"/> with another label in this <see cref="MeetLabel"/>.
        /// </summary>
        /// <param name="constant">The constant label that should be replaced.</param>
        /// <param name="replacement">The replacement label.</param>
        /// <returns>The result of the label replacement.</returns>
        public override Label ReplaceConstant(ConstantLabel constant, Label replacement) => l1.ReplaceConstant(constant, replacement) - l2.ReplaceConstant(constant, replacement);

        /// <summary>
        /// Gets the first label in the meet.
        /// </summary>
        public Label Label1 => l1;
        /// <summary>
        /// Gets the second label in the meet.
        /// </summary>
        public Label Label2 => l2;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="MeetLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="MeetLabel"/>.
        /// </returns>
        public override string ToString() => $"{l1} \u2293 {l2}";
    }
}
