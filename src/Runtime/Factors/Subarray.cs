// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;

namespace Microsoft.ML.Probabilistic.Factors
{
    using System;
    using System.Collections.Generic;
    using Distributions;
    using Math;
    using Attributes;
    using Utilities;

    /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/doc/*'/>
    /// <typeparam name="T">The type of an array item.</typeparam>
    [FactorMethod(typeof(Factor), "Subarray<>")]
    [Quality(QualityBand.Mature)]
    public static class SubarrayOp<T>
    {
        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogAverageFactor(IList{T}, IList{T}, IList{int})"]/*'/>
        public static double LogAverageFactor(IList<T> items, IList<T> array, IList<int> indices)
        {
            return GetItemsOp<T>.LogAverageFactor(items, array, indices);
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogEvidenceRatio(IList{T}, IList{T}, IList{int})"]/*'/>
        public static double LogEvidenceRatio(IList<T> items, IList<T> array, IList<int> indices)
        {
            return LogAverageFactor(items, array, indices);
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogAverageFactor{DistributionType}(IList{T}, IList{DistributionType}, IList{int})"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        public static double LogAverageFactor<DistributionType>(IList<T> items, IList<DistributionType> array, IList<int> indices)
            where DistributionType : CanGetLogProb<T>
        {
            double z = 0.0;
            for (int i = 0; i < indices.Count; i++)
            {
                z += array[indices[i]].GetLogProb(items[i]);
            }
            return z;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogAverageFactor{DistributionType}(IList{DistributionType}, IList{DistributionType}, IList{int})"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        public static double LogAverageFactor<DistributionType>(IList<DistributionType> items, IList<DistributionType> array, IList<int> indices)
            where DistributionType : CanGetLogAverageOf<DistributionType>
        {
            double z = 0.0;
            for (int i = 0; i < indices.Count; i++)
            {
                z += array[indices[i]].GetLogAverageOf(items[i]);
            }
            return z;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogAverageFactor{DistributionType}(IList{DistributionType}, IList{T}, IList{int})"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        public static double LogAverageFactor<DistributionType>(IList<DistributionType> items, IList<T> array, IList<int> indices)
            where DistributionType : CanGetLogProb<T>
        {
            double z = 0.0;
            for (int i = 0; i < indices.Count; i++)
            {
                z += items[i].GetLogProb(array[indices[i]]);
            }
            return z;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogEvidenceRatio{DistributionType}(IList{DistributionType})"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        [Skip]
        public static double LogEvidenceRatio<DistributionType>(IList<DistributionType> items) where DistributionType : CanGetLogAverageOf<DistributionType>
        {
            return 0.0;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="LogEvidenceRatio{DistributionType}(IList{T}, IList{DistributionType}, IList{int})"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        public static double LogEvidenceRatio<DistributionType>(IList<T> items, IList<DistributionType> array, IList<int> indices)
            where DistributionType : CanGetLogProb<T>
        {
            return LogAverageFactor(items, array, indices);
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageConditional{DistributionType, ResultType}(IList{DistributionType}, IList{int}, ResultType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ResultType">The type of the outgoing message.</typeparam>
        public static ResultType ItemsAverageConditional<DistributionType, ResultType>([SkipIfAllUniform] IList<DistributionType> array, IList<int> indices, ResultType result)
            where ResultType : IList<DistributionType>
            where DistributionType : SettableTo<DistributionType>
        {
            var genericType = typeof(ResultType).MakeGenericType(typeof(IList<DistributionType>));

            Assert.IsTrue(result.Count == indices.Count, "result.Count != indices.Count");
            for (int i = 0; i < indices.Count; i++)
            {
                DistributionType value = result[i];
                value.SetTo(array[indices[i]]);
                result[i] = value;
            }
            return result;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageConditionalInit{TDist}(DistributionStructArray{TDist, T}, IList{int})"]/*'/>
        /// <typeparam name="TDist">The type of a distribution over an array item.</typeparam>
        [Skip]
        public static DistributionStructArray<TDist, T> ItemsAverageConditionalInit<TDist>(
            [IgnoreDependency] DistributionStructArray<TDist, T> array, IList<int> indices)
            where TDist : struct,
                SettableToProduct<TDist>,
                SettableToRatio<TDist>,
                SettableToPower<TDist>,
                SettableToWeightedSum<TDist>,
                CanGetLogAverageOf<TDist>,
                CanGetLogAverageOfPower<TDist>,
                CanGetAverageLog<TDist>,
                IDistribution<T>,
                Sampleable<T>
        {
            return new DistributionStructArray<TDist, T>(indices.Count, i => (TDist)array[indices[i]].Clone());
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageConditionalInit{TDist}(DistributionRefArray{TDist, T}, IList{int})"]/*'/>
        /// <typeparam name="TDist">The type of a distribution over an array item.</typeparam>
        [Skip]
        public static DistributionRefArray<TDist, T> ItemsAverageConditionalInit<TDist>([IgnoreDependency] DistributionRefArray<TDist, T> array, IList<int> indices)
            where TDist : class,
                SettableTo<TDist>,
                SettableToProduct<TDist>,
                SettableToRatio<TDist>,
                SettableToPower<TDist>,
                SettableToWeightedSum<TDist>,
                CanGetLogAverageOf<TDist>,
                CanGetLogAverageOfPower<TDist>,
                CanGetAverageLog<TDist>,
                IDistribution<T>,
                Sampleable<T>
        {
            return new DistributionRefArray<TDist, T>(indices.Count, i => (TDist)array[indices[i]].Clone());
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ArrayAverageConditional{DistributionType, ArrayType}(IList{DistributionType}, IList{int}, ArrayType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ArrayType">The type of the outgoing message.</typeparam>
        public static ArrayType ArrayAverageConditional<DistributionType, ArrayType>([SkipIfAllUniform] IList<DistributionType> items, IList<int> indices, ArrayType result)
            where ArrayType : IList<DistributionType>, SettableToUniform
            where DistributionType : SettableTo<DistributionType>
        {
            if (result != null && !(result is IList))
            {
                throw new InvalidOperationException($"result is {result.GetType()}, but expecting: {typeof(ArrayType)}");
            }

            Assert.IsTrue(items.Count == indices.Count, "items.Count != indices.Count");
            result.SetToUniform();
            for (int i = 0; i < indices.Count; i++)
            {
                DistributionType value = result[indices[i]];
                value.SetTo(items[i]);
                result[indices[i]] = value;
            }
            return result;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ArrayAverageConditional{DistributionType, ArrayType}(IList{T}, IList{int}, ArrayType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ArrayType">The type of the outgoing message.</typeparam>
        public static ArrayType ArrayAverageConditional<DistributionType, ArrayType>(IList<T> items, IList<int> indices, ArrayType result)
            where ArrayType : IList<DistributionType>, SettableToUniform
            where DistributionType : HasPoint<T>
        {
            if (items.Count != indices.Count)
                throw new ArgumentException(indices.Count + " indices were given to Subarray but the output array has length " + items.Count);
            result.SetToUniform();
            for (int i = 0; i < indices.Count; i++)
            {
                DistributionType value = result[indices[i]];
                value.Point = items[i];
                result[indices[i]] = value;
            }
            return result;
        }

        //-- VMP -------------------------------------------------------------------------------------------------------------

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="AverageLogFactor()"]/*'/>
        [Skip]
        public static double AverageLogFactor()
        {
            return 0.0;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageLogarithmInit{TDist}(DistributionStructArray{TDist, T}, IList{int})"]/*'/>
        /// <typeparam name="TDist">The type of a distribution over an array item.</typeparam>
        [Skip]
        public static DistributionStructArray<TDist, T> ItemsAverageLogarithmInit<TDist>([IgnoreDependency] DistributionStructArray<TDist, T> array, IList<int> indices)
            where TDist : struct,
                SettableToProduct<TDist>,
                SettableToRatio<TDist>,
                SettableToPower<TDist>,
                SettableToWeightedSum<TDist>,
                CanGetLogAverageOf<TDist>,
                CanGetLogAverageOfPower<TDist>,
                CanGetAverageLog<TDist>,
                IDistribution<T>,
                Sampleable<T>
        {
            return new DistributionStructArray<TDist, T>(indices.Count, i => (TDist)array[indices[i]].Clone());
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageLogarithmInit{TDist}(DistributionRefArray{TDist, T}, IList{int})"]/*'/>
        /// <typeparam name="TDist">The type of a distribution over an array item.</typeparam>
        [Skip]
        public static DistributionRefArray<TDist, T> ItemsAverageLogarithmInit<TDist>([IgnoreDependency] DistributionRefArray<TDist, T> array, IList<int> indices)
            where TDist : class,
                SettableTo<TDist>,
                SettableToProduct<TDist>,
                SettableToRatio<TDist>,
                SettableToPower<TDist>,
                SettableToWeightedSum<TDist>,
                CanGetLogAverageOf<TDist>,
                CanGetLogAverageOfPower<TDist>,
                CanGetAverageLog<TDist>,
                IDistribution<T>,
                Sampleable<T>
        {
            return new DistributionRefArray<TDist, T>(indices.Count, i => (TDist)array[indices[i]].Clone());
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ItemsAverageLogarithm{DistributionType, ResultType}(IList{DistributionType}, IList{int}, ResultType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ResultType">The type of the outgoing message.</typeparam>
        public static ResultType ItemsAverageLogarithm<DistributionType, ResultType>([SkipIfAllUniform] IList<DistributionType> array, IList<int> indices, ResultType result)
            where ResultType : IList<DistributionType>
            where DistributionType : SettableTo<DistributionType>
        {
            return ItemsAverageConditional<DistributionType, ResultType>(array, indices, result);
        }

        [Skip]
        public static ResultType ItemsDeriv<ResultType>(ResultType result)
            where ResultType : SettableToUniform
        {
            result.SetToUniform();
            return result;
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ArrayAverageLogarithm{DistributionType, ArrayType}(IList{DistributionType}, IList{int}, ArrayType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ArrayType">The type of the outgoing message.</typeparam>
        public static ArrayType ArrayAverageLogarithm<DistributionType, ArrayType>([SkipIfAllUniform] IList<DistributionType> items, IList<int> indices, ArrayType result)
            where ArrayType : IList<DistributionType>, SettableToUniform
            where DistributionType : SettableTo<DistributionType>
        {
            return ArrayAverageConditional<DistributionType, ArrayType>(items, indices, result);
        }

        /// <include file='FactorDocs.xml' path='factor_docs/message_op_class[@name="SubarrayOp{T}"]/message_doc[@name="ArrayAverageLogarithm{DistributionType, ArrayType}(IList{T}, IList{int}, ArrayType)"]/*'/>
        /// <typeparam name="DistributionType">The type of a distribution over an array item.</typeparam>
        /// <typeparam name="ArrayType">The type of the outgoing message.</typeparam>
        public static ArrayType ArrayAverageLogarithm<DistributionType, ArrayType>(IList<T> items, IList<int> indices, ArrayType result)
            where ArrayType : IList<DistributionType>, SettableToUniform
            where DistributionType : HasPoint<T>
        {
            return ArrayAverageConditional<DistributionType, ArrayType>(items, indices, result);
        }
    }
}
