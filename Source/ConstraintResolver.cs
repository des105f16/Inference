﻿using System;
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
        private List<Step> steps;
        private ConstraintBuilder<T> builder;

        private ConstraintResolver(ConstraintBuilder<T> builder, IEnumerable<T> constraints)
        {
            this.constraints = new List<T>(constraints);
            this.steps = new List<Step>();
            this.builder = builder;
        }

        private void removeLeftJoins()
        {
            for (int i = 0; i < constraints.Count; i++)
                if (constraints[i].Left is JoinLabel)
                {
                    var c = constraints[i];
                    constraints.RemoveAt(i);

                    var join = c.Left as JoinLabel;

                    constraints.Insert(i, builder(c, join.Label1, c.Right));
                    constraints.Insert(i, builder(c, join.Label2, c.Right));

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
                        steps.Add(new Step(constraints[i], false));
                        return false;
                    }
                    else
                    {
                        steps.Add(new Step(constraints[i], true));
                        v.CurrentUpperBound -= c.Right.NoVariables;

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

            return new InferenceResult<T>(succes, resolver.getVariables(), resolver.constraints, constraints, resolver.steps);
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

        /// <summary>
        /// Represents a step in the inference algorithm.
        /// </summary>
        public class Step
        {
            private readonly T constraint;
            private readonly Label left, right, result;

            private readonly bool success;

            /// <summary>
            /// Initializes a new instance of the <see cref="Step" /> class.
            /// </summary>
            /// <param name="constraint">The constraint that should be updated.</param>
            /// <param name="success">A boolean value indicating if the step was succesfull.</param>
            public Step(T constraint, bool success)
            {
                this.constraint = constraint;

                this.left = constraint.Left.Clone();
                this.right = constraint.Right.Clone();

                this.success = success;
                if (this.success)
                    this.result = this.left.NoVariables - this.right.NoVariables;
                else
                    this.result = null;
            }

            /// <summary>
            /// Gets the constraint that should be updated by this step.
            /// </summary>
            public T Constraint => constraint;

            /// <summary>
            /// Gets the left side of the constraint.
            /// </summary>
            public Label Left => left;
            /// <summary>
            /// Gets the right side of the constraint.
            /// </summary>
            public Label Right => right;

            /// <summary>
            /// Gets a value indicating whether this <see cref="Step"/> succesfully updated the left side of the constraint.
            /// If <c>true</c> the updated value can be read from <see cref="Result"/>.
            /// </summary>
            public bool Success => success;
            /// <summary>
            /// Gets the updated label for the left side of the constraint, if <see cref="Success"/> is <c>true</c>; otherwise <c>null</c>.
            /// </summary>
            public Label Result => result;
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
