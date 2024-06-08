using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solarflare.Compiler
{
    /// <summary>
    /// Responsibility of the parser is to transform an array of tokens to a tree representation ready to be evaluated
    /// </summary>
    public class Parser
    {
        private readonly string _errorPrefix = "PARSER ERROR - ";
        private readonly Dictionary<TokenKind, int> _operatorPriorityMapping = new Dictionary<TokenKind, int>() {
            { TokenKind.PlusOperator, 1 },
            { TokenKind.MinusOperator, 1 },
            { TokenKind.SlashOperator, 2 },
            { TokenKind.StarOperator, 2 },
        };

        private readonly Dictionary<TokenKind, int> _unaryOperatorPriorityMapping = new Dictionary<TokenKind, int>() {
            { TokenKind.PlusOperator, 3 },
            { TokenKind.MinusOperator, 3 }
        };

        private Token[] _tokens;
        int _index;
        private Token _currentToken => _tokens[_index];
        private List<string> _errors;

        public IEnumerable<string> Errors { get { return _errors; } }

        public Parser(string statement)
        {
            _index = 0;
            _errors = new List<string>();
            _tokens = InitializeParser(statement);
        }

        public Token[] InitializeParser(string statement)
        {
            var lexer = new Lexer(statement);

            List<Token> tokens = new List<Token>();
            Token currentToken;
            do
            {
                currentToken = lexer.NextToken();

                if (currentToken.Kind != TokenKind.Whitespace)
                {
                    tokens.Add(currentToken);
                }

            }
            while (currentToken.Kind != TokenKind.EndOfText);

            _errors.AddRange(lexer.Errors);

            return tokens.ToArray();
        }
        public Node GenerateTree()
        {
            var root = ParseExpression(0);

            if (!(_currentToken.Kind == TokenKind.EndOfText))
                _errors.Add($"{_errorPrefix}Unexpected token '{_currentToken.Kind}', expected '{TokenKind.EndOfText}'");

            return root;
        }

        private Node ParseTerm()
        {
            //Before we parse for a term, let's see if we have a factor on the left side of the tree first
            var node = ParseFactor();

            while (_currentToken.Kind == TokenKind.PlusOperator
                || _currentToken.Kind == TokenKind.MinusOperator)
            {
                var operatorToken = NextToken();
                //We also need to check if there are factors on the right side of the tree
                var right = ParseFactor();

                node = new BinaryNode(operatorToken, node, right);

            }

            return node;
        }

        private Node ParseFactor()
        {
            //Before we parse for a factor, let's see if we have a number on the left side of the tree first
            var node = ParseExpression();

            while (_currentToken.Kind == TokenKind.StarOperator
                || _currentToken.Kind == TokenKind.SlashOperator)
            {
                var operatorToken = NextToken();
                //We also need to check if there are numbers on the right side of the tree
                var right = ParseExpression();

                node = new BinaryNode(operatorToken, node, right);
            }

            return node;
        }

        public Node ParseExpression()
        {
            //We firt need to check for a parenthesis
            if (_currentToken.Kind == TokenKind.OpenParenthesis)
            {
                //if there is a parenthesis, we treat what is inside as an expression that we need to parse
                var left = NextToken();
                var expression = ParseExpression(0);

                //let's make sure there is a close parenthesis
                if (!(_currentToken.Kind == TokenKind.CloseParenthesis))
                {
                    _errors.Add($"{_errorPrefix}Unexpected token '{_currentToken.Kind}', expected '{TokenKind.CloseParenthesis}'");
                    return new Node(new Token(TokenKind.CloseParenthesis, null));
                }

                var right = NextToken();

                return new ParenthesisNode(expression);
            }
            else if (_currentToken.Kind == TokenKind.Number)
                return new Node(NextToken());

            //If the first element is not a number, lets create one
            _errors.Add($"{_errorPrefix}Unexpected token '{_currentToken.Kind}', expected '{TokenKind.Number}'");
            return new Node(new Token(TokenKind.Number, null));
        }

        /// <summary>
        /// return currentToken and increment position to next token
        /// </summary>
        /// <returns></returns>
        private Token NextToken()
        {
            var currentToken = _tokens[_index];
            _index++;
            return currentToken;
        }

        private Node ParseExpression(int parentPriority)
        {
            Node node;

            var unaryPriority = GetUnaryOperatorPriority(_currentToken.Kind);
            if (unaryPriority > 0 && unaryPriority > parentPriority)
            {
                var unaryToken = NextToken();
                var child = ParseExpression();
                node = new UnaryNode(unaryToken, child);
                
            }
            else
            {
                node = ParseExpression();
            }       

            while (true)
            {
                var priority = GetOperatorPriority(_currentToken.Kind);
                if (priority == 0 || priority < parentPriority)
                    break;

                var operatorToken = NextToken();
                //We need to take priority into account when parsing the right side of the tree
                var right = ParseExpression(priority);
                node = new BinaryNode(operatorToken, node, right);
            }

            return node;
        }

        /// <summary>
        /// Get the priority of an operator
        /// </summary>
        /// <param name="tokenKind">The kind of operator</param>
        /// <returns>The prioirty of a given operator kind, 0 if the token kind is not an operator</returns>
        private int GetOperatorPriority(TokenKind tokenKind)
        {
            if (!_operatorPriorityMapping.ContainsKey(tokenKind))
                return 0;

            return _operatorPriorityMapping[tokenKind];
        }

        /// <summary>
        /// Get the priority of an operator
        /// </summary>
        /// <param name="tokenKind">The kind of operator</param>
        /// <returns>The prioirty of a given operator kind, 0 if the token kind is not an operator</returns>
        private int GetUnaryOperatorPriority(TokenKind tokenKind)
        {
            if (!_unaryOperatorPriorityMapping.ContainsKey(tokenKind))
                return 0;

            return _unaryOperatorPriorityMapping[tokenKind];
        }
    }
}
