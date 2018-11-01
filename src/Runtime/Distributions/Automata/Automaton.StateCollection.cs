// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.ML.Probabilistic.Distributions.Automata
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ML.Probabilistic.Distributions;
    using Microsoft.ML.Probabilistic.Math;

    /// <content>
    /// Contains the class used to represent a state of an automaton.
    /// </content>
    public abstract partial class Automaton<TSequence, TElement, TElementDistribution, TSequenceManipulator, TThis>
        where TSequence : class, IEnumerable<TElement>
        where TElementDistribution : class, IDistribution<TElement>, SettableToProduct<TElementDistribution>, SettableToWeightedSumExact<TElementDistribution>, CanGetLogAverageOf<TElementDistribution>, SettableToPartialUniform<TElementDistribution>, new()
        where TSequenceManipulator : ISequenceManipulator<TSequence, TElement>, new()
        where TThis : Automaton<TSequence, TElement, TElementDistribution, TSequenceManipulator, TThis>, new()
    {
        public struct StateCollection : IEnumerable<State>
        {
            private readonly Automaton<TSequence, TElement, TElementDistribution, TSequenceManipulator, TThis> owner;

            private readonly List<StateData> statesData;

            internal StateCollection(Automaton<TSequence, TElement, TElementDistribution, TSequenceManipulator, TThis> owner, List<StateData> states)
            {
                this.owner = owner;
                this.statesData = owner.statesData;
            }

            public State this[int index] => new State(this.owner, index, this.statesData[index]);

            public int Count => this.statesData.Count;

            public IEnumerator<State> GetEnumerator()
            {
                var this_ = this;
                return this.statesData.Select((data, index) => new State(this_.owner, index, data)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
