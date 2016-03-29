using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a stack of <see cref="Principal"/> that can be interpreted as a <see cref="Label"/>.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{DLM.Inference.Principal}" />
    public class AuthorityLabel : IEnumerable<Principal>
    {
        private readonly Stack<Principal> principals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorityLabel"/> class.
        /// </summary>
        public AuthorityLabel()
        {
            this.principals = new Stack<Principal>();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AuthorityLabel"/> to <see cref="Label"/>.
        /// This allows the stack to take the place of a label in case of an argument.
        /// It also allows label operators to apply to the stack.
        /// </summary>
        /// <param name="label">The stack that should be converted.</param>
        /// <returns>
        /// The current value of the stacks <see cref="AuthorityLabel.Label"/> property.
        /// </returns>
        public static implicit operator Label(AuthorityLabel label) => label.Label;

        /// <summary>
        /// Pushes a principal onto the stack.
        /// </summary>
        /// <param name="p">The p.</param>
        public void Push(Principal p)
        {
            principals.Push(p);
        }
        /// <summary>
        /// Pops the last principal of the stack.
        /// </summary>
        /// <returns></returns>
        public Principal Pop()
        {
            return principals.Pop();
        }

        /// <summary>
        /// Gets a <see cref="Label"/> with an owner set equal to the current set of owners in this <see cref="AuthorityLabel"/> and an empty reader set.
        /// </summary>
        public Label Label
        {
            get
            {
                if (principals.Count == 0)
                    return Label.LowerBound;
                else
                    return new PolicyLabel(principals.Select(p => new Policy(p)));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var p in principals)
                yield return p;
        }
        IEnumerator<Principal> IEnumerable<Principal>.GetEnumerator()
        {
            foreach (var p in principals)
                yield return p;
        }
    }
}
