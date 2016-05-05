﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

using static NUnit.Framework.Assert;

namespace CodeJam.RangesV2
{
	[TestFixture(Category = "Ranges")]
	[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
	[SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
	[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
	[SuppressMessage("ReSharper", "ConvertToConstant.Local")]
	[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
	public class RangeBoundaryToTests
	{
		[Test]
		public static void TestBoundaryToCreation()
		{
			int? value1 = 1;
			int? value2 = 2;
			int? empty = null;

			DoesNotThrow(() => new RangeBoundaryTo<int?>(empty, RangeBoundaryToKind.Empty));
			DoesNotThrow(() => new RangeBoundaryTo<int?>(empty, RangeBoundaryToKind.Infinite));
			DoesNotThrow(() => new RangeBoundaryTo<int?>(value1, RangeBoundaryToKind.Exclusive));
			DoesNotThrow(() => new RangeBoundaryTo<int?>(value2, RangeBoundaryToKind.Inclusive));

			Throws<ArgumentException>(() => new RangeBoundaryTo<int?>(value1, RangeBoundaryToKind.Empty));
			Throws<ArgumentException>(() => new RangeBoundaryTo<int?>(value2, RangeBoundaryToKind.Infinite));
			Throws<ArgumentException>(() => new RangeBoundaryTo<int?>(empty, RangeBoundaryToKind.Inclusive));
			Throws<ArgumentException>(() => new RangeBoundaryTo<int?>(empty, RangeBoundaryToKind.Exclusive));

			AreEqual(
				RangeBoundaryTo<int?>.PositiveInfinity,
				Range.BoundaryToInfinity<int?>());
			AreEqual(
				RangeBoundaryTo<int?>.PositiveInfinity,
				Range.BoundaryTo(empty));
			AreEqual(
				RangeBoundaryTo<int?>.PositiveInfinity,
				Range.BoundaryToExclusive(empty));
			AreEqual(
				new RangeBoundaryTo<int?>(value1, RangeBoundaryToKind.Inclusive),
				Range.BoundaryTo(value1));
			AreEqual(
				new RangeBoundaryTo<int?>(value1, RangeBoundaryToKind.Exclusive),
				Range.BoundaryToExclusive(value1));
			AreNotEqual(
				new RangeBoundaryTo<int?>(value1, RangeBoundaryToKind.Exclusive),
				Range.BoundaryToExclusive(value2));
		}

		[Test]
		public static void TestBoundaryPositiveInfinityValue()
		{
			double? infOk = double.PositiveInfinity;
			double? infFail = double.NegativeInfinity;
			double? empty = null;

			DoesNotThrow(() => new RangeBoundaryTo<double?>(infOk, RangeBoundaryToKind.Infinite));

			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infOk, RangeBoundaryToKind.Empty));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infFail, RangeBoundaryToKind.Empty));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infFail, RangeBoundaryToKind.Infinite));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infOk, RangeBoundaryToKind.Inclusive));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infFail, RangeBoundaryToKind.Inclusive));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infOk, RangeBoundaryToKind.Exclusive));
			Throws<ArgumentException>(() => new RangeBoundaryTo<double?>(infFail, RangeBoundaryToKind.Exclusive));

			AreEqual(
				RangeBoundaryTo<double?>.PositiveInfinity,
				Range.BoundaryTo(infOk));
			AreEqual(
				RangeBoundaryTo<double?>.PositiveInfinity,
				Range.BoundaryToExclusive(infOk));

			AreEqual(
				RangeBoundaryTo<double>.PositiveInfinity,
				Range.BoundaryTo(infOk.Value));
			AreEqual(
				RangeBoundaryTo<double>.PositiveInfinity,
				Range.BoundaryToExclusive(infOk.Value));

			AreEqual(
				Range.BoundaryToExclusive(infOk).GetValueOrDefault(),
				empty);
		}

		[Test]
		public static void TestBoundaryToProperties()
		{
			int? value1 = 1;
			int? value2 = 2;
			int? empty = null;

			var a = new RangeBoundaryTo<int?>();
			AreEqual(a.Kind, RangeBoundaryToKind.Empty);
			AreEqual(a.GetValueOrDefault(), empty);
			Throws<InvalidOperationException>(() => a.Value.ToString());

			IsTrue(a.IsEmpty);
			IsFalse(a.IsNotEmpty);
			IsFalse(a.HasValue);
			IsFalse(a.IsPositiveInfinity);
			IsFalse(a.IsInclusiveBoundary);
			IsFalse(a.IsExclusiveBoundary);

			a = Range.BoundaryToInfinity<int?>();
			AreEqual(a.Kind, RangeBoundaryToKind.Infinite);
			AreEqual(a.GetValueOrDefault(), empty);
			Throws<InvalidOperationException>(() => a.Value.ToString());

			IsFalse(a.IsEmpty);
			IsTrue(a.IsNotEmpty);
			IsFalse(a.HasValue);
			IsTrue(a.IsPositiveInfinity);
			IsFalse(a.IsInclusiveBoundary);
			IsFalse(a.IsExclusiveBoundary);

			a = Range.BoundaryTo(value1);
			AreEqual(a.Kind, RangeBoundaryToKind.Inclusive);
			AreEqual(a.Value, value1);
			AreNotEqual(a.Value, value2);

			IsFalse(a.IsEmpty);
			IsTrue(a.IsNotEmpty);
			IsTrue(a.HasValue);
			IsFalse(a.IsPositiveInfinity);
			IsTrue(a.IsInclusiveBoundary);
			IsFalse(a.IsExclusiveBoundary);

			a = Range.BoundaryToExclusive(value1);
			AreEqual(a.Kind, RangeBoundaryToKind.Exclusive);
			AreEqual(a.Value, value1);
			AreNotEqual(a.Value, value2);

			IsFalse(a.IsEmpty);
			IsTrue(a.IsNotEmpty);
			IsTrue(a.HasValue);
			IsFalse(a.IsPositiveInfinity);
			IsFalse(a.IsInclusiveBoundary);
			IsTrue(a.IsExclusiveBoundary);
		}

		[Test]
		public static void TestBoundaryToEquality()
		{
			int? value1 = 1;
			int? value2 = 2;
			int? empty = null;

			var e = new RangeBoundaryTo<int?>();
			var inf = Range.BoundaryTo(empty);
			var a1 = Range.BoundaryTo(value1);
			var a12 = Range.BoundaryTo(value1);
			var a2 = Range.BoundaryTo(value2);
			var b1 = Range.BoundaryFrom(value1);

			AreEqual(e, RangeBoundaryTo<int?>.Empty);
			IsTrue(e == RangeBoundaryTo<int?>.Empty);
			IsFalse(e != RangeBoundaryTo<int?>.Empty);

			AreEqual(inf, RangeBoundaryTo<int?>.PositiveInfinity);
			IsTrue(inf == RangeBoundaryTo<int?>.PositiveInfinity);

			AreNotEqual(a1, empty);
			AreNotEqual(a1, inf);

			AreEqual(a1, a12);
			IsTrue(a1 == a12);
			IsFalse(a1 != a12);

			AreNotEqual(a1, a2);
			IsFalse(a1 == a2);
			IsTrue(a1 != a2);

			AreEqual(a1.Value, value1);
			AreNotEqual(a1.Value, value2);
			AreNotEqual(a1, value1);
			AreNotEqual(a1, value2);

			AreNotEqual(a1, b1);
			AreEqual(b1.Value, value1);
		}

		[Test]
		public static void TestBoundaryToComplementation()
		{
			int? value1 = 1;
			int? value2 = 2;

			var boundary = Range.BoundaryTo(value1);
			var boundaryCompl = boundary.GetComplementation();
			IsTrue(boundaryCompl.IsComplementationFor(boundary));
			IsFalse(boundaryCompl.IsComplementationFor(Range.BoundaryTo(value2)));
			IsFalse(boundaryCompl.IsComplementationFor(RangeBoundaryTo<int?>.Empty));
			IsFalse(boundaryCompl.IsComplementationFor(RangeBoundaryTo<int?>.PositiveInfinity));
			AreEqual(boundaryCompl.Value, 1);
			AreEqual(boundaryCompl.Kind, RangeBoundaryFromKind.Exclusive);
			AreEqual(boundaryCompl.GetComplementation(), boundary);

			boundary = Range.BoundaryToExclusive(value1);
			boundaryCompl = boundary.GetComplementation();
			IsTrue(boundaryCompl.IsComplementationFor(boundary));
			IsFalse(boundaryCompl.IsComplementationFor(Range.BoundaryTo(value2)));
			IsFalse(boundaryCompl.IsComplementationFor(RangeBoundaryTo<int?>.Empty));
			IsFalse(boundaryCompl.IsComplementationFor(RangeBoundaryTo<int?>.PositiveInfinity));
			AreEqual(boundaryCompl.Value, 1);
			AreEqual(boundaryCompl.Kind, RangeBoundaryFromKind.Inclusive);
			AreEqual(boundaryCompl.GetComplementation(), boundary);

			boundary = RangeBoundaryTo<int?>.Empty;
			Throws<InvalidOperationException>(() => boundary.GetComplementation());
			boundary = RangeBoundaryTo<int?>.PositiveInfinity;
			Throws<InvalidOperationException>(() => boundary.GetComplementation());
		}

		[Test]
		public static void TestBoundaryToUpdate()
		{
			int? value1 = 1;
			int? value2 = 2;

			var boundary = Range.BoundaryTo(value1);
			var boundary2 = boundary.UpdateValue(i => i + 1);
			AreEqual(boundary.Kind, boundary2.Kind);
			AreEqual(boundary2.Value, value2);

			boundary = Range.BoundaryToExclusive(value1);
			boundary2 = boundary.UpdateValue(i => i + 1);
			AreEqual(boundary.Kind, boundary2.Kind);
			AreEqual(boundary2.Value, value2);

			boundary = RangeBoundaryTo<int?>.Empty;
			boundary2 = boundary.UpdateValue(i => i + 1);
			AreEqual(boundary, boundary2);
			boundary = RangeBoundaryTo<int?>.PositiveInfinity;
			boundary2 = boundary.UpdateValue(i => i + 1);
			AreEqual(boundary, boundary2);
		}
	}
}