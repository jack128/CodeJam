﻿using System;
using System.Collections.Generic;

using CodeJam.Arithmetic;
using CodeJam.Reflection;

namespace CodeJam.Ranges
{
	/// <summary>Range internal helpers</summary>
	internal static class RangeInternal
	{
		#region The stub param to mark unsafe overloads
		/// <summary>The message for unsafe (no arg validation) method.</summary>
		internal const string SkipsArgValidationObsolete =
			"Marked as obsolete to emit warnings on incorrect usage. " +
				"Read comments on RangeInternal.UnsafeOverload before suppressing the warning.";

		/// <summary>
		/// Helper enum to mark unsafe (no validation) constructor overloads.
		/// Should be used ONLY if the arguments are validated already
		/// AND the code is on the hotpath.
		/// </summary>
		[Obsolete(SkipsArgValidationObsolete)]
		internal enum UnsafeOverload
		{
			/// <summary>
			/// Marks unsafe (no validation) constructor overload.
			/// Should be used ONLY if the arguments are validated already
			/// AND the code is on the hotpath.
			/// </summary>
			SkipsArgValidation
		}

		/// <summary>
		/// Helper const to mark unsafe (no validation) constructor overloads.
		/// Should be used ONLY if the arguments are validated already
		/// AND the code is on the hotpath.
		/// </summary>
		[Obsolete(SkipsArgValidationObsolete)]
#pragma warning disable 618 // The warning is transitive: the constant is marked as obsolete.
		internal const UnsafeOverload SkipsArgValidation = UnsafeOverload.SkipsArgValidation;
#pragma warning restore 618
		#endregion

		#region Boundary constants
		internal const string EmptyString = "∅";

		internal const string NegativeInfinityBoundaryString = "(-∞";
		internal const string PositiveInfinityBoundaryString = "+∞)";

		internal const string FromExclusiveString = "(";
		internal const string FromInclusiveString = "[";
		internal const string ToExclusiveString = ")";
		internal const string ToInclusiveString = "]";
		#endregion

		#region Range constants
		internal const string KeyPrefixString = "'";
		internal const string KeySeparatorString = "':";
		internal const string SeparatorString = "..";
		#endregion

		/// <summary>Creates formattable callback for arbitrary type.</summary>
		/// <typeparam name="T">Type of the formattable object.</typeparam>
		/// <returns>The format callback. Returns <c>null</c> if the first arg is <c>null</c>.</returns>
		internal static Func<T, string, IFormatProvider, string> CreateFormattableCallback<T>()
		{
			if (typeof(IFormattable).IsAssignableFrom(typeof(T)))
			{
				var method = typeof(RangeInternal).GetMethod(nameof(Format)).MakeGenericMethod(typeof(T));
				// no boxing for IFormatProvider
				return (Func<T, string, IFormatProvider, string>)Delegate.CreateDelegate(
					typeof(Func<T, string, IFormatProvider, string>),
					method,
					true);
			}
			if (typeof(IFormattable).IsAssignableFrom(typeof(T).ToNullableUnderlying()))
			{
				var method = typeof(RangeInternal).GetMethod(nameof(FormatNullable)).MakeGenericMethod(typeof(T).ToNullableUnderlying());
				// no boxing for IFormatProvider
				return (Func<T, string, IFormatProvider, string>)Delegate.CreateDelegate(
					typeof(Func<T, string, IFormatProvider, string>),
					method,
					true);
			}
			return (value, format, formatProvider) => value?.ToString();
		}

		public static string Format<T>(T value, string format, IFormatProvider formatProvider) where T : IFormattable =>
			value?.ToString(format, formatProvider);

		public static string FormatNullable<T>(T? value, string format, IFormatProvider formatProvider)
			where T : struct, IFormattable =>
				value?.ToString(format, formatProvider);
	}
}