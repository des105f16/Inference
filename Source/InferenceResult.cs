using System.Collections.Generic;
using System.Linq;

namespace DLM.Inference
{
    /// <summary>
    /// Represents the result of a call to the <see cref="ConstraintResolver"/>.
    /// </summary>
    public class InferenceResult
    {
        private bool succes;
        private VariableLabel[] variables;
        private Constraint[] constraints;

        /// <summary>
        /// Initializes a new instance of the <see cref="InferenceResult"/> class.
        /// </summary>
        /// <param name="succes">Indicates if the inference was successful or not.</param>
        /// <param name="variables">The collection of variables in the system.</param>
        /// <param name="constraints">The collection of constraints in the system.</param>
        public InferenceResult(bool succes, IEnumerable<VariableLabel> variables, IEnumerable<Constraint> constraints)
        {
            this.succes = succes;
            this.variables = variables.ToArray();
            this.constraints = constraints.ToArray();
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
        public Constraint[] Constraints => constraints;

        /// <summary>
        /// Gets the collection of constraints in the system that could not be verified.
        /// If <see cref="Succes"/> is <c>true</c>, this will return an empty array.
        /// </summary>
        public Constraint[] ErrorConstraints => constraints.Where(x => x.Left > x.Right).ToArray();
    }
}
