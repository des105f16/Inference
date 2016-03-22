using System;

namespace DLM.Inference
{
    /// <summary>
    /// Defines the top-level type of the label composite structure.
    /// </summary>
    public abstract class Label : IEquatable<Label>
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
            return NoMoreRestrictive((dynamic)l1.NoVariables, (dynamic)l2.NoVariables);
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

        /// <summary>
        /// Determines if one label is equal to another.
        /// </summary>
        /// <param name="l1">One of the labels to compare.</param>
        /// <param name="l2">The other label to compare.</param>
        /// <returns>
        /// <c>true</c>, if the labels are equally restrictive; otherwise <c>false</c>.
        /// </returns>
        public static bool operator ==(Label l1, Label l2)
        {
            if (ReferenceEquals(l1, null) && ReferenceEquals(l2, null))
                return true;
            else if (ReferenceEquals(l1, null) || ReferenceEquals(l2, null))
                return false;
            else
                return l1.Equals(l2);
        }
        /// <summary>
        /// Determines if one label is not equal to another.
        /// </summary>
        /// <param name="l1">One of the labels to compare.</param>
        /// <param name="l2">The other label to compare.</param>
        /// <returns>
        /// <c>true</c>, if the labels are not equally restrictive; otherwise <c>false</c>.
        /// </returns>
        public static bool operator !=(Label l1, Label l2) => !(l1 == l2);

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is a <see cref="Label"/> and as restrictive as this <see cref="Label"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this <see cref="Label"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is a <see cref="Label"/> and as restrictive as this <see cref="Label"/>; otherwise, <c>false</c>.
        /// </returns>
        public sealed override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
                return Equals(obj as Label);
            else
                return false;
        }
        /// <summary>
        /// Determines if this <see cref="Label"/> is exactly as restrictive as another <see cref="Label"/>.
        /// </summary>
        /// <param name="label">The label to compare to.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Label"/> is exactly as restrictive as this <see cref="Label"/>.
        /// </returns>
        public abstract bool Equals(Label label);

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
        private static bool NoMoreRestrictive(Label l1, Label l2)
        {
            if (l1.Equals(l2))
                return true;
            else
                throw new InvalidOperationException($"The <= operator has no definition for; {l1.GetType().Name} <= {l2.GetType().Name}");
        }

        #region Basic joins

        private static Label Join(LowerBoundLabel l1, LowerBoundLabel l2) => LowerBound;
        private static Label Join(LowerBoundLabel l1, Label l2) => l2;
        private static Label Join(Label l1, LowerBoundLabel l2) => l1;

        private static Label Join(UpperBoundLabel l1, UpperBoundLabel l2) => UpperBound;
        private static Label Join(UpperBoundLabel l1, Label l2) => UpperBound;
        private static Label Join(Label l1, UpperBoundLabel l2) => UpperBound;

        private static Label Join(UpperBoundLabel l1, LowerBoundLabel l2) => UpperBound;
        private static Label Join(LowerBoundLabel l1, UpperBoundLabel l2) => UpperBound;

        private static Label Join(PolicyLabel l1, PolicyLabel l2) => l1 + l2;

        #endregion

        #region Basic meets

        private static Label Meet(LowerBoundLabel l1, LowerBoundLabel l2) => LowerBound;
        private static Label Meet(LowerBoundLabel l1, Label l2) => LowerBound;
        private static Label Meet(Label l1, LowerBoundLabel l2) => LowerBound;

        private static Label Meet(UpperBoundLabel l1, UpperBoundLabel l2) => UpperBound;
        private static Label Meet(UpperBoundLabel l1, Label l2) => l2;
        private static Label Meet(Label l1, UpperBoundLabel l2) => l2;

        private static Label Meet(UpperBoundLabel l1, LowerBoundLabel l2) => LowerBound;
        private static Label Meet(LowerBoundLabel l1, UpperBoundLabel l2) => LowerBound;

        private static Label Meet(PolicyLabel l1, PolicyLabel l2) => l1 - l2;

        #endregion

        #region Basic restriction checks

        private static bool NoMoreRestrictive(LowerBoundLabel l1, LowerBoundLabel l2) => true;
        private static bool NoMoreRestrictive(LowerBoundLabel l1, Label l2) => true;
        private static bool NoMoreRestrictive(Label l1, LowerBoundLabel l2) => false;

        private static bool NoMoreRestrictive(UpperBoundLabel l1, UpperBoundLabel l2) => true;
        private static bool NoMoreRestrictive(UpperBoundLabel l1, Label l2) => false;
        private static bool NoMoreRestrictive(Label l1, UpperBoundLabel l2) => true;

        private static bool NoMoreRestrictive(UpperBoundLabel l1, LowerBoundLabel l2) => false;
        private static bool NoMoreRestrictive(LowerBoundLabel l1, UpperBoundLabel l2) => true;

        #endregion

        private static bool NoMoreRestrictive(JoinLabel l1, Label l2) => l1.Label1 <= l2 && l1.Label2 <= l2;
        private static bool NoMoreRestrictive(Label l1, JoinLabel l2) => l1 <= l2.Label1 || l1 <= l2.Label2;
        private static bool NoMoreRestrictive(JoinLabel l1, JoinLabel l2) => l1.Label1 <= l2 && l1.Label2 <= l2;
    }
}
