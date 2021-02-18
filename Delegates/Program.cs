using System;

namespace Delegates
{
    class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Microsoft hat gepennt")]
        static void Main()
        {
            Console.WriteLine("UI> Delegaten-Testprogramm");

            Logic l = new();
            l.LogicWillBescheidGeben = wennLogicWasZuSagenHat;
            l.LogicWillTextWeitergeben = ausgabeWichtigerNachricht;

            Console.WriteLine("UI> Starte eine Methode in der Logic");
            l.AufrufVomUI();

            Console.WriteLine("UI> Sind wieder in der Main und der erste Aufruf ist fertig");

            l.LogicWillBescheidGeben += ausgabeWennLogicWasSagt;
            l.AufrufVomUI();

            Console.WriteLine("UI> Delegates und Lambdas");

            l.LogicWillTextWeitergeben += (m) =>Console.WriteLine(m);


            Console.WriteLine("UI> Spass mit Touples");

            int gamma = 1;
            
            (gamma, _ )= l.FunktionMitZweiRueckgaben();

            int meineZahl;
            l.FunktionMitOutParameter("Hallo Welt", out meineZahl);
            Console.WriteLine($"UI> Der Satz Hallo Welt hat {meineZahl} Zeichen");

            Console.ReadLine();// damit das fenster offen bleibt
        }

        static void wennLogicWasZuSagenHat()
        {
            Console.WriteLine("UI> Logic hat eine Methode wennLogicWasZuSagenHat im UI gestartet");
        }
        static void ausgabeWennLogicWasSagt()
        {
            Console.WriteLine("UI> Logic hat eine Methode ausgabeWennLogicWasSagt im UI gestartet");
        }

        static void ausgabeWichtigerNachricht(string Nachricht)
        {
            Console.WriteLine("UI> Die nachricht von der Logic lautet: " + Nachricht);
        }
    }
}
