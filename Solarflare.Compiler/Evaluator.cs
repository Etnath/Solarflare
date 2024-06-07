using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solarflare.Compiler
{
    /// <summary>
    /// Evaluate an expression tree
    /// </summary>
    public class Evaluator
    {
        public Evaluator()
        {
                
        }

        public int Evaluate(Node rootNode)
        {
            if (rootNode.Token.Kind == TokenKind.Number)
                return (int)rootNode.Token.Value;

            if (rootNode is BinaryNode b)
            {
                var left = Evaluate(b.Left);
                var right = Evaluate(b.Right);

                if (b.Token.Kind == TokenKind.PlusOperator)
                    return left + right;
                else if (b.Token.Kind == TokenKind.MinusOperator)
                    return left - right;
                else if (b.Token.Kind == TokenKind.StarOperator)
                    return left * right;
                else if (b.Token.Kind == TokenKind.SlashOperator)
                    return left / right;
                else throw new InvalidOperationException($"Invalid operator {b.Token.Kind}");
            }
            else if (rootNode is ParenthesisNode p)
            {
                return Evaluate(p.Expression);
            }

            throw new InvalidOperationException($"Invalid node {rootNode.Token.Kind}");
        }
    }
}
