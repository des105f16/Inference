using System;
using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Provides methods that implement the label inference algorithm.
    /// </summary>
    /// <typeparam name="T">The type of the constraints that should be resolved.</typeparam>
    public class ConstraintResolver<T> where T : Constraint
    {
        private List<T> constraints;
        private List<T> steps;
        private ConstraintBuilder<T> builder;

        private ConstraintResolver(ConstraintBuilder<T> builder, IEnumerable<T> constraints)
        {
            this.constraints = new List<T>(constraints);
            this.steps = new List<T>();
            this.builder = builder;
        }

        private void removeLeftJoins()
        {
            for (int i = 0; i < constraints.Count; i++)
                if (constraints[i].Left is JoinLabel)
                {
                    var l = constraints[i].Left as JoinLabel;
                    var r = constraints[i].Right;

                    constraints.RemoveAt(i);

                    constraints.Insert(i, builder(constraints[i], l.Label1, r));
                    constraints.Insert(i, builder(constraints[i], l.Label2, r));

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
                    {
                        steps.Add(builder(constraints[i], constraints[i].Left.Clone(), constraints[i].Right.Clone()));
                        return false;
                    }
                    else
                    {
                        v.CurrentUpperBound -= c.Right.NoVariables;
                        steps.Add(builder(constraints[i], constraints[i].Left.Clone(), constraints[i].Right.Clone()));

                        i = -1;
                    }
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
        /// <param name="builder">A function that will generate new constraints while preserving meta-data.</param>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>
        /// A <see cref="InferenceResult{T}"/> representing the result of the method.
        /// <see cref="InferenceResult{T}.Succes"/> indicates if the constraints were successfully resolved.
        /// </returns>
        public static InferenceResult<T> Resolve(ConstraintBuilder<T> builder, IEnumerable<T> constraints)
        {
            var resolver = new ConstraintResolver<T>(builder, constraints);

            resolver.removeLeftJoins();
            resolver.clearTrivials();
            resolver.removeDuplicates();
            bool succes = resolver.resolve();

            return new InferenceResult<T>(succes, resolver.getVariables(), resolver.constraints, resolver.steps);
        }
        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="builder">A function that will generate new constraints while preserving meta-data.</param>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>
        /// A <see cref="InferenceResult{T}"/> representing the result of the method.
        /// <see cref="InferenceResult{T}.Succes"/> indicates if the constraints were successfully resolved.
        /// </returns>
        public static InferenceResult<T> Resolve(ConstraintBuilder<T> builder, params T[] constraints)
        {
            return Resolve(builder, constraints as IEnumerable<T>);
        }
    }

    public static class ConstraintResolver
    {
        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>
        /// A <see cref="InferenceResult{T}"/> representing the result of the method.
        /// <see cref="InferenceResult{T}.Succes"/> indicates if the constraints were successfully resolved.
        /// </returns>
        public static InferenceResult<Constraint> Resolve(IEnumerable<Constraint> constraints)
        {
            return ConstraintResolver<Constraint>.Resolve((o, l, r) => new Constraint(l, r), constraints);
        }
        /// <summary>
        /// Implements the label inference algorithm by setting the <see cref="VariableLabel.CurrentUpperBound"/> on all variables.
        /// </summary>
        /// <param name="constraints">The constraint-system that should be resolved.</param>
        /// <returns>
        /// A <see cref="InferenceResult{T}"/> representing the result of the method.
        /// <see cref="InferenceResult{T}.Succes"/> indicates if the constraints were successfully resolved.
        /// </returns>
        public static InferenceResult<Constraint> Resolve(params Constraint[] constraints)
        {
            return ConstraintResolver<Constraint>.Resolve((o, l, r) => new Constraint(l, r), constraints);
        }
    }
}
