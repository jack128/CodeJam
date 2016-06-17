﻿using System;

using BenchmarkDotNet.Running;

using CodeJam.PerfTests.Running.CompetitionLimits;

using JetBrains.Annotations;

namespace CodeJam.PerfTests.Running.SourceAnnotations
{
	/// <summary>
	/// Stores limits for benchmark target
	/// </summary>
	/// <seealso cref="CompetitionLimit"/>
	/// <seealso cref="Target"/>
	internal class CompetitionTarget : CompetitionLimit
	{
		private static readonly CompetitionLimitProperties _allPropertiesMask =
			EnumHelper.GetFlagsMask<CompetitionLimitProperties>();

		#region Fields & .ctor
		private CompetitionLimitProperties _changedProperties;

		// TODO: check .ctor usages
		/// <summary>Initializes a new instance of the <see cref="CompetitionTarget"/> class.</summary>
		/// <param name="target">The target.</param>
		/// <param name="minRatio">The minimum timing ratio relative to the baseline.</param>
		/// <param name="maxRatio">The maximum timing ratio relative to the baseline.</param>
		/// <param name="fromMetadataResource"><c>True</c> if the limits are obtained from XML resource.</param>
		/// <param name="metadataResourcePath">The relative path to the resource containing xml document with competition limits.</param>
		public CompetitionTarget(
			[CanBeNull] Target target,
			double minRatio, double maxRatio,
			bool fromMetadataResource, string metadataResourcePath) :
				base(minRatio, maxRatio)
		{
			Target = target;
			FromMetadataResource = fromMetadataResource;
			MetadataResourcePath = metadataResourcePath;
		}

		/// <summary>Initializes a new instance of the <see cref="CompetitionTarget"/> class.</summary>
		/// <param name="target">The target.</param>
		/// <param name="other">Competition limits source.</param>
		/// <param name="fromMetadataResource"><c>True</c> if the limits are obtained from XML resource.</param>
		/// <param name="metadataResourcePath">The relative path to the resource containing xml document with competition limits.</param>
		public CompetitionTarget(
			[CanBeNull] Target target,
			[NotNull] CompetitionLimit other,
			bool fromMetadataResource, string metadataResourcePath) :
				this(target, other.MinRatio, other.MaxRatio, fromMetadataResource, metadataResourcePath) { }
		#endregion

		#region Properties
		/// <summary>The benchmark target.</summary>
		/// <value>The benchmark target.</value>
		public Target Target { get; }

		/// <summary>Gets a value indicating whether benchmark limits are obtained from XML resource.</summary>
		/// <value>
		/// <c>true</c> if benchmark limits are obtained from XML resource.; otherwise, <c>false</c>.
		/// </value>
		public bool FromMetadataResource { get; }

		/// <summary>The relative path to the resource containing xml document with competition limits.</summary>
		/// <value>The relative path to the resource containing xml document with competition limits.</value>
		public string MetadataResourcePath { get; }

		/// <summary>The limit properties are updated but not saved.</summary>
		/// <value><c>true</c> if this instance has unsaved changes; otherwise, <c>false</c>.</value>
		public bool HasUnsavedChanges => _changedProperties != CompetitionLimitProperties.None;
		#endregion

		/// <summary>Determines whether all of the specified properties is changed.</summary>
		/// <param name="property">The properties to check.</param>
		/// <returns></returns>
		public bool IsChanged(CompetitionLimitProperties property) =>
			property != CompetitionLimitProperties.None && _changedProperties.IsFlagSet(property);

		#region Core logic for adjusting the limits
		private void MarkAsChanged(CompetitionLimitProperties property) =>
			_changedProperties = _changedProperties.SetFlag(property);

		private bool UnionWithMinRatio(double newMin)
		{
			if (ShouldBeUpdatedMin(MinRatio, newMin))
			{
				MinRatio = newMin;
				MarkAsChanged(CompetitionLimitProperties.MinRatio);
				return true;
			}

			return false;
		}

		private bool UnionWithMaxRatio(double newMax)
		{
			if (ShouldBeUpdatedMax(MaxRatio, newMax))
			{
				MaxRatio = newMax;
				MarkAsChanged(CompetitionLimitProperties.MaxRatio);
				return true;
			}

			return false;
		}
		#endregion

		/// <summary>Adjusts competition limits with specified values.</summary>
		/// <param name="limitsHolder">Limits to merge with.</param>
		/// <returns><c>true</c> if any of the limits were updated.</returns>
		public bool UnionWith([NotNull] CompetitionLimit limitsHolder)
		{
			Code.NotNull(limitsHolder, nameof(limitsHolder));

			var result = false;
			result |= UnionWithMinRatio(limitsHolder.MinRatio);
			result |= UnionWithMaxRatio(limitsHolder.MaxRatio);
			return result;
		}

		/// <summary>Looses the limits.</summary>
		/// <param name="percent">Percent to loose by.</param>
		/// <exception cref="ArgumentOutOfRangeException">The percent is not in range 0..99</exception>
		public void LooseLimits(int percent) => LooseLimits(percent, _allPropertiesMask);

		/// <summary>Looses the limits.</summary>
		/// <param name="percent">Percent to loose by.</param>
		/// <param name="propertiesToLoose">The properties to loose.</param>
		/// <exception cref="ArgumentOutOfRangeException">The percent is not in range 0..99</exception>
		public void LooseLimits(int percent, CompetitionLimitProperties propertiesToLoose)
		{
			Code.InRange(percent, nameof(percent), 0, 99);

			propertiesToLoose &= _allPropertiesMask;

			if (IsChanged(CompetitionLimitProperties.MinRatio & propertiesToLoose))
			{
				var newValue = Math.Floor(MinRatio * (100 - percent)) / 100;
				UnionWithMinRatio(newValue);
			}
			if (IsChanged(CompetitionLimitProperties.MaxRatio & propertiesToLoose))
			{
				var newValue = Math.Ceiling(MaxRatio * (100 + percent)) / 100;
				UnionWithMaxRatio(newValue);
			}
		}

		/// <summary>Marks limits as saved.</summary>
		public void MarkAsSaved() => MarkAsSaved(_allPropertiesMask);

		/// <summary>Marks limits as saved.</summary>
		/// <param name="propertiesToLoose">The properties to loose.</param>
		public void MarkAsSaved(CompetitionLimitProperties propertiesToLoose)
		{
			propertiesToLoose &= _allPropertiesMask;
			_changedProperties = _changedProperties.ClearFlag(propertiesToLoose);
		}
	}
}