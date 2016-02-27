using System;

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
        /// Determines if one label is less (or equally) restrictive than another one.
        /// Note that this operator employs the <see cref="Label.NoVariables"/> property for comparison.
        /// </summary>
        /// <param name="l1">The least restrictive label.</param>
        /// <param name="l2">The most restrictive label.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="l1"/> is less (or equally) restrictive than <paramref name="l2"/>.
        /// </returns>
        public static bool operator <=(Label l1, Label l2)
        {
            return l1.NoVariables.lessRestrictiveThan(l2.NoVariables);
        }
        /// <summary>
        /// Determines if one label is more (or equally) restrictive than another one.
        /// Note that this operator employs the <see cref="Label.NoVariables"/> property for comparison.
        /// </summary>
        /// <param name="l1">The most restrictive label.</param>
        /// <param name="l2">The least restrictive label.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="l1"/> is more (or equally) restrictive than <paramref name="l2"/>.
        /// </returns>
        public static bool operator >=(Label l1, Label l2) => l2 <= l1;

        private bool lessRestrictiveThan(Label label)
        {
            return LessRestrictiveThan((dynamic)label);
        }

        internal virtual bool LessRestrictiveThan(PolicyLabel label)
        {
            throw new NotImplementedException();
        }
        internal virtual bool LessRestrictiveThan(ConstantLabel label)
        {
            throw new NotImplementedException();
        }
        internal bool LessRestrictiveThan(VariableLabel label)
        {
            throw new NotImplementedException();
        }

        internal virtual bool LessRestrictiveThan(JoinLabel label)
        {
            throw new NotImplementedException();
        }
        internal virtual bool LessRestrictiveThan(MeetLabel label)
        {
            throw new NotImplementedException();
        }

        internal bool LessRestrictiveThan(LowerBoundLabel label) => this is LowerBoundLabel;
        internal bool LessRestrictiveThan(UpperBoundLabel label) => true;

        /// <summary>
        /// Gets the upper bound estimate <see cref="Label"/> of this <see cref="Label"/>
        /// </summary>
        public abstract Label NoVariables { get; }

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

        #region Basic joins

        private static Label Join(LowerBoundLabel l1, LowerBoundLabel l2)
        {
            return LowerBound;
        }
        private static Label Join(LowerBoundLabel l1, Label l2)
        {
            return l2;
        }
        private static Label Join(Label l1, LowerBoundLabel l2)
        {
            return l1;
        }

        private static Label Join(UpperBoundLabel l1, UpperBoundLabel l2)
        {
            return UpperBound;
        }
        private static Label Join(UpperBoundLabel l1, Label l2)
        {
            return UpperBound;
        }
        private static Label Join(Label l1, UpperBoundLabel l2)
        {
            return UpperBound;
        }

        #endregion

        #region Basic meets

        private static Label Meet(LowerBoundLabel l1, LowerBoundLabel l2)
        {
            return LowerBound;
        }
        private static Label Meet(LowerBoundLabel l1, Label l2)
        {
            return LowerBound;
        }
        private static Label Meet(Label l1, LowerBoundLabel l2)
        {
            return LowerBound;
        }

        private static Label Meet(UpperBoundLabel l1, UpperBoundLabel l2)
        {
            return UpperBound;
        }
        private static Label Meet(UpperBoundLabel l1, Label l2)
        {
            return l2;
        }
        private static Label Meet(Label l1, UpperBoundLabel l2)
        {
            return l1;
        }

        #endregion
    }
}
