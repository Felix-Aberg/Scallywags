using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ScallyWags;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ShipTest
    {
        // A Test behaves as an ordinary method
        [TestCase(1, 9)]
        [TestCase(11, 0)]
        [TestCase(5, 5)]
        [TestCase(0, 10)]
        public void ShipTakeDamage(int damage, int expected)
        {
            // Use the Assert class to test conditions
            ShipHealth ship = new ShipHealth(10);

            ship.TakeDamage(damage);

            Assert.That(ship.GetHealth(), Is.EqualTo(expected));
        }
        
        // A Test behaves as an ordinary method
        [TestCase(5, 5, 10)]
        [TestCase(9, 11, 10)]
        [TestCase(10, 1, 1)]
        [TestCase(0, 10, 10)]
        public void ShipFixDamage(int damage, int fix, int expected)
        {
            // Use the Assert class to test conditions
            ShipHealth ship = new ShipHealth(10);

            ship.TakeDamage(damage);
            ship.FixDamage(fix);

            Assert.That(ship.GetHealth(), Is.EqualTo(expected));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ShipTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
