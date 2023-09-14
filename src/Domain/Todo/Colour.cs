namespace Exemplum.Domain.Todo
{
    using System;
    using System.Linq;

    public record Colour 
    {
        public string Code { get; }

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
            return code is ['#', _, _, _, _, _, _]
                   && code.All(x => Char.IsLetterOrDigit(x) || x == '#');
        }

        public static Colour Blue => new("#6666FF");
        
        public static implicit operator string(Colour colour)
        {
            return colour.Code ?? throw new InvalidOperationException();
        }

        public static explicit operator Colour(string code)
        {
            return From(code);
        }
    }
}