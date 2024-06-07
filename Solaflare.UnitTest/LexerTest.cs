using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Solarflare.Compiler;

namespace Solaflare.UnitTest
{
    [TestFixture]
    public class LexerTest
    {
        [Test]
        public void NextTokenTest_WhenTextIsEmpty()
        {
            //Arrange
            Lexer _lexer = new Lexer("");

            //Act
            var token = _lexer.NextToken();

            //Assert
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Kind == TokenKind.EndOfText);
        }

        [Test]
        public void NextTokenTest_WhenTextIsAMathExpression()
        {
            //Arrange
            Lexer _lexer = new Lexer("5+8*9");
            List<Token> tokens = new List<Token>();

            //Act
            Token currentToken;
            do
            {
                currentToken = _lexer.NextToken();
                tokens.Add(currentToken);
            }
            while (currentToken.Kind != TokenKind.EndOfText);

            //Assert
            Assert.IsTrue(tokens.Count() == 6);
            Assert.IsTrue(_lexer.Errors.Count() == 0);

            Assert.IsTrue(tokens[0].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[1].Kind == TokenKind.PlusOperator);
            Assert.IsTrue(tokens[2].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[3].Kind == TokenKind.StarOperator);
            Assert.IsTrue(tokens[4].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[5].Kind == TokenKind.EndOfText);

            Assert.IsTrue(tokens[0].Value.ToString() == "5");
            Assert.IsTrue(tokens[1].Value.ToString() == "+");
            Assert.IsTrue(tokens[2].Value.ToString() == "8");
            Assert.IsTrue(tokens[3].Value.ToString() == "*");
            Assert.IsTrue(tokens[4].Value.ToString() == "9");
            Assert.IsNull(tokens[5].Value);
        }

        [Test]
        public void NextTokenTest_WhenTextHasWhitespace()
        {
            //Arrange
            Lexer _lexer = new Lexer(" 5 +8 ");
            List<Token> tokens = new List<Token>();

            //Act
            Token currentToken;
            do
            {
                currentToken = _lexer.NextToken();
                tokens.Add(currentToken);
            }
            while (currentToken.Kind != TokenKind.EndOfText);

            //Assert
            Assert.IsTrue(tokens.Count() == 7);

            Assert.IsTrue(tokens[0].Kind == TokenKind.Whitespace);
            Assert.IsTrue(tokens[1].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[2].Kind == TokenKind.Whitespace);
            Assert.IsTrue(tokens[3].Kind == TokenKind.PlusOperator);
            Assert.IsTrue(tokens[4].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[5].Kind == TokenKind.Whitespace);
            Assert.IsTrue(tokens[6].Kind == TokenKind.EndOfText);

            Assert.IsTrue(tokens[0].Value.ToString() == " ");
            Assert.IsTrue(tokens[1].Value.ToString() == "5");
            Assert.IsTrue(tokens[2].Value.ToString() == " ");
            Assert.IsTrue(tokens[3].Value.ToString() == "+");
            Assert.IsTrue(tokens[4].Value.ToString() == "8");
            Assert.IsTrue(tokens[5].Value.ToString() == " ");
            Assert.IsNull(tokens[6].Value);
        }


        [Test]
        public void NextTokenTest_WhenTextisEmpty()
        {
            //Arrange
            Lexer _lexer = new Lexer("");
            List<Token> tokens = new List<Token>();

            //Act
            Token currentToken;
            do
            {
                currentToken = _lexer.NextToken();
                tokens.Add(currentToken);
            }
            while (currentToken.Kind != TokenKind.EndOfText);

            //Assert
            Assert.IsTrue(tokens.Count() == 1);
            Assert.IsTrue(_lexer.Errors.Count() == 0);

            Assert.IsTrue(tokens[0].Kind == TokenKind.EndOfText);
            Assert.IsNull(tokens[0].Value);
        }


        [Test]
        public void NextTokenTest_WhenTextHasparenthesis()
        {
            //Arrange
            Lexer _lexer = new Lexer("(5+8)*6");
            List<Token> tokens = new List<Token>();

            //Act
            Token currentToken;
            do
            {
                currentToken = _lexer.NextToken();
                tokens.Add(currentToken);
            }
            while (currentToken.Kind != TokenKind.EndOfText);

            //Assert
            Assert.IsTrue(tokens.Count() == 8);
            Assert.IsTrue(_lexer.Errors.Count() == 0);

            Assert.IsTrue(tokens[0].Kind == TokenKind.OpenParenthesis);
            Assert.IsTrue(tokens[1].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[2].Kind == TokenKind.PlusOperator);
            Assert.IsTrue(tokens[3].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[4].Kind == TokenKind.CloseParenthesis);
            Assert.IsTrue(tokens[5].Kind == TokenKind.StarOperator);
            Assert.IsTrue(tokens[6].Kind == TokenKind.Number);
            Assert.IsTrue(tokens[7].Kind == TokenKind.EndOfText);

            Assert.IsTrue(tokens[0].Value.ToString() == "(");
            Assert.IsTrue(tokens[1].Value.ToString() == "5");
            Assert.IsTrue(tokens[2].Value.ToString() == "+");
            Assert.IsTrue(tokens[3].Value.ToString() == "8");
            Assert.IsTrue(tokens[4].Value.ToString() == ")");
            Assert.IsTrue(tokens[5].Value.ToString() == "*");
            Assert.IsTrue(tokens[6].Value.ToString() == "6");
            Assert.IsNull(tokens[7].Value);
        }
    }
}
