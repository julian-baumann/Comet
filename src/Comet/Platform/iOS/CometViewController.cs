﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Devices;
using UIKit;
using Color = System.Drawing.Color;

namespace Comet.iOS
{
	public class CometViewController : UIViewController
	{
		private CometView _containerView;
		private View _startingCurrentView;
		public IMauiContext MauiContext { get; set; }

		public View CurrentView
		{
			get => _containerView?.CurrentView as View ?? _startingCurrentView;
			set
			{
				if (_containerView != null)
					_containerView.CurrentView = value;
				else
					_startingCurrentView = value;

				Title = value?.GetTitle() ?? "";

			}
		}

		public object PlatformView => null;

		public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool wasPopped;
		public void WasPopped() => wasPopped = true;

		public override void LoadView()
		{
			base.View = _containerView = new CometView(MauiContext);
			_containerView.CurrentView = _startingCurrentView;
			Title = _startingCurrentView?.GetTitle() ?? "";
			_startingCurrentView = null;
		}
		internal CometView ContainerView
		{
			get => _containerView;
			set
			{
				_containerView?.RemoveFromSuperview();
				View = _containerView = value;
			}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			CurrentView?.ViewDidAppear();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ApplyStyle();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			CurrentView?.ViewDidDisappear();
			if (wasPopped)
			{
				CurrentView?.Dispose();
				CurrentView = null;
			}
		}

		public void ApplyStyle()
		{
			var barColor = CurrentView?.GetNavigationBackgroundColor()?.ToPlatform() ?? CUINavigationController.DefaultBarTintColor;

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.BarTintColor = barColor;
				
				if (View != null && OperatingSystem.IsIOSVersionAtLeast(13))
				{
					View.BackgroundColor = UIColor.SystemBackground;
				}

				NavigationController.NavigationBar.PrefersLargeTitles = true;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CurrentView?.Dispose();
				CurrentView = null;
			}
			base.Dispose(disposing);
		}

	}
}
