namespace Exemplum.Domain.Todo
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Colour : ValueObject
    {
        private Colour(string code)
        {
            Code = code;
        }

        public static Colour From(string code)
        {
            if (!IsValidColour(code))
            {
                throw new UnsupportedColourException(code);
            }

            var colour = new Colour(code);
            return colour;
        }

        public static bool IsValidColour(string code)
        {
            return code.Length == 7 &&
                   code[0] == '#'
                   && code.All(x => Char.IsLetterOrDigit(x) || x == '#');
        }

        public static Colour White => new Colour("#FFFFFF");

        public static Colour Red => new Colour("#FF5733");

        public static Colour Orange => new Colour("#FFC300");

        public static Colour Yellow => new Colour("#FFFF66");

        public static Colour Green => new Colour("#CCFF99");

        public static Colour Blue => new Colour("#6666FF");

        public static Colour Purple => new Colour("#9966CC");

        public static Colour Grey => new Colour("#999999");

        public string Code { get; private set; }

        public static implicit operator string(Colour colour)
        {
            return colour.ToString();
        }

        public static explicit operator Colour(string code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}