namespace DLM.Inference
{
    /// <summary>
    /// Defines the top-level type of the label composite structure.
    /// </summary>
    public abstract class Label
    {
        /// <summary>
        /// Gets the lower bound label.
        /// </summary>
        public static Label LowerBound => LowerBoundLabel.Singleton;
        /// <summary>
        /// Gets the upper bound label.
        /// </summary>
        public static Label UpperBound => UpperBoundLabel.Singleton;

        /// <summary>
        /// Generates the join of the two labels.
        /// </summary>
        /// <param name="l1">The first label in the join.</param>
        /// <param name="l2">The second label in the join.</param>
        /// <returns>
        /// A new label that is the join of <paramref name="l1"/> and <paramref name="l2"/>.
        /// </returns>
        public static Label operator +(Label l1, Label l2)
        {
            return Join((dynamic)l1, (dynamic)l2);
        }
        /// <summary>
        /// Generates the meet of the two labels.
        /// </summary>
        /// <param name="l1">The first label in the meet operation.</param>
        /// <param name="l2">The second label in the meet operation.</param>
        /// <returns>
        /// A new label that is the meet of <paramref name="l1"/> and <paramref name="l2"/>.
        /// </returns>
        public static Label operator -(Label l1, Label l2)
        {
            return Meet((dynamic)l1, (dynamic)l2);
        }

        private static Label Join(Label l1, Label l2)
        {
            return new JoinLabel(l1, l2);
        }
        private static Label Meet(Label l1, Label l2)
        {
            return new MeetLabel(l1, l2);
        }
    }
}
