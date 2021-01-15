using System.Windows.Input;

namespace Binding
{
    public static class ExampleCommands // statisch Klasse, new nicht erlaubt, wird direkt beim hochfaren Initialisiert
    {
        public static readonly RoutedUICommand RedAlert = new RoutedUICommand(
            "Alarmstufe Rot", //Beschreibung des Kommandos
            "RedAlert",//Name des Kommandos, idealerweise identisch mit Variablennamen
            typeof(ExampleCommands),//KlassenTyp in welcher das Kommando abgelegt ist
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control)} // Sammlung an Gesten und Hotkeys welche das Kommando auslösen sollen
            );

        public static readonly RoutedUICommand OpenWindow = new RoutedUICommand(
            "Fenster zeigen", //Beschreibung des Kommandos
            "OpenWindow",//Name des Kommandos, idealerweise identisch mit Variablennamen
            typeof(ExampleCommands),//KlassenTyp in welcher das Kommando abgelegt ist
            new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control)} // Sammlung an Gesten und Hotkeys welche das Kommando auslösen sollen
            );


    }
}
