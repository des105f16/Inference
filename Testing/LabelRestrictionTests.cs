using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLM.Inference.Tests
{
    [TestClass()]
    public class LabelRestrictionTests
    {
        private Principal a;
        private Principal b;

        private PolicyLabel p1;
        private PolicyLabel p2;

        private ConstantLabel c1;
        private ConstantLabel c2;

        private VariableLabel v1;
        private VariableLabel v2;

        private static void isTrue(Label l1, Label l2)
        {
            Assert.IsTrue(l1 <= l2, $"{l1} \u2291 {l2} should be true.");
        }
        private static void isFalse(Label l1, Label l2)
        {
            Assert.IsFalse(l1 <= l2, $"{l1} \u2291 {l2} should be false.");
        }

        [TestInitialize()]
        public void Initialize()
        {
            a = new Principal(nameof(a));
            b = new Principal(nameof(b));

            p1 = new PolicyLabel(new Policy(a, a));
            p2 = new PolicyLabel(new Policy(b, b));

            c1 = new ConstantLabel(nameof(c1));
            c2 = new ConstantLabel(nameof(c2));

            v1 = new VariableLabel(nameof(v1));
            v2 = new VariableLabel(nameof(v2));
        }

        [TestMethod()]
        public void UpperBound()
        {
            isFalse(Label.UpperBound, p1);
            isTrue(p1, Label.UpperBound);
            isFalse(Label.UpperBound, p2);
            isTrue(p2, Label.UpperBound);

            isFalse(Label.UpperBound, c1);
            isTrue(c1, Label.UpperBound);
            isFalse(Label.UpperBound, c2);
            isTrue(c2, Label.UpperBound);

            isFalse(Label.UpperBound, Label.LowerBound);
            isTrue(Label.LowerBound, Label.UpperBound);
            isTrue(Label.UpperBound, Label.UpperBound);
        }

        [TestMethod()]
        public void LowerBound()
        {
            isTrue(Label.LowerBound, p1);
            isFalse(p1, Label.LowerBound);
            isTrue(Label.LowerBound, p2);
            isFalse(p2, Label.LowerBound);

            isTrue(Label.LowerBound, c1);
            isFalse(c1, Label.LowerBound);
            isTrue(Label.LowerBound, c2);
            isFalse(c2, Label.LowerBound);

            isTrue(Label.LowerBound, Label.UpperBound);
            isFalse(Label.UpperBound, Label.LowerBound);
            isTrue(Label.LowerBound, Label.LowerBound);
        }

        [TestMethod()]
        public void Constant()
        {
            isFalse(c1, c2);
            isFalse(c2, c1);

            isTrue(c1, c1);
            isTrue(c2, c2);

            isFalse(p1, c1);
            isFalse(c1, p1);

            isFalse(p1, c2);
            isFalse(c2, p1);

            isFalse(p2, c1);
            isFalse(c1, p2);

            isFalse(p2, c2);
            isFalse(c2, p2);
        }
    }
}
