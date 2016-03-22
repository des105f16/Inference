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
    }
}
