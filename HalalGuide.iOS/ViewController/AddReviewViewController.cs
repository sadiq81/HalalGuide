// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using HalalGuide.iOS.ViewController;
using HalalGuide.ViewModels;
using SimpleDBPersistence.Service;
using XUbertestersSDK;
using HalalGuide.Util;
using HalalGuide.Domain.Enum;
using System.Security.Cryptography.X509Certificates;
using System.Resources;

namespace HalalGuide.iOS.ViewController
{
	public partial class AddReviewViewController : KeyboardSupportedUIViewController
	{
		private readonly AddReviewViewModel ViewModel = ServiceContainer.Resolve<AddReviewViewModel> ();
		private readonly int STAR_TAG_START = 101;
		private readonly int STAR_TAG_END = 105;

		private int Rating = 1;

		public AddReviewViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			XUbertesters.LogInfo ("ReviewController: ViewDidLoad-Start");
			base.ViewDidLoad ();
			SetupReviewTextField ();
			XUbertesters.LogInfo ("ReviewController: ViewDidLoad-End");
		}

		#region Setup

		private void SetupReviewTextField ()
		{
			Review.Layer.BorderColor = UIColor.Gray.CGColor;
			Review.Layer.BorderWidth = 2;
			Review.Layer.CornerRadius = 5;
			Review.ClipsToBounds = true;
		}

		#endregion

		#region Actions

		partial void Regreet (UIBarButtonItem sender)
		{
			XUbertesters.LogInfo ("ReviewController: Regreet-Start");
			DismissViewController (true, null);
			XUbertesters.LogInfo ("ReviewController: Regreet-End");

		}

		async partial  void Save (UIBarButtonItem sender)
		{
			XUbertesters.LogInfo ("ReviewController: Save-Start");

			ResignFirstResponder ();

			InvokeOnMainThread (ActivityIndicator.StartAnimating);

			CreateEntityResult result = await ViewModel.CreateNewReview (ViewModel.SelectedLocation, Rating, Review.Text);

			ActivityIndicator.StopAnimating ();

			if (result == CreateEntityResult.OK) {
				new UIAlertView ("Ok", "Din anmeldelse er sent til godkendelse", null, "Ok", null){ WeakDelegate = this }.Show ();
			} else {
				new UIAlertView ("Fejl", result.ToString (), null, "Ok", null).Show ();
			}

			XUbertesters.LogInfo ("ReviewController: Save-Start");
		}

		partial void StarPressed (UIButton sender)
		{
			Review.ResignFirstResponder ();

			int tag = sender.Tag;
			Rating = tag - 100;
			for (int i = STAR_TAG_START; i <= STAR_TAG_END; i++) {
				UIButton button = (UIButton)View.ViewWithTag (i);
				if (button.Tag <= tag) {
					button.SetImage (UIImage.FromBundle (Images.StarSelected), UIControlState.Normal);
				} else {
					button.SetImage (UIImage.FromBundle (Images.Star), UIControlState.Normal);
				}
			}
		}

		#endregion

		[Export ("alertView:clickedButtonAtIndex:")]
		public virtual void Clicked (UIAlertView alertview, int buttonIndex)
		{
			if (PresentingViewController.PresentingViewController != null) {
				PresentingViewController.PresentingViewController.DismissViewController (true, null);
			} else {
				PresentingViewController.DismissViewController (true, null);
			}
		}

		[Export ("positionForBar:")]
		public  UIBarPosition GetPositionForBar (IUIBarPositioning barPositioning)
		{
			return UIBarPosition.TopAttached;
		}
	}
}
