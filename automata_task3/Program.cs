using static lib.Automaton;
using System.Diagnostics;
using System.Linq;
using System;
using lib;

namespace automata_task3 {
    static class Program {
        static void Main(string[] args) {
            var word = "bbbbbbbaaaaaababbbbabaaaabaaaaab";

            Console.WriteLine($"ДКА: {Детерминированный(word)}");
            Console.WriteLine($"НКА: {Недетерминированный(word)}");

            if (Debugger.IsAttached) {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static bool Детерминированный(string word) {
            var rules = new[] { -1, -1, 3, 3, 6, 7, 8, 10, 6, 7, 11, 10, 14, 14, -1, -1 }
               .Select((t, i) => new Rule(i.ToString(), 'a', t.ToString()))
               .Concat(new[] { 1, 2, 4, 5, 4, 5, 9, 9, 5, 5, 12, 13, -1, -1, 15, -1 }
               .Select((t, i) => new Rule(i.ToString(), 'b', t.ToString())))
               .Concat(new[] { new Rule(STR, EPS, "0") }).ToList();

            var gotEnd = false;
            try {
                new Automaton(new[] {
                    new Final(state => {
                        if (gotEnd = word.Length == state.Position)
                            throw new Exception();
                    }, "15", "13", "9")
                }, rules).Run(word);
            } catch { /**/ }
            return gotEnd;
        }

        static bool Недетерминированный(string word) {
            var rules = new[] {
                new Rule(STR, 'b', STR),
                new Rule(STR, 'b', '1'),
                new Rule('1', 'b', '2'),
                new Rule('2', 'a', '2'),
                new Rule('2', EPS, '3'),
                new Rule('3', 'a', '4'),
                new Rule('3', 'b', '5'),
                new Rule('3', EPS, '6'),
                new Rule('4', 'b', '5'),
                new Rule('5', EPS, '3'),
                new Rule('6', 'a', '6'),
                new Rule('6', 'b', '7'),
                new Rule('7', 'a', '8'),
                new Rule('8', 'a', '7'),
                new Rule('8', 'b', '9'),
            };

            var gotEnd = false;
            try {
                new Automaton(new[] {
                    new Final(state => {
                        if (gotEnd = word.Length == state.Position)
                            throw new Exception();
                    }, "9")
                }, rules).Run(word);
            } catch { /**/ }
            return gotEnd;
        }
    }
}
