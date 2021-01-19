using System;
using System.ComponentModel;

namespace TaschenrechnerLogic
{
    public class TaschenrechnerViewModel : INotifyPropertyChanged // das INotifyPropertyChanged-Interface wird benötigt damit wir dem GUI sagen können wann sich werte ändern
    {
        public int OperatorA
        {
            get { return opA; }
            set
            {
                if (opA != value)
                {
                    opA = value;
                    // da sich durch das verändern von einem Operator auch das ergebnis ändert geben wir dem GUI
                    // die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperatorA)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }
        private int opA;


        public int OperatorB
        {
            get { return opB; }
            set
            {

                if (opB != value)
                {
                    opB = value;
                    // da sich durch das verändern von einem Operator auch das ergebnis ändert geben wir dem GUI
                    // die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperatorB)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }
        private int opB;


        public int Result
        {
            // beim lesen des Ergebnisses wird erst berechnet. dadurch brauchen wir keinen setter und auch keine private variable
            get
            {
                return op switch
                {
                    Operations.Addition => opA + opB,
                    Operations.Subtraction => opA - opB,
                    Operations.Multiplication => opA * opB,
                    Operations.Division => opB == 0 ? 0 : opA / opB,
                    Operations.Modulo => opB == 0 ? 0 : opA % opB,
                    _ => 0,
                };
            }
        }


        public Operations Operator
        {
            get { return op; }
            set
            {
                if (op != value)
                {
                    op = value;
                    // da sich durch das verändern von der Mathematischen Operation auch das ergebnis ändert geben wir
                    // dem GUI die info das auch all Bindungen auf das Property Result neu gelesen werden sollen
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Operator)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }
        private Operations op;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
