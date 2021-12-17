#nullable enable
using System;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		FrameworkElement? INativeViewHandler.NativeView => this.GetWrappedNativeView();
		FrameworkElement? INativeViewHandler.ContainerView => ContainerView;

		public new WrapperView? ContainerView
		{
			get => (WrapperView?)base.ContainerView;
			protected set => base.ContainerView = value;
		}

		public override void NativeArrange(Rectangle rect)
		{
			var nativeView = this.GetWrappedNativeView();

			if (nativeView == null)
				return;

			if (rect.Width < 0 || rect.Height < 0)
				return;

			nativeView.Arrange(new global::Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height));

			Invoke(nameof(IView.Frame), rect);
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var nativeView = this.GetWrappedNativeView();

			if (nativeView == null)
				return Size.Zero;

			widthConstraint = AdjustForExplicitSize(widthConstraint, nativeView.Width);
			heightConstraint = AdjustForExplicitSize(heightConstraint, nativeView.Height);

			if (widthConstraint < 0 || heightConstraint < 0)
				return Size.Zero;

			var measureConstraint = new global::Windows.Foundation.Size(widthConstraint, heightConstraint);

			nativeView.Measure(measureConstraint);

			return new Size(nativeView.DesiredSize.Width, nativeView.DesiredSize.Height);
		}

		protected override void SetupContainer()
		{
			if (NativeView == null || ContainerView != null)
				return;

			var oldParent = (Panel?)NativeView.Parent;

			var oldIndex = oldParent?.Children.IndexOf(NativeView);
			oldParent?.Children.Remove(NativeView);

			ContainerView ??= new WrapperView();
			ContainerView.Child = NativeView;

			if (oldIndex is int idx && idx >= 0)
				oldParent?.Children.Insert(idx, ContainerView);
			else
				oldParent?.Children.Add(ContainerView);
		}

		protected override void RemoveContainer()
		{
			if (NativeView == null || ContainerView == null || NativeView.Parent != ContainerView)
				return;

			var oldParent = (Panel?)ContainerView.Parent;

			var oldIndex = oldParent?.Children.IndexOf(ContainerView);
			oldParent?.Children.Remove(ContainerView);

			ContainerView.Child = null;
			ContainerView = null;

			if (oldIndex is int idx && idx >= 0)
				oldParent?.Children.Insert(idx, NativeView);
			else
				oldParent?.Children.Add(NativeView);
		}

		static double AdjustForExplicitSize(double externalConstraint, double explicitValue)
		{
			// Even with an explicit value specified, Windows will limit the size of the control to 
			// the size of the parent's explicit size. Since we want our controls to get their
			// explicit sizes regardless (and possibly exceed the size of their layouts), we need
			// to measure them at _at least_ their explicit size.
			
			if (double.IsNaN(explicitValue))
			{
				// NaN for an explicit height/width on Windows means "unspecified", so we just use the external value
				return externalConstraint;
			}

			// If the control's explicit height/width is larger than the containers, use the control's value
			return Math.Max(externalConstraint, explicitValue);
		}
	}
}