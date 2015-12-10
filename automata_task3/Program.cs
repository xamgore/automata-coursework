using System;
using System.Collections.Generic;
using System.Linq;
using static automata_task2.Automata;

namespace automata_task2 {
    static class Program {
        static void Main(string[] args) {
            var machine = new Automata(
                // final states
                new[] {
                    new Final(() => { Console.WriteLine("True"); Environment.Exit(0); }, "C"),
                },

                // rules
                new[] {
                    new Rule("S", 'a', "A"),
                    new Rule("S", EPS, "A"),
                    new Rule("A", 'b', "B"),
                    new Rule("A", 'c', "C"),
                    new Rule("B", 'c', "C"),
                });

            machine.Run("bc");

            Console.WriteLine("False");
        }
    }

    class Automata {
        public const char EPS = '_';
        public Dictionary<string, List<Rule>> Rules;
        public List<Final> Final;

        public Automata(IEnumerable<Final> final, IEnumerable<Rule> list) {
            Final = new List<Final>(final);
            Rules = list.GroupBy(rule => rule.FromState)
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        public void Run(string word) {
            Queue<State> que = new Queue<State>();
            que.Enqueue(new State("S", 0));

            while (que.Count > 0) {
                var curState = que.Dequeue();

                IsFinal(curState);

                WithNextOf(curState, nextStates => nextStates
                    .Where(rule => rule.Symbol == EPS || rule.Symbol == word[curState.Position])
                    .Select(rule => new State(rule.NextState, curState.Position + 1))
                    .ForEach(state => que.Enqueue(state)));
            }
        }

        void IsFinal(State state)
            => Final.FindAll(listOf => listOf.States.Contains(state.Name))
                .ForEach(finalState => finalState.Action());

        void WithNextOf(State state, Action<List<Rule>> @do) {
            if (Rules.ContainsKey(state.Name))
                @do(Rules[state.Name]);
        }
    }

    class Rule {
        public string FromState;
        public string NextState;
        public char Symbol;

        public Rule(string fromState, char symbol, string nextState) {
            FromState = fromState;
            NextState = nextState;
            Symbol = symbol;
        }
    }

    class State {
        public string Name;
        public int Position;

        public State(string name, int position) {
            Name = name;
            Position = position;
        }
    }

    class Final {
        public List<string> States;
        public Action Action;

        public Final(Action action, params string[] states) {
            States = states.ToList();
            Action = action;
        }
    }

    public static class ForeachOperation {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) {
            foreach (T item in enumeration) action(item);
        }
    }
}
