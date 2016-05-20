using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLM.Inference.Tests
{
    [TestFixture()]
    public class PolicyTests
    {
        private PolicyLabel l1, l2, l3, l4, l5, l6, l7, l8, l9, l10;

        private Principal o1, o2, o3, r1, r2, r3;

        [SetUp()]
        public void Initialize()
        {
            o1 = new Principal(nameof(o1));
            o2 = new Principal(nameof(o2));
            o3 = new Principal(nameof(o3));
            r1 = new Principal(nameof(r1));
            r2 = new Principal(nameof(r2));
            r3 = new Principal(nameof(r3));

            l1 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r1, r2, r3),
                new Policy(o2, r1, r2, r3)
            });

            l2 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r1, r2, r3),
                new Policy(o2, r2)
            });

            l3 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r2),
                new Policy(o2)
            });

            l4 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1),
                new Policy(o2)
            });

            l5 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r1),
                new Policy(o2, r2)
            });

            l6 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r1),
                new Policy(o2, r2),
                new Policy(o3, r2)
            });

            l7 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r1),
                new Policy(o2, r2),
                new Policy(o3, r3)
            });

            l8 = new PolicyLabel(new List<Policy>
            {
                new Policy(o1, r2),
                new Policy(o2, r2),
                new Policy(o3, r2)
            });

            l9 = new PolicyLabel(new Policy(o1, r2));

            l10 = new PolicyLabel(new Policy(o1));
        }

        [Test()]
        public void EffectiveReadersTest()
        {
            CollectionAssert.AreEquivalent(l1.EffectiveReaders, new Principal[] { r1, r2, r3 });
            CollectionAssert.AreEquivalent(l2.EffectiveReaders, new Principal[] { r2 });
            CollectionAssert.AreEquivalent(l3.EffectiveReaders, new Principal[0]);
            CollectionAssert.AreEquivalent(l4.EffectiveReaders, new Principal[0]);
            CollectionAssert.AreEquivalent(l5.EffectiveReaders, new Principal[0]);
            CollectionAssert.AreEquivalent(l6.EffectiveReaders, new Principal[0]);
            CollectionAssert.AreEquivalent(l7.EffectiveReaders, new Principal[0]);
            CollectionAssert.AreEquivalent(l8.EffectiveReaders, new Principal[] { r2 });
            CollectionAssert.AreEquivalent(l9.EffectiveReaders, new Principal[] { r2 });
            CollectionAssert.AreEquivalent(l10.EffectiveReaders, new Principal[0]);
        }
    }
}
