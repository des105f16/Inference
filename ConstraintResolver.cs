using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Provides a method that implements the label inference algorithm.
    /// </summary>
    public static class ConstraintResolver
    {
        private static void removeLeftJoins(List<Constraint> constraints)
        {
            for (int i = 0; i < constraints.Count; i++)
                if (constraints[i].Left is JoinLabel)
                {
                    var l = constraints[i].Left as JoinLabel;
                    var r = constraints[i].Right;

                    constraints.RemoveAt(i);

                    constraints.Insert(i, new Constraint(l.Label1, r));
                    constraints.Insert(i, new Constraint(l.Label2, r));

                    i--;
                }
        }
        private static void clearTrivials(List<Constraint> constraints)
        {
            for (int i = 0; i < constraints.Count; i++)
                if (
                    constraints[i].Left is LowerBoundLabel ||
                    constraints[i].Right is UpperBoundLabel ||
                    constraints[i].Right == constraints[i].Left
                    )
                    constraints.RemoveAt(i--);
        }

        private static bool resolve(List<Constraint> constraints)
        {
            for (int i = 0; i < constraints.Count; i++)
            {
                if (constraints[i].Left > constraints[i].Right)
                {
                    var c = constraints[i];
                    var v = c.Left as VariableLabel;

                    if (v == null)
                        return false;

                    v.CurrentUpperBound -= c.Right.NoVariables;

                    i = -1;
                }
            }

            return true;
        }

        private static VariableLabel[] getVariables(List<Constraint> constraints)
        {
            return constraints.Select(c => c.Left).OfType<VariableLabel>().Distinct().ToArray();
        }

        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>The collection of variables in the system, or <c>null</c> is labels could not be inferred.</returns>
        public static VariableLabel[] Resolve(IEnumerable<Constraint> constraints)
        {
            List<Constraint> list = new List<Constraint>(constraints);

            removeLeftJoins(list);
            clearTrivials(list);

            if (resolve(list))
                return getVariables(list);
            else
                return null;
        }
        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>The collection of variables in the system, or <c>null</c> is labels could not be inferred.</returns>
        public static VariableLabel[] Resolve(params Constraint[] constraints)
        {
            return Resolve(constraints as IEnumerable<Constraint>);
        }
    }
}
