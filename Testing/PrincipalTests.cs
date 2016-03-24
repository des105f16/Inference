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

            //Hierarchy from Myers2000 - Figure 3
            carl = new Principal(nameof(carl));
            manager = new Principal(nameof(manager));
            doctor = new Principal(nameof(doctor));
            bob = new Principal(nameof(bob));
            amy = new Principal(nameof(amy));
            group = new Principal(nameof(group));

            carl.AddSubordinate(manager);
            carl.AddSubordinate(doctor);
            manager.AddSubordinate(bob);
            manager.AddSubordinate(amy);
            bob.AddSubordinate(group);
            amy.AddSubordinate(group);
        }

        [Test()]
        public void AddSubordinateTest()
        {
            Assert.True(p1.AddSubordinate(p2), $"{p1.Name} can have {p2.Name} added as a subordinate as it doesn't already have it.");
            Assert.False(p1.AddSubordinate(p2), $"{p1.Name} cannot have {p2.Name} added as a subordinate, because it already has it.");
            var ex = Assert.Throws<PrincipalCircularDependencyException>(() => p2.AddSubordinate(p1), $"{p2.Name} cannot have {p2.Name} added as a subordinate, because it would cause a circular dependency");
        }

        [Test()]
        public void ActualSubordinatesTest()
        {
            CollectionAssert.AreEquivalent(carl.ActualSubordinates, new Principal[] { manager, doctor, bob, amy, group });
            CollectionAssert.AreEquivalent(doctor.ActualSubordinates, new Principal[] { });
            CollectionAssert.AreEquivalent(manager.ActualSubordinates, new Principal[] { bob, amy, group });
            CollectionAssert.AreEquivalent(bob.ActualSubordinates, new Principal[] { group });
            CollectionAssert.AreEquivalent(amy.ActualSubordinates, new Principal[] { group });
            CollectionAssert.AreEquivalent(group.ActualSubordinates, new Principal[] { });
        }
    }
}