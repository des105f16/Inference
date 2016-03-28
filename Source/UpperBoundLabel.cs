namespace DLM.Inference
{
    /// <summary>
    /// Represents the most restrictive label; everybody is an owner and nobody can read.
    /// </summary>
    public class UpperBoundLabel : Label
    {
        private static UpperBoundLabel label = new UpperBoundLabel();

        /// <summary>
        /// Gets the singleton upper bound label.
        /// </summary>
        public static UpperBoundLabel Singleton => label;

        private UpperBoundLabel()
        {
        }

        public override bool Equals(Label label) => label is UpperBoundLabel;

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="UpperBoundLabel" />
        /// </summary>
        public override Label NoVariables => this;
        /// <summary>
        /// Returns a <see cref="UpperBoundLabel"/>; see <see cref="Label.ReplaceConstant(ConstantLabel, Label)"/>.
        /// </summary>
        /// <param name="constant">The constant label that should be replaced.</param>
        /// <param name="replacement">The replacement label.</param>
        /// <returns>A <see cref="UpperBoundLabel"/>.</returns>
        public override Label ReplaceConstant(ConstantLabel constant, Label replacement) => this;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="UpperBoundLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="UpperBoundLabel"/>.
        /// </returns>
        public override string ToString() => "\u22a4";
    }
}
