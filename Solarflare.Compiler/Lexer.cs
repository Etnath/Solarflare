using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Solarflare.Compiler
{
    public class Lexer
    {
        private readonly string _errorPrefix = "LEXER ERROR - ";

        private int _position = 0;
        private readonly string _text;
        private List<string> _errors;

        public IEnumerable<string> Errors { get { return _errors; } }

        public Lexer(string statement)
        {
            _text = statement;
            _errors = new List<string>();
        }

        public Token NextToken()
        {
            if (_position >= _text.Length) 
            {
                return new Token(TokenKind.EndOfText, null);
            }

            if (char.IsWhiteSpace(_text[_position]))
            {
                _position++;
                return new Token(TokenKind.Whitespace, " ");
            }

            else if (char.IsDigit(_text[_position]))
            {
                int startPosition = _position;
                _position++;

                while (_position < _text.Length
                        && char.IsDigit(_text[_position]))
                {
                    _position++;
                }
                string value = _text.Substring(startPosition, _position - startPosition);

                if (!int.TryParse(value, out var intValue))
                {
                    _errors.Add($"{_errorPrefix}{value} cannot be represented as an Int32");
                }
                return new Token(TokenKind.Number, intValue);
            }
            else if (_text[_position] == '+')
            {
                _position++;
                return new Token(TokenKind.PlusOperator, _text[_position - 1].ToString());
            }
            else if (_text[_position] == '-')
            {
                _position++;
                return new Token(TokenKind.MinusOperator, _text[_position - 1].ToString());
            }
            else if (_text[_position] == '*')
            {
                _position++;
                return new Token(TokenKind.StarOperator, _text[_position - 1].ToString());
            }
            else if (_text[_position] == '/')
            {
                _position++;
                return new Token(TokenKind.SlashOperator, _text[_position - 1].ToString());
            }
            else if (_text[_position] == '(')
            {
                _position++;
                return new Token(TokenKind.OpenParenthesis, "(");
            }
            else if (_text[_position] == ')')
            {
                _position++;
                return new Token(TokenKind.CloseParenthesis, ")");
            }

            _errors.Add($"{_errorPrefix}Bad character: '{_text[_position]}'");
            _position++;
            return new Token(TokenKind.BadToken, null);
        }

    }

    public class Token
    {
        public TokenKind Kind { get; }
        public object Value { get; }

        public Token(TokenKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }
    }

    public enum TokenKind
    {
        Unknown,
        Number,
        PlusOperator,
        MinusOperator,
        SlashOperator,
        StarOperator,
        Whitespace,
        OpenParenthesis,
        CloseParenthesis,
        BadToken,
        EndOfText,
    }
}
