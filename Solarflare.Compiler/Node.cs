﻿namespace Solarflare.Compiler
{
    /// <summary>
    /// Node of the syntax tree
    /// </summary>
    public class Node
    {
        public Token Token  { get; set; }
        public Node(Token token)
        {
            Token = token;
        }

        public virtual IEnumerable<Node> GetChildren()
        {
            yield return this;
        }
    }
    public class ParenthesisNode : Node
    {
        public Node Expression { get; set; }
        public ParenthesisNode(Node expression) : base(expression.Token)
        {
            Expression = expression;
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Expression;
        }
    }

    public class BinaryNode : Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }

        public BinaryNode(Token token, Node left, Node right) : base(token)
        {
            Left = left;
            Right = right;
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Left;
            yield return new Node(Token);
            yield return Right;
        }
    }

    public class UnaryNode : Node
    {
        public Node Child { get; set; }
        
        public UnaryNode(Token token, Node child) : base(token)
        {
            Child = child;
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return new Node(Token);
            yield return Child;
        }
    }
}
