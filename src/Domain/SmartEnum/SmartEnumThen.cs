namespace Domain.SmartEnum
{
    using System;

    public readonly struct SmartEnumThen<TEnum, TValue>
        where TEnum : SmartEnum<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly bool _isMatch;
        private readonly SmartEnum<TEnum, TValue> _smartEnum;
        private readonly bool _stopEvaluating;

        internal SmartEnumThen(bool isMatch, bool stopEvaluating, SmartEnum<TEnum, TValue> smartEnum)
        {
            this._isMatch = isMatch;
            this._smartEnum = smartEnum;
            this._stopEvaluating = stopEvaluating;
        }

        /// <summary>
        /// Calls <paramref name="doThis"/> Action when the preceding When call matches.
        /// </summary>
        /// <param name="doThis">Action method to call.</param>
        /// <returns>A chainable instance of CaseWhen for more when calls.</returns>
        public SmartEnumWhen<TEnum, TValue> Then(Action doThis)
        {
            if (!_stopEvaluating && _isMatch)
                doThis();

            return new SmartEnumWhen<TEnum, TValue>(_stopEvaluating || _isMatch, _smartEnum);
        }
    }
}