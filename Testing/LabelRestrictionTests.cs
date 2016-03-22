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
            Assert.IsFalse(Label.UpperBound <= p1);
            Assert.IsTrue(p1 <= Label.UpperBound);
            Assert.IsFalse(Label.UpperBound <= p2);
            Assert.IsTrue(p2 <= Label.UpperBound);

            Assert.IsFalse(Label.UpperBound <= c1);
            Assert.IsTrue(c1 <= Label.UpperBound);
            Assert.IsFalse(Label.UpperBound <= c2);
            Assert.IsTrue(c2 <= Label.UpperBound);

            Assert.IsFalse(Label.UpperBound <= Label.LowerBound);
            Assert.IsTrue(Label.LowerBound <= Label.UpperBound);
            Assert.IsTrue(Label.UpperBound <= Label.UpperBound);
        }

        [TestMethod()]
        public void LowerBound()
        {
            Assert.IsTrue(Label.LowerBound <= p1);
            Assert.IsFalse(p1 <= Label.LowerBound);
            Assert.IsTrue(Label.LowerBound <= p2);
            Assert.IsFalse(p2 <= Label.LowerBound);

            Assert.IsTrue(Label.LowerBound <= c1);
            Assert.IsFalse(c1 <= Label.LowerBound);
            Assert.IsTrue(Label.LowerBound <= c2);
            Assert.IsFalse(c2 <= Label.LowerBound);

            Assert.IsTrue(Label.LowerBound <= Label.UpperBound);
            Assert.IsFalse(Label.UpperBound <= Label.LowerBound);
            Assert.IsTrue(Label.LowerBound <= Label.LowerBound);
        }

        [TestMethod()]
        public void Constant()
        {
            Assert.IsFalse(c1 <= c2);
            Assert.IsFalse(c2 <= c1);

            Assert.IsTrue(c1 <= c1);
            Assert.IsTrue(c2 <= c2);

            Assert.IsFalse(p1 <= c1);
            Assert.IsFalse(c1 <= p1);

            Assert.IsFalse(p1 <= c2);
            Assert.IsFalse(c2 <= p1);

            Assert.IsFalse(p2 <= c1);
            Assert.IsFalse(c1 <= p2);

            Assert.IsFalse(p2 <= c2);
            Assert.IsFalse(c2 <= p2);
        }
    }
}
