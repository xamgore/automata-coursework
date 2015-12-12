using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace lib {
    public class Automaton {
        public const char EPS = '_';
        public const string STR = "S";

        private readonly Queue<State> que;
        public Dictionary<string, List<Rule>> Rules;
        public List<Final> Final;

        public Automaton(IEnumerable<Final> final, IEnumerable<Rule> list) {
            Final = new List<Final>(final);
            Rules = list.GroupBy(rule => rule.From)
                .ToDictionary(group => group.Key, group => group.ToList());
            que = new Queue<State>(new[] { new State(STR, 0) });

            Rules.ForEach(pair => Debug.WriteLine(string.Join(", ", pair.Value)));
        }

        public void Run(string word) {
            while (que.Count > 0) {
                var curState = que.Dequeue();

                IsFinal(curState);

                WithNextOf(curState, nextStates => nextStates
                    .Where(rule => curState.Position < word.Length)
                    .Where(rule => rule.Symbol == EPS || rule.Symbol == word[curState.Position])
                    .Middleware(rule => rule.Display(curState))
                    .Select(rule => rule.NextState(curState))
                    .ForEach(state => que.Enqueue(state)));
            }
        }

        void IsFinal(State curState) {
            Final.FindAll(listOf => listOf.States.Contains(curState.Name))
                .ForEach(state => state.Action(curState));
        }

        void WithNextOf(State state, Action<List<Rule>> @do) {
            if (Rules.ContainsKey(state.Name))
                @do(Rules[state.Name]);
        }
    }

    public class Rule {
        public string From;
        public string To;
        public char Symbol;

        public Rule(string @from, char symbol, string to) {
            Symbol = symbol;
            From = @from;
            To = to;
        }

        public Rule(char @from, char symbol, string to) : this(@from.ToString(), symbol, to) { }
        public Rule(char @from, char symbol, char to) : this(@from.ToString(), symbol, to.ToString()) { }
        public Rule(string @from, char symbol, char to) : this(@from, symbol, to.ToString()) { }

        public State NextState(State curState) {
            return new State(To, curState.Position + (Symbol.Equals(Automaton.EPS) ? 0 : 1));
        }

        public void Display(State curState) {
            var next = NextState(curState);
            Debug.WriteLine($"{curState.Name} \t-> {To}\t\"{Symbol}\" | {next.Position}");
        }

        public override string ToString() => $"{From} {Symbol}> {To}";
    }

    public class State {
        public string Name;
        public int Position;

        public State(string name, int position) {
            Name = name;
            Position = position;
        }

        public override string ToString() => $"{Name} \t:{Position}";
    }

    public class Final {
        public List<string> States;
        public Action<State> Action;

        public Final(Action<State> action, params string[] states) {
            States = states.ToList();
            Action = action;
        }
    }

    public static class ForeachOperation {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) {
            foreach (T item in enumeration) action(item);
        }

        public static IEnumerable<T> Middleware<T>(this IEnumerable<T> enumeration, Action<T> action) {
            foreach (T item in enumeration) {
                action(item); yield return item;
            }
        }
    }
}
