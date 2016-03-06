using System.Collections.Generic;

namespace DLM.Inference
{
    /// <summary>
    /// Represents a stack of labels that are merged using either the join or the meet operation.
    /// </summary>
    public class LabelStack
    {
        private bool join;
        private Stack<Label> labels;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelStack"/> class.
        /// </summary>
        /// <param name="join">if set to <c>true</c>, labels will be merged by join; otherwise by meet.</param>
        public LabelStack(bool join)
        {
            this.join = join;
            this.labels = new Stack<Label>();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="LabelStack"/> to <see cref="Label"/>.
        /// This allows the stack to take the place of a label in case of an argument.
        /// It also allows label operators to apply to the stack.
        /// </summary>
        /// <param name="stack">The stack that should be converted.</param>
        /// <returns>
        /// The current value of the stacks <see cref="LabelStack.Label"/> property.
        /// </returns>
        public static implicit operator Label(LabelStack stack) => stack.Label;

        /// <summary>
        /// Pushes a new label onto the stack.
        /// </summary>
        /// <param name="label">The label that should be added to the stack using join or meet.</param>
        public void Push(Label label)
        {
            if (join)
                labels.Push(label + Label);
            else
                labels.Push(label - Label);
        }
        /// <summary>
        /// Removes the top-most label from the stack.
        /// </summary>
        public void Pop()
        {
            labels.Pop();
        }

        /// <summary>
        /// Gets a value indicating whether this stack merges labels using join.
        /// </summary>
        public bool IsJoin => join;
        /// <summary>
        /// Gets a value indicating whether this stack merges labels using meet.
        /// </summary>
        public bool IsMeet => !join;

        /// <summary>
        /// Gets the join or meet of all the labels in this <see cref="LabelStack"/>.
        /// </summary>
        public Label Label
        {
            get
            {
                if (labels.Count == 0)
                    if (join)
                        return Label.LowerBound;
                    else
                        return Label.UpperBound;
                else
                    return labels.Peek();
            }
        }
    }
}
