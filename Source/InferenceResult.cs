using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Represents the result of a call to the <see cref="ConstraintResolver"/>.
    /// </summary>
    /// <typeparam name="T">The type of the constraints that have been resolved.</typeparam>
    public class InferenceResult<T> where T : Constraint
    {
        private bool succes;
        private VariableLabel[] variables;
        private T[] constraints;
        private T[] originalConstraints;
        private T[] steps;

        /// <summary>
        /// Initializes a new instance of the <see cref="InferenceResult" /> class.
        /// </summary>
        /// <param name="succes">Indicates if the inference was successful or not.</param>
        /// <param name="variables">The collection of variables in the system.</param>
        /// <param name="constraints">The collection of constraints in the system.</param>
        /// <param name="originalConstraints">The collection of constraints that were the input to the resolver.</param>
        /// <param name="steps">The collection of steps that were taken to resolve the system (each step represents a meet of left and right).</param>
        public InferenceResult(bool succes, IEnumerable<VariableLabel> variables, IEnumerable<T> constraints, IEnumerable<T> originalConstraints, IEnumerable<T> steps)
        {
            this.succes = succes;
            this.variables = variables.ToArray();
            this.constraints = constraints.ToArray();
            this.originalConstraints = originalConstraints.ToArray();
            this.steps = steps.ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="InferenceResult"/> was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if succes; otherwise, <c>false</c>.
        /// </value>
        public bool Succes => succes;
        /// <summary>
        /// Gets the collection of variables in the system.
        /// The <see cref="VariableLabel.CurrentUpperBound"/> represents their resolved upper bound, or the state at which a constraint failed.
        /// </summary>
        public VariableLabel[] Variables => variables;
        /// <summary>
        /// Gets the collection of constraints in the resolved system.
        /// </summary>
        public T[] Constraints => constraints;
        /// <summary>
        /// Gets the set of constraints that were the input to the resolver.
        /// </summary>
        public T[] OriginalConstraints => originalConstraints;
        /// <summary>
        /// The set of constraints that were updated when attempting to resolve the system.
        /// If <see cref="Succes"/> if <c>false</c> the last constraint in this array will be the one that caused the error.
        /// </summary>
        public T[] ResolveSteps => steps;

        /// <summary>
        /// Gets the collection of constraints in the system that could not be verified.
        /// If <see cref="Succes"/> is <c>true</c>, this will return an empty array.
        /// </summary>
        public T[] ErrorConstraints => constraints.Where(x => !(x.Left <= x.Right)).ToArray();
    }
}
