using NUnit.Framework;
using DLM.Inference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLM.Inference.Tests
{
    [TestFixture()]
    public class PrincipalTests
    {
        private Principal p1, p2;
        private Principal carl, manager, doctor, bob, amy, group;

        [SetUp()]
        public void Initialize()
        {
            p1 = new Principal(nameof(p1));
            p2 = new Principal(nameof(p2));

            carl = new Principal(nameof(carl));
            manager = new Principal(nameof(manager));
            doctor = new Principal(nameof(doctor));
            bob = new Principal(nameof(bob));
            amy = new Principal(nameof(amy));
            group = new Principal(nameof(group));
        }

        [Test()]
        public void AddSubordinateTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void ActualSubordinatesTest()
        {
            Assert.Fail();
        }
    }
}