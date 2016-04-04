using System;
using System.Linq;

namespace DLM.Inference
{
    internal static class BinaryOperations
    {
        public class Join : BinaryOperation<Label>
        {
            public Join()
            {
                Add<LowerBoundLabel>((x, y) => x, (x, y) => x);
                Add<UpperBoundLabel>((x, y) => y, (x, y) => x);
                Add<PolicyLabel>(null, (x, y) => x + y);
            }

            protected override Label Default(Label l1, Label l2)
            {
                return new JoinLabel(l1, l2);
            }
        }
        public class Meet : BinaryOperation<Label>
        {
            public Meet()
            {
                Add<LowerBoundLabel>((x, y) => y, (x, y) => x);
                Add<UpperBoundLabel>((x, y) => x, (x, y) => x);
                Add<PolicyLabel>(null, (x, y) => x - y);
            }

            protected override Label Default(Label l1, Label l2)
            {
                return new MeetLabel(l1, l2);
            }
        }
        public class NoMoreRestrictive : BinaryOperation<bool>
        {
            public NoMoreRestrictive()
            {
                Add<JoinLabel>(
                    (x, y) => Apply(x.Label1, y) && Apply(x.Label2, y),
                    (x, y) => Apply(x, y.Label1) || Apply(x, y.Label2),
                    (x, y) => Apply(x.Label1, y) && Apply(x.Label2, y));
                Add<LowerBoundLabel>((x, y) => true, (x, y) => false, (x, y) => true);
                Add<UpperBoundLabel>((x, y) => false, (x, y) => true, (x, y) => true);
                Add<ConstantLabel>((x, y) => false, (x, y) => false, (x, y) => x.Equals(y));
                Add<PolicyLabel>((x, y) => false, (x, y) => false, policies);
            }

            private bool policies(PolicyLabel l1, PolicyLabel l2)
            {
                foreach (var o in l1.Owners())
                {
                    if (!l2.Owners().Contains(o))
                        return false;

                    foreach (var r in l2.Readers(o))
                        if (!l1.Readers(o).Contains(r))
                            return false;
                }

                return true;
            }

            protected override bool Default(Label l1, Label l2)
            {
                if (l1.Equals(l2))
                    return true;
                else
                    throw new InvalidOperationException($"The <= operator has no definition for; {l1.GetType().Name} <= {l2.GetType().Name}");
            }
        }
    }
}
