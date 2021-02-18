using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates
{
    // muss exakt übereinstimmen
    public delegate void KeinParameterKeineRueckgabe(); // macht das gleiche wie das mitgelieferte  Action
    public delegate void EinParameterKeineRueckgabe(string para); //macht das gleiche wie das mitgelieferte  Action<string>
    public delegate byte ZweiParameterEineRueckgabe(string paraA, bool paraB); //macht das gleiche wie das mitgelieferte  Func<string, bool,byte>


    class Logic
    {

        public KeinParameterKeineRueckgabe LogicWillBescheidGeben;
        public EinParameterKeineRueckgabe LogicWillTextWeitergeben;

        public void AufrufVomUI()
        {
            Console.WriteLine("LG> UI hat gerde in der Logic eine Funktion gestartet");

            if (LogicWillBescheidGeben != null && LogicWillTextWeitergeben != null)
            {
                Console.WriteLine("LG> Starte alle eingehängten Methoden ohne parameter und ohne rückgabe");
                LogicWillBescheidGeben?.Invoke();

                Console.WriteLine("LG> Starte alle eingehängten Methoden mit string parameter und ohne rückgabe");
                LogicWillTextWeitergeben("Wichtige Nachricht der Logic");
            }
        }

        public (int ,double) FunktionMitZweiRueckgaben()
        {
            int a = 1;
            double b = 0.3;
            return (a,b);
        }

        public void FunktionMitOutParameter(string Wort, out int Laenge)
        {
            Laenge = Wort.Length;
        }

    }

 

}
