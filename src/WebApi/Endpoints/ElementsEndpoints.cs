namespace Exemplum.WebApi.Endpoints
{
    public class ElementsEndpoints : IEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapGet("api/elements", GetElements)
                .WithTags("Chemical Elements");
        }

        private static Task<List<Element>> GetElements()
        {
            return Task.FromResult(Elements);
        }
        
        private static List<Element>? _elements;
        
        private static List<Element> Elements
        {
            get
            {
                if(_elements == null)
                {
                    _elements = new List<Element>();
                    _elements.Add(new Element { Group = "Other", Position = 0, Name = "Hydrogen", Number = 1, Small = "H", Molar = 1.00794 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Helium", Number = 2, Small = "He", Molar = 4.002602 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Lithium", Number = 3, Small = "Li", Molar = 6.941 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Beryllium", Number = 4, Small = "Be", Molar = 9.012182 });
                    _elements.Add(new Element { Group = "Metalloid Boron (p)", Position = 12, Name = "Boron", Number = 5, Small = "B", Molar = 10.811 });
                    _elements.Add(new Element { Group = "Nonmetal Carbon (p)", Position = 13, Name = "Carbon", Number = 6, Small = "C", Molar = 12.0107 });
                    _elements.Add(new Element { Group = "Nonmetal Pnictogen (p)", Position = 14, Name = "Nitrogen", Number = 7, Small = "N", Molar = 14.0067 });
                    _elements.Add(new Element { Group = "Nonmetal Chalcogen (p)", Position = 15, Name = "Oxygen", Number = 8, Small = "O", Molar = 15.9994 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Fluorine", Number = 9, Small = "F", Molar = 18.998404 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Neon", Number = 10, Small = "Ne", Molar = 20.1797 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Sodium", Number = 11, Small = "Na", Molar = 22.989769 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Magnesium", Number = 12, Small = "Mg", Molar = 24.305 });
                    _elements.Add(new Element { Group = "Poor Boron (p)", Position = 12, Name = "Aluminium", Number = 13, Small = "Al", Molar = 26.981539 });
                    _elements.Add(new Element { Group = "Metalloid Carbon (p)", Position = 13, Name = "Silicon", Number = 14, Small = "Si", Molar = 28.0855 });
                    _elements.Add(new Element { Group = "Nonmetal Pnictogen (p)", Position = 14, Name = "Phosphorus", Number = 15, Small = "P", Molar = 30.973763 });
                    _elements.Add(new Element { Group = "Nonmetal Chalcogen (p)", Position = 15, Name = "Sulfur", Number = 16, Small = "S", Molar = 32.065 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Chlorine", Number = 17, Small = "Cl", Molar = 35.453 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Argon", Number = 18, Small = "Ar", Molar = 39.948 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Potassium", Number = 19, Small = "K", Molar = 39.0983 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Calcium", Number = 20, Small = "Ca", Molar = 40.078 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 2, Name = "Scandium", Number = 21, Small = "Sc", Molar = 44.955914 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 3, Name = "Titanium", Number = 22, Small = "Ti", Molar = 47.867 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 4, Name = "Vanadium", Number = 23, Small = "V", Molar = 50.9415 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 5, Name = "Chromium", Number = 24, Small = "Cr", Molar = 51.9961 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 6, Name = "Manganese", Number = 25, Small = "Mn", Molar = 54.938046 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 7, Name = "Iron", Number = 26, Small = "Fe", Molar = 55.845 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 8, Name = "Cobalt", Number = 27, Small = "Co", Molar = 58.933193 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 9, Name = "Nickel", Number = 28, Small = "Ni", Molar = 58.6934 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 10, Name = "Copper", Number = 29, Small = "Cu", Molar = 63.546 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 11, Name = "Zinc", Number = 30, Small = "Zn", Molar = 65.38 });
                    _elements.Add(new Element { Group = "Poor Boron (p)", Position = 12, Name = "Gallium", Number = 31, Small = "Ga", Molar = 69.723 });
                    _elements.Add(new Element { Group = "Metalloid Carbon (p)", Position = 13, Name = "Germanium", Number = 32, Small = "Ge", Molar = 72.63 });
                    _elements.Add(new Element { Group = "Metalloid Pnictogen (p)", Position = 14, Name = "Arsenic", Number = 33, Small = "As", Molar = 74.9216 });
                    _elements.Add(new Element { Group = "Nonmetal Chalcogen (p)", Position = 15, Name = "Selenium", Number = 34, Small = "Se", Molar = 78.96 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Bromine", Number = 35, Small = "Br", Molar = 79.904 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Krypton", Number = 36, Small = "Kr", Molar = 83.798 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Rubidium", Number = 37, Small = "Rb", Molar = 85.4678 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Strontium", Number = 38, Small = "Sr", Molar = 87.62 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 2, Name = "Yttrium", Number = 39, Small = "Y", Molar = 88.90585 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 3, Name = "Zirconium", Number = 40, Small = "Zr", Molar = 91.224 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 4, Name = "Niobium", Number = 41, Small = "Nb", Molar = 92.90638 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 5, Name = "Molybdenum", Number = 42, Small = "Mo", Molar = 95.96 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 6, Name = "Technetium", Number = 43, Small = "Tc", Molar = 98 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 7, Name = "Ruthenium", Number = 44, Small = "Ru", Molar = 101.07 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 8, Name = "Rhodium", Number = 45, Small = "Rh", Molar = 102.9055 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 9, Name = "Palladium", Number = 46, Small = "Pd", Molar = 106.42 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 10, Name = "Silver", Number = 47, Small = "Ag", Molar = 107.8682 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 11, Name = "Cadmium", Number = 48, Small = "Cd", Molar = 112.411 });
                    _elements.Add(new Element { Group = "Poor Boron (p)", Position = 12, Name = "Indium", Number = 49, Small = "In", Molar = 114.818 });
                    _elements.Add(new Element { Group = "Poor Carbon (p)", Position = 13, Name = "Tin", Number = 50, Small = "Sn", Molar = 118.71 });
                    _elements.Add(new Element { Group = "Metalloid Pnictogen (p)", Position = 14, Name = "Antimony", Number = 51, Small = "Sb", Molar = 121.76 });
                    _elements.Add(new Element { Group = "Metalloid Chalcogen (p)", Position = 15, Name = "Tellurium", Number = 52, Small = "Te", Molar = 127.6 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Iodine", Number = 53, Small = "I", Molar = 126.90447 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Xenon", Number = 54, Small = "Xe", Molar = 131.293 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Caesium", Number = 55, Small = "Cs", Molar = 132.90546 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Barium", Number = 56, Small = "Ba", Molar = 137.327 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 3, Name = "Hafnium", Number = 72, Small = "Hf", Molar = 178.49 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 4, Name = "Tantalum", Number = 73, Small = "Ta", Molar = 180.94788 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 5, Name = "Tungsten", Number = 74, Small = "W", Molar = 183.84 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 6, Name = "Rhenium", Number = 75, Small = "Re", Molar = 186.207 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 7, Name = "Osmium", Number = 76, Small = "Os", Molar = 190.23 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 8, Name = "Iridium", Number = 77, Small = "Ir", Molar = 192.217 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 9, Name = "Platinum", Number = 78, Small = "Pt", Molar = 195.084 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 10, Name = "Gold", Number = 79, Small = "Au", Molar = 196.96657 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 11, Name = "Mercury", Number = 80, Small = "Hg", Molar = 200.59 });
                    _elements.Add(new Element { Group = "Poor Boron (p)", Position = 12, Name = "Thallium", Number = 81, Small = "Tl", Molar = 204.3833 });
                    _elements.Add(new Element { Group = "Poor Carbon (p)", Position = 13, Name = "Lead", Number = 82, Small = "Pb", Molar = 207.2 });
                    _elements.Add(new Element { Group = "Poor Pnictogen (p)", Position = 14, Name = "Bismuth", Number = 83, Small = "Bi", Molar = 208.9804 });
                    _elements.Add(new Element { Group = "Metalloid Chalcogen (p)", Position = 15, Name = "Polonium", Number = 84, Small = "Po", Molar = 209 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Astatine", Number = 85, Small = "At", Molar = 210 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Radon", Number = 86, Small = "Rn", Molar = 222 });
                    _elements.Add(new Element { Group = "Alkaline Earth Metal (s)", Position = 0, Name = "Francium", Number = 87, Small = "Fr", Molar = 223 });
                    _elements.Add(new Element { Group = "Alkaline Metal (s)", Position = 1, Name = "Radium", Number = 88, Small = "Ra", Molar = 226 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 3, Name = "Rutherfordium", Number = 104, Small = "Rf", Molar = 267 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 4, Name = "Dubnium", Number = 105, Small = "Db", Molar = 268 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 5, Name = "Seaborgium", Number = 106, Small = "Sg", Molar = 271 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 6, Name = "Bohrium", Number = 107, Small = "Bh", Molar = 272 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 7, Name = "Hassium", Number = 108, Small = "Hs", Molar = 270 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 8, Name = "Meitnerium", Number = 109, Small = "Mt", Molar = 276 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 9, Name = "Darmstadtium", Number = 110, Small = "Ds", Molar = 281 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 10, Name = "Roentgenium", Number = 111, Small = "Rg", Molar = 280 });
                    _elements.Add(new Element { Group = "Transition Metal (d)", Position = 11, Name = "Copernicium", Number = 112, Small = "Cn", Molar = 285 });
                    _elements.Add(new Element { Group = "Poor Boron (p)", Position = 12, Name = "Nihonium", Number = 113, Small = "Nh", Molar = 284 });
                    _elements.Add(new Element { Group = "Poor Carbon (p)", Position = 13, Name = "Flerovium", Number = 114, Small = "Fl", Molar = 289 });
                    _elements.Add(new Element { Group = "Poor Pnictogen (p)", Position = 14, Name = "Moscovium", Number = 115, Small = "Mc", Molar = 288 });
                    _elements.Add(new Element { Group = "Poor Chalcogen (p)", Position = 15, Name = "Livermorium", Number = 116, Small = "Lv", Molar = 293 });
                    _elements.Add(new Element { Group = "Halogen (p)", Position = 16, Name = "Tennessine", Number = 117, Small = "Ts", Molar = 294 });
                    _elements.Add(new Element { Group = "Noble Gas (p)", Position = 17, Name = "Oganesson", Number = 118, Small = "Og", Molar = 294 });

                }
                return _elements;
            }
        }
    }

    public record Element
    {
        public string Group { get; init; } = string.Empty;
        public int Position { get; init; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; init; }
        public string Small { get; set; } = string.Empty;
        public double Molar { get; set; }
    }
}
