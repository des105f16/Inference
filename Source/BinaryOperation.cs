using System;
using System.Collections.Generic;

namespace DLM.Inference
{
    internal abstract class BinaryOperation<TResult>
    {
        #region handle wrapper

        private class Handle
        {
            private Type type;

            private Func<Label, Label, TResult> opLeft, opRight, opBoth;

            public Handle(Type type, Func<Label, Label, TResult> opLeft, Func<Label, Label, TResult> opRight, Func<Label, Label, TResult> opBoth)
            {
                this.type = type;
                this.opLeft = opLeft;
                this.opRight = opRight;
                this.opBoth = opBoth;
            }

            public Type Type => type;
            public Func<Label, Label, TResult> OpLeft => opLeft;
            public Func<Label, Label, TResult> OpRight => opRight;
            public Func<Label, Label, TResult> OpBoth => opBoth;
        }

        #endregion

        private List<Handle> handles;

        public BinaryOperation()
        {
            this.handles = new List<Handle>();
        }

        protected void Add<TLabel>(Func<TLabel, Label, TResult> op1, Func<Label, TLabel, TResult> op2, Func<TLabel, TLabel, TResult> op3) where TLabel : Label
        {
            Func<Label, Label, TResult> f1 = (l1, l2) => op1(l1 as TLabel, l2 as Label);
            Func<Label, Label, TResult> f2 = (l1, l2) => op2(l1 as Label, l2 as TLabel);
            Func<Label, Label, TResult> f3 = (l1, l2) => op3(l1 as TLabel, l2 as TLabel);

            if (op1 == null) f1 = null;
            if (op2 == null) f2 = null;
            if (op3 == null) f3 = null;

            handles.Add(new Handle(typeof(TLabel), f1, f2, f3));
        }
        protected void Add<TLabel>(Func<TLabel, Label, TResult> op1, Func<TLabel, TLabel, TResult> op2) where TLabel : Label
        {
            if (op1 == null)
                Add(null, null, op2);
            else
                Add(op1, (l1, l2) => op1(l2, l1), op2);
        }

        public TResult Apply(Label l1, Label l2)
        {
            for (int i = 0; i < handles.Count; i++)
            {
                var t0 = handles[i].Type;
                var t1 = l1.GetType();
                var t2 = l2.GetType();

                if (t0 == t1 && t0 == t2)
                {
                    if (handles[i].OpBoth != null)
                        return handles[i].OpBoth(l1, l2);
                }
                else if (t0 == t1)
                {
                    if (handles[i].OpBoth != null)
                        return handles[i].OpLeft(l1, l2);
                }
                else if (t0 == t2)
                {
                    if (handles[i].OpBoth != null)
                        return handles[i].OpRight(l1, l2);
                }
            }

            return Default(l1, l2);
        }

        protected abstract TResult Default(Label l1, Label l2);
    }
}
