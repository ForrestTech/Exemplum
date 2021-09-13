namespace Domain.Todo
{
    using Common;
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
            var colour = new Colour(code);

            if (!SupportedColours.Contains(colour))
            {
                throw new UnsupportedColourException(code);
            }

            return colour;
        }

        public static bool IsValidColour(string code)
        {
            var colour = new Colour(code);

            return SupportedColours.Contains(colour);
        }

        public static Colour White => new Colour("#FFFFFF");

        public static Colour Red => new Colour("#FF5733");

        public static Colour Orange => new Colour("#FFC300");

        public static Colour Yellow => new Colour("#FFFF66");

        public static Colour Green => new Colour("#CCFF99 ");

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

        private static IEnumerable<Colour> SupportedColours
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}