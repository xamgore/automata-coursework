using static lib.Automaton;
using System;
using System.Diagnostics;
using lib;

namespace automata_task2 {
    static class Program {
        static void Main(string[] args) {
            int count53150 = 0, count53555 = 0, count5510 = 0, count0001 = 0;

            var machine = new Automaton(
                new[] {
                    // final states
                    new Final(_ => count53150++, "A6"),
                    new Final(_ => count53555++, "B6"),
                    new Final(_ => count5510++, "C4"),
                    new Final(_ => count0001++, "D4"),
                }, rules);

            machine.Run("000100010001005355553555315000000005355553555");

            var @out = new[] { $"53150: {count53150}", $"53555: {count53555}", $"5510: {count5510}", $"0001: {count0001}" };
            Console.WriteLine(string.Join("\n", @out));

            if (Debugger.IsAttached) {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }


        static readonly Rule[] rules = {
            // automaton 53150
            new Rule("A0", '0', "A0"),
            new Rule("A0", '1', "A0"),
            new Rule("A0", '2', "A0"),
            new Rule("A0", '3', "A0"),
            new Rule("A0", '4', "A0"),
            new Rule("A0", '5', "A0"),
            new Rule("A0", EPS, "A1"),
            new Rule("A1", '5', "A2"),
            new Rule("A2", '3', "A3"),
            new Rule("A3", '1', "A4"),
            new Rule("A4", '5', "A5"),
            new Rule("A5", '0', "A6"),
            //new Rule("A6", EPS, "A0"),

            // automaton 53555
            new Rule("B0", '0', "B0"),
            new Rule("B0", '1', "B0"),
            new Rule("B0", '2', "B0"),
            new Rule("B0", '3', "B0"),
            new Rule("B0", '4', "B0"),
            new Rule("B0", '5', "B0"),
            new Rule("B0", EPS, "B1"),
            new Rule("B1", '5', "B2"),
            new Rule("B2", '3', "B3"),
            new Rule("B3", '5', "B4"),
            new Rule("B4", '5', "B5"),
            new Rule("B5", '5', "B6"),
            //new Rule("B6", EPS, "B0"),

            // automaton 5510
            new Rule("C0", '0', "C0"),
            new Rule("C0", '1', "C0"),
            new Rule("C0", '2', "C0"),
            new Rule("C0", '3', "C0"),
            new Rule("C0", '4', "C0"),
            new Rule("C0", '5', "C0"),
            new Rule("C0", EPS, "C1"),
            new Rule("C1", '5', "C2"),
            new Rule("C2", '5', "C3"),
            new Rule("C3", '1', "C4"),
            new Rule("C4", '0', "C5"),

            // automaton 0001
            new Rule("D0", '1', "D0"),
            new Rule("D0", '2', "D0"),
            new Rule("D0", '3', "D0"),
            new Rule("D0", '4', "D0"),
            new Rule("D0", '5', "D0"),

            new Rule("D0", '0', "D1"),
            new Rule("D1", '0', "D2"),
            new Rule("D2", '0', "D3"),
            new Rule("D3", '1', "D4"),
            new Rule("D4", '0', "D1"),
            new Rule("D3", '0', "D3"),

            new Rule("D1", '1', "D0"),
            new Rule("D1", '2', "D0"),
            new Rule("D1", '3', "D0"),
            new Rule("D1", '4', "D0"),
            new Rule("D1", '5', "D0"),

            new Rule("D2", '1', "D0"),
            new Rule("D2", '2', "D0"),
            new Rule("D2", '3', "D0"),
            new Rule("D2", '4', "D0"),
            new Rule("D2", '5', "D0"),

            new Rule("D3", '2', "D0"),
            new Rule("D3", '3', "D0"),
            new Rule("D3", '4', "D0"),
            new Rule("D3", '5', "D0"),

            new Rule("D4", '1', "D0"),
            new Rule("D4", '2', "D0"),
            new Rule("D4", '3', "D0"),
            new Rule("D4", '4', "D0"),
            new Rule("D4", '5', "D0"),

            // links to starter states
            new Rule(STR, EPS, "A0"),
            new Rule(STR, EPS, "B0"),
            new Rule(STR, EPS, "C0"),
            new Rule(STR, EPS, "D0"),
        };
    }

}
