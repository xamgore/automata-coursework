using System.Diagnostics;
using System;
using lib;

namespace automata_task1 {
    static class Program {
        static void Main(string[] args) {
            var word = "0000100000100000101010101";

            var gotEnd = false;
            var machine = new Automaton(
                new[] {
                    new Final(state => gotEnd = word.Length == state.Position, "F", "E")
                }, rules);

            machine.Run(word);
            Console.WriteLine(gotEnd);

            if (Debugger.IsAttached) {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }


        static readonly Rule[] rules = {
            new Rule('S', '0', 'S'),
            new Rule('S', '1', 'A'),
            new Rule('A', '0', 'A'),
            new Rule('A', '1', 'B'),
            new Rule('B', '1', 'F'),
            new Rule('B', '0', 'B'),
            new Rule('F', '0', "K0"),
            new Rule("K0", '1', "K01"),
            new Rule("K01", '0', "K010"),
            new Rule("K010", '1', "E"),
            new Rule('E', '0', "K0"),
        };
    }
}
