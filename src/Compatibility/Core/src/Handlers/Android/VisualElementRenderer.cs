﻿#nullable enable
using System;
using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public abstract partial class VisualElementRenderer<TElement> : AViewGroup, INativeViewHandler
		where TElement : Element, IView
	{
		public void UpdateLayout()
		{
			if (Element != null)
				this.InvalidateMeasure(Element);
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (ChildCount > 0)
			{
				var platformView = GetChildAt(0);
				if (platformView != null)
				{
					platformView.Layout(l, t, r, b);
				}
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (ChildCount > 0)
			{
				var platformView = GetChildAt(0);
				if (platformView != null)
				{
					platformView.Measure(widthMeasureSpec, heightMeasureSpec);
					SetMeasuredDimension(platformView.MeasuredWidth, platformView.MeasuredHeight);
					return;
				}
			}

			SetMeasuredDimension(0, 0);
		}
	}
}
