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

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="UpperBoundLabel" />
        /// </summary>
        public override Label NoVariables => this;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="UpperBoundLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="UpperBoundLabel"/>.
        /// </returns>
        public override string ToString() => "\u22a4";
    }
}
