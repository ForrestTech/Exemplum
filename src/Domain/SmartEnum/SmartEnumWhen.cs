namespace Domain.SmartEnum
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct SmartEnumWhen<TEnum, TValue>
        where TEnum : SmartEnum<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private readonly SmartEnum<TEnum, TValue> _smartEnum;
        private readonly bool _stopEvaluating;

        internal SmartEnumWhen(bool stopEvaluating, SmartEnum<TEnum, TValue> smartEnum)
        {
            this._stopEvaluating = stopEvaluating;
            this._smartEnum = smartEnum;
        }

        /// <summary>
        /// Execute this action if no other calls to When have matched.
        /// </summary>
        /// <param name="action">The Action to call.</param>
        public void Default(Action action)
        {
            if (!_stopEvaluating)
            {
                action();
            }
        }

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnumWhen">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public SmartEnumThen<TEnum, TValue> When(SmartEnum<TEnum, TValue> smartEnumWhen) =>
            new SmartEnumThen<TEnum, TValue>(isMatch: _smartEnum.Equals(smartEnumWhen), stopEvaluating: _stopEvaluating, smartEnum: _smartEnum);

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnums">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public SmartEnumThen<TEnum, TValue> When(params SmartEnum<TEnum, TValue>[] smartEnums) =>
            new SmartEnumThen<TEnum, TValue>(isMatch: smartEnums.Contains(_smartEnum), stopEvaluating: _stopEvaluating, smartEnum: _smartEnum);

        /// <summary>
        /// When this instance is one of the specified <see cref="SmartEnum{TEnum, TValue}"/> parameters.
        /// Execute the action in the subsequent call to Then().
        /// </summary>
        /// <param name="smartEnums">A collection of <see cref="SmartEnum{TEnum, TValue}"/> values to compare to this instance.</param>
        /// <returns>A executor object to execute a supplied action.</returns>
        public SmartEnumThen<TEnum, TValue> When(IEnumerable<SmartEnum<TEnum, TValue>> smartEnums) =>
            new SmartEnumThen<TEnum, TValue>(isMatch: smartEnums.Contains(_smartEnum), stopEvaluating: _stopEvaluating, smartEnum: _smartEnum);
    }
}