﻿using System.Collections.Generic;

namespace DLM.Inference
{
    /// <summary>
    /// Represents the join of two other labels.
    /// </summary>
    public class JoinLabel : Label
    {
        private Label l1, l2;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinLabel"/> class.
        /// </summary>
        /// <param name="l1">The first label in the join.</param>
        /// <param name="l2">The second label in the join.</param>
        public JoinLabel(Label l1, Label l2)
        {
            this.l1 = l1;
            this.l2 = l2;
        }
        
        public override bool Equals(Label label) => label is JoinLabel ? Equals(label as JoinLabel) : false;
        public bool Equals(JoinLabel label) => ArrayEquatable.Equals(flatten(), label.flatten());

        private IEnumerable<Label> flatten()
        {
            if (l1 is JoinLabel)
                foreach (var l in (l1 as JoinLabel).flatten())
                    yield return l;
            else
                yield return l1;

            if (l2 is JoinLabel)
                foreach (var l in (l2 as JoinLabel).flatten())
                    yield return l;
            else
                yield return l2;
        }

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="JoinLabel" />
        /// </summary>
        public override Label NoVariables => l1.NoVariables + l2.NoVariables;

        /// <summary>
        /// Gets the first label in the join.
        /// </summary>
        public Label Label1 => l1;
        /// <summary>
        /// Gets the second label in the join.
        /// </summary>
        public Label Label2 => l2;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="JoinLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="JoinLabel"/>.
        /// </returns>
        public override string ToString() => $"{l1} \u2294 {l2}";
    }
}