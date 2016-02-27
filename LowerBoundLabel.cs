﻿namespace DLM.Inference
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
        /// Returns a <see cref="string"/> that represents this <see cref="LowerBoundLabel"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this <see cref="LowerBoundLabel"/>.
        /// </returns>
        public override string ToString() => "\u22a5";
    }
}