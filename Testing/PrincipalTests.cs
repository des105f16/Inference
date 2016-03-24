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
        private Principal carl, manager, doctor, bob, amy, group;

        [SetUp()]
        public void Initialize()
        {
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