using Solarflare.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solaflare.UnitTest
{
    [TestFixture]
    public class EvaluatorTest
    {
        [Test]
        public void EvaluateTest_WithOnlyTerm() 
        {
            Parser parser = new Parser("5 + 8 - 2");
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(parser.GenerateTree());

            Assert.That(result == 11);
        }

        [Test]
        public void EvaluateTest_WithOnlyFactor()
        {
            Parser parser = new Parser("2 * 6 / 3");
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(parser.GenerateTree());

            Assert.That(result == 4);
        }

        [Test]
        public void EvaluateTest_WithTermAndFactor()
        {
            Parser parser = new Parser("5 + 8 * 2");
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(parser.GenerateTree());

            Assert.That(result == 21);
        }

        [Test]
        public void EvaluateTest_WithParenthesis()
        {
            Parser parser = new Parser("(5 + 8) * 2");
            Evaluator evaluator = new Evaluator();
            var result = evaluator.Evaluate(parser.GenerateTree());

            Assert.That(result == 26);
        }
    }
}
