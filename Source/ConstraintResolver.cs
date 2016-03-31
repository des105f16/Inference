using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Provides methods that implement the label inference algorithm.
    /// </summary>
    public class ConstraintResolver
    {
        private List<Constraint> constraints;

        private ConstraintResolver(IEnumerable<Constraint> constraints)
        {
            this.constraints = new List<Constraint>(constraints);
        }

        private void removeLeftJoins()
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
        private void clearTrivials()
        {
            for (int i = 0; i < constraints.Count; i++)
                if (
                    constraints[i].Left is LowerBoundLabel ||
                    constraints[i].Right is UpperBoundLabel ||
                    constraints[i].Right == constraints[i].Left
                    )
                    constraints.RemoveAt(i--);
        }
        private void removeDuplicates()
        {
            for (int i = 0; i < constraints.Count - 1; i++)
                for (int j = i + 1; j < constraints.Count; j++)
                    if (constraints[i].Left.Equals(constraints[j].Left) &&
                        constraints[i].Right.Equals(constraints[j].Right))
                        constraints.RemoveAt(j--);
        }

        private bool resolve()
        {
            for (int i = 0; i < constraints.Count; i++)
            {
                if (!(constraints[i].Left <= constraints[i].Right))
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

        private VariableLabel[] getVariables()
        {
            return constraints.Select(c => c.Left).OfType<VariableLabel>().Distinct().ToArray();
        }

        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>A <see cref="InferenceResult"/> representing the result of the method.
        /// <see cref="InferenceResult.Succes"/> indicates if the constraints were successfully resolved.</returns>
        public static InferenceResult Resolve(IEnumerable<Constraint> constraints)
        {
            var resolver = new ConstraintResolver(constraints);

            resolver.removeLeftJoins();
            resolver.clearTrivials();
            resolver.removeDuplicates();
            bool succes = resolver.resolve();

            return new InferenceResult(succes, resolver.getVariables(), resolver.constraints);
        }
        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>A <see cref="InferenceResult"/> representing the result of the method.
        /// <see cref="InferenceResult.Succes"/> indicates if the constraints were successfully resolved.</returns>
        public static InferenceResult Resolve(params Constraint[] constraints)
        {
            return Resolve(constraints as IEnumerable<Constraint>);
        }
    }
}
