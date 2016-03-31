namespace DLM.Inference
{
    /// <summary>
    /// Represents the least restrictive label; has no owners.
    /// </summary>
    public class LowerBoundLabel : Label
    {
        private static LowerBoundLabel label = new LowerBoundLabel();

        /// <summary>
        /// Gets the singleton lower bound label.
        /// </summary>
        public static LowerBoundLabel Singleton => label;

        private LowerBoundLabel()
        {
        }

        /// <summary>
        /// Returns this <see cref="LowerBoundLabel"/>, which is a singleton object.
        /// </summary>
        /// <returns>This <see cref="LowerBoundLabel"/>.</returns>
        public override Label Clone() => this;

        public override bool Equals(Label label) => label is LowerBoundLabel;

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="LowerBoundLabel" />
        /// </summary>
        public override Label NoVariables => this;
        /// <summary>
        /// Returns a <see cref="LowerBoundLabel"/>; see <see cref="Label.ReplaceConstant(ConstantLabel, Label)"/>.
        /// </summary>
        /// <param name="constant">The constant label that should be replaced.</param>
        /// <param name="replacement">The replacement label.</param>
        /// <returns>A <see cref="LowerBoundLabel"/>.</returns>
        public override Label ReplaceConstant(ConstantLabel constant, Label replacement) => this;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="LowerBoundLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="LowerBoundLabel"/>.
        /// </returns>
        public override string ToString() => "\u22a5";
    }
}
