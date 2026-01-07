using System.Collections;
using System.Numerics;

namespace LazyCSharp {
	public class LazyType<T>(Func<T> value, bool isConstValue = false)  {


		public Func<T> LazyFunction { get; private set; } = value;
		public bool IsConstValue { get; private set; } = isConstValue;

		/// <summary>
		/// Gets the current value held by the proxy. If no proxy is held it will calculate it.
		/// See <seealso cref="ProxyValue"/> to get <see langword="null"/> if no value was set yet.
		/// </summary>
		public T Value { get {
				if (ProxyWasSet && IsConstValue)
				{
					return ProxyValue!;
				}
				else
				{
					T variable = LazyFunction();
					ProxyValue = variable;
					return variable;
				}
			} }
		/// <summary>
		/// Gets the current saved value, or <see langword="null"/> if no value is set.
		/// See <seealso cref="Value"/> to compute the value when called.
		/// </summary>
		public T? ProxyValue { get => _proxyValue; private set { ProxyWasSet = true; _proxyValue = value; } }
		private T? _proxyValue = default;
		public bool ProxyWasSet { get; private set; } = false;


		static public implicit operator Func<T>(LazyType<T> lazy) => lazy.LazyFunction;
		static public implicit operator LazyType<T>(Func<T> func) => new(func);
		static public implicit operator LazyType<T>(T value) => new(() => value);


		// Arithmetic Operators, Bitwise Operators, Comparison Operators
		// Also sets IsConstValue to true if both operands are constant values.
		/// <summary>
		/// Creates lambda new lazy value that represents the sum of two specified lazy values.
		/// </summary>
		/// <remarks>The addition is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IAdditionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the addition.</typeparam>
		/// <param name="a">The first summand, provided as lambda lazy value.</param>
		/// <param name="b">The second summand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by adding the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Add<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IAdditionOperators<T1, T2, TResult> => new(() => a.Value + b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the difference of two specified lazy values.
		/// </summary>
		/// <remarks>The subtraction is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the minuend. Must implement <see cref="ISubtractionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the subtrahend.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the subtraction.</typeparam>
		/// <param name="a">The minuend, provided as lambda lazy value.</param>
		/// <param name="b">The subtrahend, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by subtracting the value of <paramref name="b"/> from <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Subtract<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : ISubtractionOperators<T1, T2, TResult> => new(() => a.Value - b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the product of two specified lazy values.
		/// </summary>
		/// <remarks>The multiplication is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first factor. Must implement <see cref="IMultiplyOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second factor.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the multiplication.</typeparam>
		/// <param name="a">The first factor, provided as lambda lazy value.</param>
		/// <param name="b">The second factor, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by multiplying the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Multiply<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IMultiplyOperators<T1, T2, TResult> => new(() => a.Value * b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the quotient of two specified lazy values.
		/// </summary>
		/// <remarks>The division is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The dividend type. Must implement <see cref="IDivisionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The divisor type.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the division.</typeparam>
		/// <param name="a">The dividend, provided as lambda lazy value.</param>
		/// <param name="b">The divisor, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by dividing the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Divide<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IDivisionOperators<T1, T2, TResult> => new(() => a.Value / b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the remainder of dividing two specified lazy values.
		/// </summary>
		/// <remarks>The modulus operation is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The dividend type. Must implement <see cref="IModulusOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The divisor type.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the modulus operation.</typeparam>
		/// <param name="a">The dividend, provided as lambda lazy value.</param>
		/// <param name="b">The divisor, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by taking the modulus of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Modulo<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IModulusOperators<T1, T2, TResult> => new(() => a.Value % b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the unary negation of the specified lazy value.
		/// </summary>
		/// <remarks>The negation is performed lazily; the operand is not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IUnaryNegationOperators{T1, TResult}"/> with the lowestUnchecked type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the negation.</typeparam>
		/// <param name="a">The operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying unary negation to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Negate<T1, TResult>(LazyType<T1> a) where T1 : IUnaryNegationOperators<T1, TResult> => new(() => -a.LazyFunction(), a.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the bitwise AND of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise AND is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the bitwise AND.</typeparam>
		/// <param name="a">The first operand, provided as lambda lazy value.</param>
		/// <param name="b">The second operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise AND on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitAnd<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.Value & b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the bitwise OR of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise OR is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the bitwise OR.</typeparam>
		/// <param name="a">The first operand, provided as lambda lazy value.</param>
		/// <param name="b">The second operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise OR on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitOr<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.Value | b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the bitwise XOR of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise XOR is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T2"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the bitwise XOR.</typeparam>
		/// <param name="a">The first operand, provided as lambda lazy value.</param>
		/// <param name="b">The second operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise XOR on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitXor<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.Value ^ b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the bitwise NOT (complement) of the specified lazy value.
		/// </summary>
		/// <remarks>The bitwise complement is performed lazily; the operand is not evaluated until the lowestUnchecked's value is
		/// requested. This method forwards to <see cref="BitComplement{T1, TResult}"/>.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IBitwiseOperators{T1, T1, TResult}"/> with the lowestUnchecked type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the bitwise NOT.</typeparam>
		/// <param name="a">The operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying bitwise NOT to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitNot<T1, TResult>(LazyType<T1> a) where T1 : IBitwiseOperators<T1, T1, TResult> => BitComplement<T1, TResult>(a);

		/// <summary>
		/// Creates lambda new lazy value that represents the right-shift of lambda specified lazy value by another.
		/// </summary>
		/// <remarks>The shift is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The value to shift. Must implement <see cref="IShiftOperators{T1, TShift, TResult}"/> with the other types (<typeparamref name="TShift"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TShift">The shift amount type.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the right shift.</typeparam>
		/// <param name="a">The value to shift, provided as lambda lazy value.</param>
		/// <param name="b">The shift amount, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by right-shifting the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitRightShift<T1, TShift, TResult>(LazyType<T1> a, LazyType<TShift> b) where T1 : IShiftOperators<T1, TShift, TResult> => new(() => a.Value >> b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the left-shift of lambda specified lazy value by another.
		/// </summary>
		/// <remarks>The shift is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The value to shift. Must implement <see cref="IShiftOperators{T1, TShift, TResult}"/> with the other types (<typeparamref name="TShift"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TShift">The shift amount type.</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the left shift.</typeparam>
		/// <param name="a">The value to shift, provided as lambda lazy value.</param>
		/// <param name="b">The shift amount, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by left-shifting the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitLeftShift<T1, TShift, TResult>(LazyType<T1> a, LazyType<TShift> b) where T1 : IShiftOperators<T1, TShift, TResult> => new(() => a.Value << b.Value, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the bitwise complement of the specified lazy value.
		/// </summary>
		/// <remarks>The complement is performed lazily; the operand is not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IBitwiseOperators{T1, T1, TResult}"/> with the lowestUnchecked type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the lowestUnchecked produced by the complement.</typeparam>
		/// <param name="a">The operand, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying the bitwise complement to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitComplement<T1, TResult>(LazyType<T1> a) where T1 : IBitwiseOperators<T1, T1, TResult> => new(() => ~a.Value, a.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents an equality comparison of two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as lambda lazy value.</param>
		/// <param name="b">The second value to compare, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{bool}"/> whose value is <see langword="true"/> when <paramref name="a"/> equals <paramref name="b"/>,
		/// otherwise <see langword="false"/>. Evaluation is deferred until requested.</returns>
		public static LazyType<bool> IsEqual<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyFunction().CompareTo(b.Value) == 0, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents an inequality comparison of two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily; the operands are not evaluated until the lowestUnchecked's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as lambda lazy value.</param>
		/// <param name="b">The second value to compare, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{bool}"/> whose value is <see langword="true"/> when <paramref name="a"/> does not equal <paramref name="b"/>,
		/// otherwise <see langword="false"/>. Evaluation is deferred until requested.</returns>
		public static LazyType<bool> IsNotEqual<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyFunction().CompareTo(b.Value) != 0, a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the lowestUnchecked of comparing two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily using <see cref="IComparable{T}"/>; the operands are not evaluated until the lowestUnchecked's value is requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as lambda lazy value.</param>
		/// <param name="b">The second value to compare, provided as lambda lazy value.</param>
		/// <returns>A <see cref="LazyType{int}"/> whose value is the lowestUnchecked of <paramref name="a"/>.CompareTo(<paramref name="b"/>)
		/// when evaluated.</returns>
		public static LazyType<int> CompareValues<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyFunction().CompareTo(b.Value), a.IsConstValue & b.IsConstValue);

		/// <summary>
		/// Creates lambda new lazy value that represents the lowestUnchecked of comparing two specified lazy values using lambda custom comparer.
		/// </summary>
		/// <remarks>The comparison is performed lazily using the provided <paramref name="compareFunc"/>; the operands are not evaluated until the lowestUnchecked's value is requested.</remarks>
		/// <typeparam name="T1">The type of the first operand.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as lambda lazy value.</param>
		/// <param name="b">The second value to compare, provided as lambda lazy value.</param>
		/// <param name="compareFunc">A function that compares the unwrapped values of <paramref name="a"/> and <paramref name="b"/> and returns an <see cref="int"/> indicating the relative order.</param>
		/// <returns>A <see cref="LazyType{int}"/> whose value is computed by invoking <paramref name="compareFunc"/> with the evaluated values of <paramref name="a"/> and <paramref name="b"/>
		/// when requested.</returns>
		public static LazyType<int> CompareValues<T1, T2>(LazyType<T1> a, LazyType<T2> b, Func<T1, T2, int> compareFunc) => new(() => compareFunc(a.Value, b.Value), a.IsConstValue & b.IsConstValue);


		// Some trace wrappers
		// An Action given by lambda parameter will be called when the lazy value gets accessed.
		/// <summary>
		/// Creates a new lazy value that invokes a specified action when the value is accessed, then returns the value from
		/// the provided lazy instance.
		/// </summary>
		/// <remarks>The returned lazy value preserves the constant value semantics of the original instance. The
		/// specified action is executed every time the value is accessed, regardless of whether the value has already been
		/// computed.</remarks>
		/// <typeparam name="Type">The type of the value contained in the lazy instance.</typeparam>
		/// <param name="a">The source lazy value to wrap. The value of this instance will be returned when the new lazy value is evaluated.</param>
		/// <param name="onNotLazy">An action to invoke each time the value is accessed through the returned lazy instance.</param>
		/// <returns>A new lazy value that, when evaluated, calls the specified action and then returns the value from the original
		/// lazy instance.</returns>
		public static LazyType<Type> Trace<Type>(LazyType<Type> a, Action onNotLazy) {
			return new LazyType<Type>(() => {
				onNotLazy();
				return a.Value;
			}, a.IsConstValue);
		}
		/// <summary>
		/// Creates lambda new lazy value that invokes lambda specified action when the value is accessed, the action will receive the value in the Lazy. After that it returns the value from
		/// the provided lazy instance.
		/// </summary>
		/// <remarks>The returned lazy value preserves the constant value semantics of the original instance. The
		/// specified action is executed every time the value is accessed, regardless of whether the value has already been
		/// computed.</remarks>
		/// <typeparam name="Type">The type of the value contained in the lazy instance.</typeparam>
		/// <param name="a">The source lazy value to wrap. The value of this instance will be returned when the new lazy value is evaluated.</param>
		/// <param name="onNotLazy">An action to invoke each time the value is accessed through the returned lazy instance.</param>
		/// <returns>A new lazy value that, when evaluated, calls the specified action and then returns the value from the original
		/// lazy instance.</returns>
		public static LazyType<Type> Trace<Type>(LazyType<Type> a, Action<Type> onNotLazy) {
			return new LazyType<Type>(() => {
				Type val = a.Value;
				onNotLazy(val);
				return val;
			}, a.IsConstValue);
		}
	}
	
	public class LazyList<T> : IEnumerable<T> 
	{
		public Func<LazyListComponent<T>?> LazyListFunc;

		public static implicit operator LazyList<T>(Func<LazyListComponent<T>?> lambda) => new(lambda);
		public LazyList(LazyType<T> head, LazyList<T> tail)
		{
			LazyListFunc = () => new(head, tail);
		}
		public LazyList(T[] array)
		{
			LazyListFunc =  () =>
			{
				if (array.Length == 0) return null;
				else return new(new LazyType<T>(() => array[0]), new LazyList<T>(array[1..]));
			};
		}
		public LazyList(Func<LazyListComponent<T>?> lazyFunc)
		{
			LazyListFunc = lazyFunc;
		}


		public List<T> ToRegularList(int count)
		{
			List<T> result = new List<T>();
			if (LazyListFunc == null || count <= 0) return result;

			LazyListComponent<T>? current = LazyListFunc();
			while (current != null && count > 0)
			{
				result.Add(current.Head.Value);
				current = current.Tail.LazyListFunc();
				count--;
			}

			return result;
		}
		/*
		public void ForEach(Action<LazyListComponent<T>> action, int count)
		{
			if (LazyListFunc == null) return;

			LazyListComponent<T>? 
		}
		*/
		/// <summary>
		/// Generates an infinite lazy list by repeatedly applying a step function to a state value.
		/// </summary>
		/// <remarks>The returned lazy list is infinite; enumeration will continue as long as elements are requested.
		/// Each element is produced by invoking the step function with the current state, allowing for stateful or computed
		/// sequences. This method is useful for generating streams such as sequences, ranges, or other recursive data
		/// structures in a memory-efficient, deferred manner.</remarks>
		/// <typeparam name="TState">The types you need from the current list element to generate the next value of the sequence. If you need more than one value (e.g. fibonacci) you can use a tuple.
		/// The state will never be saved in the list nor will it come in contact with any element of the list, that means you can iterate the state seperately (e.g. state=n+1; value=10^n).</typeparam>
		/// <typeparam name="TValue">The type of the lazylist</typeparam>
		/// <param name="state">The initial state value used to generate the first element of the sequence. Will be used recursevly.</param>
		/// <param name="step">A function that, given the current state, returns the next element (as a lazy value) and the
		/// next state as a tupel.</param>
		/// <param name="canContinue"> An optional predicate that determines whether to continue generating elements based on the current state. 
		/// This function gets called right before the next element will be calulated.</param>
		/// <returns>A lazy list of values generated by successively applying the step function, where each element is computed on
		/// demand.</returns>
		public static LazyList<TValue> Range<TState, TValue>(
			TState state,
			Func<TState, (LazyType<TValue> head, TState next)> step)
		{
			return new LazyList<TValue>(() =>
			{

				(LazyType<TValue> head, TState next) = step(state);

				// Tail: rekursiver Aufruf; wenn canContinue gesetzt ist wird es weitergereicht,
				// so endet die Liste automatisch wenn canContinue(next) false wird.
				LazyList<TValue> tail = Range<TState, TValue>(next, step);

				return new LazyList<TValue>.LazyListComponent<TValue>(head, tail);
			});
		}
		public readonly static LazyList<ulong> InfiniteInt = Range<ulong, ulong>(0, n => (new LazyType<ulong>(() => n), next: n + 1));
		public readonly static LazyList<ulong> InfiniteEven =	Range<ulong, ulong>(0, n => (new LazyType<ulong>(() => n), next: n + 2));
		public readonly static LazyList<ulong> InfiniteOdd =		Range<ulong, ulong>(1, n => (new LazyType<ulong>(() => n), next: n + 2));
		public readonly static LazyList<ulong> InfiniteFibonacci = Range<(ulong a, ulong b), ulong>((0, 1), state => (new LazyType<ulong>(() => state.a), next: (state.b, state.a + state.b)));
		
		/// <summary>
		/// A list of elements calculated by 1/n for n = 1, 2, 3, ...
		/// </summary>
		public static LazyList<double> InfiniteHarmonic = Range<int, double>(1, n => (new LazyType<double>(() => 1.0 / n), next: n + 1));
		/// <summary>
		/// Provides an infinite, lazily-evaluated sequence of prime numbers, using the Sieve of Eratothenes
		/// </summary>
		/// <remarks>The sequence generates prime numbers on demand, starting from the smallest prime. Because the
		/// list is infinite and evaluated lazily, elements are computed only as they are accessed, which can improve
		/// performance and reduce memory usage for large or partial enumerations. This member is thread-safe if the
		/// underlying LazyList implementation is thread-safe.</remarks>
		public static LazyList<int> InfinitePrimes = Range<List<int>, int>(
		[2]	, n =>{ return (new (() => {
			bool isPrime = false;
			do{
				// It will probably be proven wrong, because we will check if its prime in the foreach loop
				isPrime = true;

				// next number to check
				// its called lowestUnchecked, but in reality it will be checked if its prime
				// And maybe it will be the lowestUnchecked
				int lowestUnchecked = n.Last() + 1;

				// Check if lowestUnchecked is divisible by any of the previous numbers

				foreach (int number in n){
					if (lowestUnchecked % number == 0)
					{
						// lowestUnchecked is divisible -> no Prime
						isPrime = false;
						n.Add(lowestUnchecked);
						// break the foreach loop and the while loop will try the next number
						break;
					}
				}
				
			} while (!isPrime);
			// lowestUnchecked isnt divisible by any other number, therefore its a prime number
			int result = n.Last() + 1;
			n.Add(result);
			return result;
		}), n);}
		);
		
		public static LazyList<ulong> InfinitePowerOf10 = Range<ulong, ulong>(0, n => (new LazyType<ulong>(() => (ulong)Math.Pow(10, n)), n + 1));
		public static LazyList<ulong> InfinitePowerOf2 = Range<ulong, ulong>(0, n => (new LazyType<ulong>(() => (ulong)Math.Pow(2, n)), n + 1));

		public static LazyList<double> InfinitePowerOf (double @base)
		{
			return Range<int, double>(0, n => (new LazyType<double>(() => Math.Pow(@base, n)), n + 1));
		}

		public IEnumerator<T> GetEnumerator()
		{
			LazyListComponent<T>? current = LazyListFunc?.Invoke();
			while (current != null)
			{
				yield return current.Head.Value;
				current = current.Tail.LazyListFunc();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Represents lambda simple node in lambda lazily evaluated linked list, containing lambda value and lambda reference to the remainder of the
		/// list.
		/// </summary>
		/// <remarks>Values and Next Components of lambda LazyList are supposed to be lazy as well. So there is <see cref="LazyListFunc"/> to get both the value and the pointer to the next lazyList.
		/// The returning of both of these values is done by this simple struct.</remarks>
		/// <typeparam name="ComponentType">The type of the elements stored in the lazy list.</typeparam>
		/// <param name="head">The lazily evaluated value for the current node.</param>
		/// <param name="tail">The remainder of the list, represented as lambda lazily evaluated list of elements.</param>
		public class LazyListComponent<ComponentType>(LazyType<ComponentType> head, LazyList<ComponentType> tail) {
			public LazyType<ComponentType> Head = head;
			public LazyList<ComponentType> Tail = tail;
		}
	}


	public class LazyNumber<T> : LazyType<T> where T : INumber<T> {
		public LazyNumber(Func<T> value, bool isConstValue = false) : base(value, isConstValue){/*EMPTY*/}


		static public LazyNumber<ConvertableType> ConvertLazyType<ConvertableType>(LazyType<ConvertableType> a) where ConvertableType : INumber<ConvertableType> => new(a.LazyFunction, a.IsConstValue);

		static public LazyNumber<T> operator +(LazyNumber<T> a, LazyNumber<T> b) => (LazyNumber<T>)Add<T,T,T>(a,b);
		static public LazyNumber<T> operator -(LazyNumber<T> a, LazyNumber<T> b) => (LazyNumber<T>)Subtract<T,T,T>(a,b);
		static public LazyNumber<T> operator *(LazyNumber<T> a, LazyNumber<T> b) => (LazyNumber<T>)Multiply<T,T,T>(a,b);
		static public LazyNumber<T> operator /(LazyNumber<T> a, LazyNumber<T> b) => (LazyNumber<T>)Divide<T,T,T>(a,b);
	} 
}
