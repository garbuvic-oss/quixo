using Quixo.Lib.Enum;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Quixo.Lib.Model
{
    /// <summary>
    /// Represents a board cube with the symbol displayed on its visible face.
    /// </summary>
    [DataContract]
    [KnownType(typeof(FreezedCube))]
    public class Cube : INotifyPropertyChanged
    {
        private Symbol symbol;
        private bool isSelected;

        // Coordinates on the board (0-based)
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the symbol visible on the cube.
        /// </summary>
        [DataMember]
        public Symbol Symbol
        {
            get => symbol;
            set
            {
                if (symbol == value) return;
                symbol = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplaySymbol));
            }
        }

        /// <summary>
        /// Gets or sets whether the cube is selected in the UI.
        /// This is a lightweight UI hint stored on the model for simplicity.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// String suitable for display in the UI for the cube symbol.
        /// </summary>
        public string DisplaySymbol => Symbol == Symbol.NONE ? string.Empty : Symbol.ToString();

        /// <summary>
        /// Indicates whether this cube is frozen. Overridden by FreezedCube.
        /// </summary>
        public virtual bool IsFrozen => false;
        /// <summary>
        /// Initializes a new instance of the Cube class.
        /// </summary>
        /// <param name="symbol">Initial cube symbol.</param>
        public Cube(Symbol symbol)
        {
            this.symbol = symbol;
        }

        /// <summary>
        /// Returns a string representation of the cube.
        /// </summary>
        /// <returns>A string containing the cube symbol.</returns>
        public override string ToString()
        {
            return $"Symbol: {Symbol}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}