﻿using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui;

namespace Maui.Controls.Sample.Pages.CollectionViewGalleries.CarouselViewGalleries
{
	[Preserve(AllMembers = true)]
	public partial class ExampleTemplateCarousel : Microsoft.Maui.Controls.Grid
	{
		double _initialY = -1;
		bool _delete;
		readonly double _maxYScroll = 300;
		readonly double _diffYScroll = -150;
		readonly double _minYScroll = -30;

		public ExampleTemplateCarousel()
		{
			InitializeComponent();

			var gesture = new PanGestureRecognizer();

			gesture.PanUpdated += (sender, e) =>
			{
				if (e.StatusType == GestureStatus.Started)
				{
					_initialY = Y;
				}

				if (e.StatusType == GestureStatus.Running)
				{
					if (e.TotalY < _minYScroll)
					{
						var scaledValue = 1 - (Math.Abs(e.TotalY) / _maxYScroll);
						this.ScaleTo(0.9);
						this.FadeTo(scaledValue);
						this.TranslateTo(X, Y + e.TotalY);
					}
					if (e.TotalY < _diffYScroll)
					{
						_delete = true;
					}
				}

				if (e.StatusType == GestureStatus.Completed || e.StatusType == GestureStatus.Canceled)
				{
					if (_delete)
					{
						this.FadeTo(0.1);
						this.TranslateTo(X, Y - 1000);
						MessagingCenter.Send(this, "remove");
					}
					else
					{
						this.ScaleTo(1);
						this.FadeTo(1);
						this.TranslateTo(X, _initialY);
					}
				}
			};
			//GestureRecognizers.Add(gesture);
		}
	}
}