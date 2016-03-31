namespace DLM.Inference
{
    /// <summary>
    /// Creates a new constraint based on some existing constraint and its label components.
    /// Provides a means of maintaining meta-data on <see cref="Constraint"/> subclasses.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Constraint"/> created by this <see cref="ConstraintBuilder{T}"/>.</typeparam>
    /// <param name="origin">The constraint from which meta-data should be retrieved.</param>
    /// <param name="left">The left label in the constraint. See <see cref="Constraint(Label, Label)"/>.</param>
    /// <param name="right">The right label in the constraint. See <see cref="Constraint(Label, Label)"/>.</param>
    /// <returns>A new constraint based on <paramref name="left"/> and <paramref name="right"/> with meta-data from <paramref name="origin"/>.</returns>
    public delegate T ConstraintBuilder<T>(T origin, Label left, Label right) where T : Constraint;
}
