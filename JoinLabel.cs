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

        protected internal override bool LessRestrictiveThan(PolicyLabel label) => l1 <= label && l2 <= label;
        protected internal override bool LessRestrictiveThan(ConstantLabel label) => l1 <= label && l2 <= label;

        protected internal override bool LessRestrictiveThan(JoinLabel label) => l1 <= label && l2 <= label;
        protected internal override bool LessRestrictiveThan(MeetLabel label) => l1 <= label && l2 <= label;

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
