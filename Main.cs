using System.Numerics;

namespace LaC_Sharp {

	public class LazyType<T>(Func<T> value, bool isConstValue = false) {
		public Func<T> LazyValue = value;
		public bool IsConstValue = isConstValue;

		/// <summary>
		/// Gets the current value held by the proxy. If no proxy is held it will calculate it.
		/// See <seealso cref="ProxyValue"/> to get <see langword="null"/> if no value was set yet.
		/// </summary>
		public T ValueNoNull { get { return ProxyValue == null ? LazyValue() : ProxyValue; } }
		/// <summary>
		/// Gets the current saved value, or <see langword="null"/> if no value is set.
		/// See <seealso cref="ValueNoNull"/> to compute the value when called.
		/// </summary>
		public T? ProxyValue { get; private set; }


		static public implicit operator Func<T>(LazyType<T> lazy) => lazy.LazyValue;
		static public implicit operator LazyType<T>(Func<T> func) => new(func);
		static public implicit operator T(LazyType<T> lazy) => lazy.LazyValue();
		static public implicit operator LazyType<T>(T value) => new(() => value);
		
		
		
		/// <summary>
		/// Creates a new lazy value that represents the sum of two specified lazy values.
		/// </summary>
		/// <remarks>The addition is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IAdditionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the addition.</typeparam>
		/// <param name="a">The first summand, provided as a lazy value.</param>
		/// <param name="b">The second summand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by adding the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Add<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IAdditionOperators<T1, T2, TResult> => new(() => a.LazyValue() + b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the difference of two specified lazy values.
		/// </summary>
		/// <remarks>The subtraction is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the minuend. Must implement <see cref="ISubtractionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the subtrahend.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the subtraction.</typeparam>
		/// <param name="a">The minuend, provided as a lazy value.</param>
		/// <param name="b">The subtrahend, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by subtracting the value of <paramref name="b"/> from <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Subtract<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : ISubtractionOperators<T1, T2, TResult> => new(() => a.LazyValue() - b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the product of two specified lazy values.
		/// </summary>
		/// <remarks>The multiplication is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first factor. Must implement <see cref="IMultiplyOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second factor.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the multiplication.</typeparam>
		/// <param name="a">The first factor, provided as a lazy value.</param>
		/// <param name="b">The second factor, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by multiplying the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Multiply<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IMultiplyOperators<T1, T2, TResult> => new(() => a.LazyValue() * b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the quotient of two specified lazy values.
		/// </summary>
		/// <remarks>The division is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The dividend type. Must implement <see cref="IDivisionOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The divisor type.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the division.</typeparam>
		/// <param name="a">The dividend, provided as a lazy value.</param>
		/// <param name="b">The divisor, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by dividing the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Divide<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IDivisionOperators<T1, T2, TResult> => new(() => a.LazyValue() / b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the remainder of dividing two specified lazy values.
		/// </summary>
		/// <remarks>The modulus operation is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The dividend type. Must implement <see cref="IModulusOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The divisor type.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the modulus operation.</typeparam>
		/// <param name="a">The dividend, provided as a lazy value.</param>
		/// <param name="b">The divisor, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by taking the modulus of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Modulo<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IModulusOperators<T1, T2, TResult> => new(() => a.LazyValue() % b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the unary negation of the specified lazy value.
		/// </summary>
		/// <remarks>The negation is performed lazily; the operand is not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IUnaryNegationOperators{T1, TResult}"/> with the result type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the negation.</typeparam>
		/// <param name="a">The operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying unary negation to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> Negate<T1, TResult>(LazyType<T1> a) where T1 : IUnaryNegationOperators<T1, TResult> => new(() => -a.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the bitwise AND of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise AND is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the bitwise AND.</typeparam>
		/// <param name="a">The first operand, provided as a lazy value.</param>
		/// <param name="b">The second operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise AND on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitAnd<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.LazyValue() & b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the bitwise OR of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise OR is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the bitwise OR.</typeparam>
		/// <param name="a">The first operand, provided as a lazy value.</param>
		/// <param name="b">The second operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise OR on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitOr<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.LazyValue() | b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the bitwise XOR of two specified lazy values.
		/// </summary>
		/// <remarks>The bitwise XOR is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IBitwiseOperators{T1, T2, TResult}"/> with the other types (<typeparamref name="T1"/>, <typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the bitwise XOR.</typeparam>
		/// <param name="a">The first operand, provided as a lazy value.</param>
		/// <param name="b">The second operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by performing bitwise XOR on the values of <paramref name="a"/> and <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitXor<T1, T2, TResult>(LazyType<T1> a, LazyType<T2> b) where T1 : IBitwiseOperators<T1, T2, TResult> => new(() => a.LazyValue() ^ b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the bitwise NOT (complement) of the specified lazy value.
		/// </summary>
		/// <remarks>The bitwise complement is performed lazily; the operand is not evaluated until the result's value is
		/// requested. This method forwards to <see cref="BitComplement{T1, TResult}"/>.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IBitwiseOperators{T1, T1, TResult}"/> with the result type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the bitwise NOT.</typeparam>
		/// <param name="a">The operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying bitwise NOT to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitNot<T1, TResult>(LazyType<T1> a) where T1 : IBitwiseOperators<T1, T1, TResult>
		{
			return BitComplement<T1, TResult>(a);
		}

		/// <summary>
		/// Creates a new lazy value that represents the right-shift of a specified lazy value by another.
		/// </summary>
		/// <remarks>The shift is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The value to shift. Must implement <see cref="IShiftOperators{T1, TShift, TResult}"/> with the shift type and result type.</typeparam>
		/// <typeparam name="TShift">The shift amount type.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the right shift.</typeparam>
		/// <param name="a">The value to shift, provided as a lazy value.</param>
		/// <param name="b">The shift amount, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by right-shifting the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitRightShift<T1, TShift, TResult>(LazyType<T1> a, LazyType<TShift> b) where T1 : IShiftOperators<T1, TShift, TResult> => new(() => a.LazyValue() >> b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the left-shift of a specified lazy value by another.
		/// </summary>
		/// <remarks>The shift is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The value to shift. Must implement <see cref="IShiftOperators{T1, TShift, TResult}"/> with the shift type and result type.</typeparam>
		/// <typeparam name="TShift">The shift amount type.</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the left shift.</typeparam>
		/// <param name="a">The value to shift, provided as a lazy value.</param>
		/// <param name="b">The shift amount, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by left-shifting the value of <paramref name="a"/> by <paramref name="b"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitLeftShift<T1, TShift, TResult>(LazyType<T1> a, LazyType<TShift> b) where T1 : IShiftOperators<T1, TShift, TResult> => new(() => a.LazyValue() << b.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents the bitwise complement of the specified lazy value.
		/// </summary>
		/// <remarks>The complement is performed lazily; the operand is not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The operand type. Must implement <see cref="IBitwiseOperators{T1, T1, TResult}"/> with the result type (<typeparamref name="TResult"/>).</typeparam>
		/// <typeparam name="TResult">The type of the result produced by the complement.</typeparam>
		/// <param name="a">The operand, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{T}"/> whose value is computed by applying the bitwise complement to the value of <paramref name="a"/>
		/// when evaluated.</returns>
		public static LazyType<TResult> BitComplement<T1, TResult>(LazyType<T1> a) where T1 : IBitwiseOperators<T1, T1, TResult> => new(() => ~a.LazyValue());

		/// <summary>
		/// Creates a new lazy value that represents an equality comparison of two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as a lazy value.</param>
		/// <param name="b">The second value to compare, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{bool}"/> whose value is <see langword="true"/> when <paramref name="a"/> equals <paramref name="b"/>,
		/// otherwise <see langword="false"/>. Evaluation is deferred until requested.</returns>
		public static LazyType<bool> IsEqual<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyValue().CompareTo(b.LazyValue()) == 0);

		/// <summary>
		/// Creates a new lazy value that represents an inequality comparison of two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily; the operands are not evaluated until the result's value is
		/// requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as a lazy value.</param>
		/// <param name="b">The second value to compare, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{bool}"/> whose value is <see langword="true"/> when <paramref name="a"/> does not equal <paramref name="b"/>,
		/// otherwise <see langword="false"/>. Evaluation is deferred until requested.</returns>
		public static LazyType<bool> IsNotEqual<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyValue().CompareTo(b.LazyValue()) != 0);

		/// <summary>
		/// Creates a new lazy value that represents the result of comparing two specified lazy values.
		/// </summary>
		/// <remarks>The comparison is performed lazily using <see cref="IComparable{T}"/>; the operands are not evaluated until the result's value is requested.</remarks>
		/// <typeparam name="T1">The type of the first operand. Must implement <see cref="IComparable{T2}"/> with <typeparamref name="T2"/>.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as a lazy value.</param>
		/// <param name="b">The second value to compare, provided as a lazy value.</param>
		/// <returns>A <see cref="LazyType{int}"/> whose value is the result of <paramref name="a"/>.CompareTo(<paramref name="b"/>)
		/// when evaluated.</returns>
		public static LazyType<int> CompareValues<T1, T2>(LazyType<T1> a, LazyType<T2> b) where T1 : IComparable<T2> => new(() => a.LazyValue().CompareTo(b.LazyValue()));

		/// <summary>
		/// Creates a new lazy value that represents the result of comparing two specified lazy values using a custom comparer.
		/// </summary>
		/// <remarks>The comparison is performed lazily using the provided <paramref name="compareFunc"/>; the operands are not evaluated until the result's value is requested.</remarks>
		/// <typeparam name="T1">The type of the first operand.</typeparam>
		/// <typeparam name="T2">The type of the second operand.</typeparam>
		/// <param name="a">The first value to compare, provided as a lazy value.</param>
		/// <param name="b">The second value to compare, provided as a lazy value.</param>
		/// <param name="compareFunc">A function that compares the unwrapped values of <paramref name="a"/> and <paramref name="b"/> and returns an <see cref="int"/> indicating the relative order.</param>
		/// <returns>A <see cref="LazyType{int}"/> whose value is computed by invoking <paramref name="compareFunc"/> with the evaluated values of <paramref name="a"/> and <paramref name="b"/>
		/// when requested.</returns>
		public static LazyType<int> CompareValues<T1, T2>(LazyType<T1> a, LazyType<T2> b, Func<T1, T2, int> compareFunc) => new(() => compareFunc(a.LazyValue(), b.LazyValue()));
	}

	public class LazyList<T> {
		//public LazyType<T>;
	}

	public class LazyNumber<T> : LazyType<T> where T : INumber<T> {
		public LazyNumber(Func<T> value) : base(value) {/*EMPTY*/}

		static public LazyNumber<T> operator +(LazyNumber<T> a, LazyNumber<T> b) => new LazyNumber<T>(() => a.LazyValue() + b.LazyValue());
		static public LazyNumber<T> operator -(LazyNumber<T> a, LazyNumber<T> b) => new LazyNumber<T>(() => a.LazyValue() - b.LazyValue());
		static public LazyNumber<T> operator *(LazyNumber<T> a, LazyNumber<T> b) => new LazyNumber<T>(() => a.LazyValue() * b.LazyValue());
		static public LazyNumber<T> operator /(LazyNumber<T> a, LazyNumber<T> b) => new LazyNumber<T>(() => a.LazyValue() / b.LazyValue());
	} 
}
