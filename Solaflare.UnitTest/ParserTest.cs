using Solarflare.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Solaflare.UnitTest
{
    [TestFixture]
    public class ParserTest
    {

        //5+8+1
        //    +
        //   / \
        //  5   +
        //     / \ 
        //    8   1   
        //
        [Test]
        public void ParseStatementTest_WithTermOnly()
        {
            Parser parser = new Parser("5+8+1");
            var root = parser.GenerateTree();

            Assert.That(root, Is.TypeOf(typeof(BinaryNode)));
            Assert.IsTrue(parser.Errors.Count() == 0);

            var children = root.GetChildren().ToArray();

            Assert.That(children[2], Is.TypeOf(typeof(BinaryNode)));
            Assert.That(children[1].Token.Value.ToString() == "+");
            Assert.That(children[0].Token.Value.ToString() == "5");

            children = children[2].GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "8");
            Assert.That(children[1].Token.Value.ToString() == "+");
            Assert.That(children[2].Token.Value.ToString() == "1");
        }

        //5+8*9
        //  +
        // / \
        //5   *
        //   / \
        //  8   9
        //
        [Test]
        public void ParseStatementTest_WithTermandFactor()
        {
            Parser parser = new Parser("5+8*9");
            var root = parser.GenerateTree();

            var children = root.GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "5");
            Assert.That(children[1].Token.Value.ToString() == "+");
            Assert.That(children[2], Is.TypeOf(typeof(BinaryNode)));

            children = children[2].GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "8");
            Assert.That(children[1].Token.Value.ToString() == "*");
            Assert.That(children[2].Token.Value.ToString() == "9");
        }


        //(5+8)*9
        //    *
        //   / \
        //  ()  9
        //  |
        //  +
        // / \
        //5   8
        //
        [Test]
        public void ParseStatementTest_WithParenthesis()
        {
            Parser parser = new Parser("(5+8)*9");
            var root = parser.GenerateTree();

            var children = root.GetChildren().ToArray();

            Assert.That(children[0], Is.TypeOf(typeof(ParenthesisNode)));
            Assert.That(children[1].Token.Value.ToString() == "*");
            Assert.That(children[2].Token.Value.ToString() == "9");       

            children = children[0].GetChildren().ToArray();

            Assert.That(children[0], Is.TypeOf(typeof(BinaryNode)));

            children = children[0].GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "5");
            Assert.That(children[1].Token.Value.ToString() == "+");
            Assert.That(children[2].Token.Value.ToString() == "8");
        }

        /// -
        /// |
        /// 1
        [Test]
        public void ParseStatementTest_SimpleUnary()
        {
            Parser parser = new Parser("-1");
            var root = parser.GenerateTree();

            Assert.That(root, Is.TypeOf(typeof(UnaryNode)));

            var children = root.GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "-");
            Assert.That(children[1].Token.Value.ToString() == "1");
        }

        //     *
        //    / \
        //   5   -
        //       |
        //       7
        [Test]
        public void ParseStatementTest_UnaryOperator()
        {
            Parser parser = new Parser("5 * -7");
            var root = parser.GenerateTree();

            var children = root.GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "5");
            Assert.That(children[1].Token.Value.ToString() == "*");       
            Assert.That(children[2], Is.TypeOf(typeof(UnaryNode)));

            children = children[2].GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "-");
            Assert.That(children[1].Token.Value.ToString() == "7");
        }

        //     -
        //     |
        //     +
        //    / \ 
        //   8   7
        [Test]
        public void ParseStatementTest_UnaryOperatorWithParenthesis()
        {
            Parser parser = new Parser("-(8 + 7)");
            var root = parser.GenerateTree();

            Assert.That(root, Is.TypeOf(typeof(UnaryNode)));

            var children = root.GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "-");
            Assert.That(children[1], Is.TypeOf(typeof(ParenthesisNode)));

            children = children[1].GetChildren().ToArray();

            Assert.That(children[0], Is.TypeOf(typeof(BinaryNode)));

            children = children[0].GetChildren().ToArray();

            Assert.That(children[0].Token.Value.ToString() == "8");
            Assert.That(children[1].Token.Value.ToString() == "+");
            Assert.That(children[2].Token.Value.ToString() == "7");
        }
    }
}
