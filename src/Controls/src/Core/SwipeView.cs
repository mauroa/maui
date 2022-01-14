using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Microsoft.Maui.Controls
{
	/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="Type[@FullName='Microsoft.Maui.Controls.SwipeView']/Docs" />
	[ContentProperty(nameof(Content))]
	public class SwipeView : ContentView, IElementConfiguration<SwipeView>, ISwipeViewController
	{
		readonly Lazy<PlatformConfigurationRegistry<SwipeView>> _platformConfigurationRegistry;

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='.ctor']/Docs" />
		public SwipeView()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<SwipeView>>(() => new PlatformConfigurationRegistry<SwipeView>(this));
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='ThresholdProperty']/Docs" />
		public static readonly BindableProperty ThresholdProperty =
			BindableProperty.Create(nameof(Threshold), typeof(double), typeof(SwipeView), default(double));

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='LeftItemsProperty']/Docs" />
		public static readonly BindableProperty LeftItemsProperty =
			BindableProperty.Create(nameof(LeftItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator,
				propertyChanged: OnSwipeItemsChanged);

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='RightItemsProperty']/Docs" />
		public static readonly BindableProperty RightItemsProperty =
			BindableProperty.Create(nameof(RightItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator,
				propertyChanged: OnSwipeItemsChanged);

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='TopItemsProperty']/Docs" />
		public static readonly BindableProperty TopItemsProperty =
			BindableProperty.Create(nameof(TopItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator,
				propertyChanged: OnSwipeItemsChanged);

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='BottomItemsProperty']/Docs" />
		public static readonly BindableProperty BottomItemsProperty =
			BindableProperty.Create(nameof(BottomItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.OneWay, null, defaultValueCreator: SwipeItemsDefaultValueCreator,
				propertyChanged: OnSwipeItemsChanged);

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='Threshold']/Docs" />
		public double Threshold
		{
			get { return (double)GetValue(ThresholdProperty); }
			set { SetValue(ThresholdProperty, value); }
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='LeftItems']/Docs" />
		public SwipeItems LeftItems
		{
			get { return (SwipeItems)GetValue(LeftItemsProperty); }
			set { SetValue(LeftItemsProperty, value); }
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='RightItems']/Docs" />
		public SwipeItems RightItems
		{
			get { return (SwipeItems)GetValue(RightItemsProperty); }
			set { SetValue(RightItemsProperty, value); }
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='TopItems']/Docs" />
		public SwipeItems TopItems
		{
			get { return (SwipeItems)GetValue(TopItemsProperty); }
			set { SetValue(TopItemsProperty, value); }
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='BottomItems']/Docs" />
		public SwipeItems BottomItems
		{
			get { return (SwipeItems)GetValue(BottomItemsProperty); }
			set { SetValue(BottomItemsProperty, value); }
		}

		bool ISwipeViewController.IsOpen { get; set; }

		static void OnSwipeItemsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((SwipeView)bindable).UpdateSwipeItemsParent((SwipeItems)newValue);
		}

		public event EventHandler<SwipeStartedEventArgs> SwipeStarted;
		public event EventHandler<SwipeChangingEventArgs> SwipeChanging;
		public event EventHandler<SwipeEndedEventArgs> SwipeEnded;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler<OpenRequestedEventArgs> OpenRequested;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler<CloseRequestedEventArgs> CloseRequested;

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='Open']/Docs" />
		public void Open(OpenSwipeItem openSwipeItem, bool animated = true)
		{
			OpenRequested?.Invoke(this, new OpenRequestedEventArgs(openSwipeItem, animated));
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='Close']/Docs" />
		public void Close(bool animated = true)
		{
			CloseRequested?.Invoke(this, new CloseRequestedEventArgs(animated));
		}

		void ISwipeViewController.SendSwipeStarted(SwipeStartedEventArgs args) => SwipeStarted?.Invoke(this, args);

		void ISwipeViewController.SendSwipeChanging(SwipeChangingEventArgs args) => SwipeChanging?.Invoke(this, args);

		void ISwipeViewController.SendSwipeEnded(SwipeEndedEventArgs args) => SwipeEnded?.Invoke(this, args);

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			object bc = BindingContext;

			if (LeftItems != null)
				SetInheritedBindingContext(LeftItems, bc);

			if (RightItems != null)
				SetInheritedBindingContext(RightItems, bc);

			if (TopItems != null)
				SetInheritedBindingContext(TopItems, bc);

			if (BottomItems != null)
				SetInheritedBindingContext(BottomItems, bc);
		}

		SwipeItems SwipeItemsDefaultValueCreator() => new SwipeItems();

		static object SwipeItemsDefaultValueCreator(BindableObject bindable)
		{
			return ((SwipeView)bindable).SwipeItemsDefaultValueCreator();
		}

		/// <include file="../../docs/Microsoft.Maui.Controls/SwipeView.xml" path="//Member[@MemberName='On']/Docs" />
		public IPlatformElementConfiguration<T, SwipeView> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}

		void UpdateSwipeItemsParent(SwipeItems swipeItems)
		{
			if (swipeItems.Parent == null)
				swipeItems.Parent = this;

			foreach (var item in swipeItems)
			{
				if (item is Element swipeItem && swipeItem.Parent == null)
					swipeItem.Parent = swipeItems;
			}
		}
	}
}