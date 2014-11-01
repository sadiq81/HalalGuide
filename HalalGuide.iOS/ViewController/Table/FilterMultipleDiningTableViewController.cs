// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using HalalGuide.ViewModels;
using HalalGuide.Domain.Enums;
using System.Collections.Generic;
using HalalGuide.Util;
using HalalGuide.iOS.Tables.Cells;
using HalalGuide.Services;

namespace HalalGuide.iOS.ViewController.Table
{
	public partial class FilterMultipleDiningTableViewController : KeyboardSupportedTableViewController
	{
		public FilterMultipleDiningTableViewController (IntPtr handle) : base (handle)
		{
		}

		private MultipleDiningViewModel MultipleDiningViewModel = ServiceContainer.Resolve<MultipleDiningViewModel> ();

		private bool isExpanded { get; set; }

		public List<DiningCategory> VisibleCategories { get; set; }

		public List<DiningCategory> CategoriesChoosen { get; set; }

		public  override void ViewDidLoad ()
		{
			VisibleCategories = new List<DiningCategory> ();
			CategoriesChoosen = MultipleDiningViewModel.CategoryFilter;

			base.ViewDidLoad ();

			SetupUIValues ();
			SetupEventHandlers ();

		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}


		#region Setup

		private void SetupUIValues ()
		{
			DistanceSlider.Value = (float)MultipleDiningViewModel.DistanceFilter;

			if (MultipleDiningViewModel.DistanceFilter < Constants.MaxDistanceLimit) {
				DistanceLabel.Text = MultipleDiningViewModel.DistanceFilter + " km";
			} else {
				DistanceLabel.Text = Localization.GetLocalizedValue (Feedback.Unlimited);
			}

			PorkSwitch.On = MultipleDiningViewModel.PorkFilter;
			AlcoholSwitch.On = MultipleDiningViewModel.AlcoholFilter;
			HalalSwitch.On = MultipleDiningViewModel.HalalFilter;

			DistanceSlider.ValueChanged += (sender, e) => DistanceSliderValueChanged ((UISlider)sender);

			CategoriesChoosen = MultipleDiningViewModel.CategoryFilter;

			Count.Text = CategoriesChoosen.Count.ToString ();

			TableView.TableFooterView = new UIView ();
		}

		private void SetupEventHandlers ()
		{
			Choose.TouchUpInside += (button, e) => ShowHideCategories ((NSObject)button);
			Reset.TouchUpInside += (button, e) => ResetCategories ((NSObject)button);
		}

		#endregion

		#region Actions

		private void DistanceSliderValueChanged (UISlider sender)
		{
			sender.Value = (float)Math.Round (sender.Value, MidpointRounding.AwayFromZero);
			if (sender.Value < Constants.MaxDistanceLimit) {
				DistanceLabel.Text = sender.Value + " km";
			} else {
				DistanceLabel.Text = Localization.GetLocalizedValue (Feedback.Unlimited);
			}
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			MultipleDiningViewModel.DistanceFilter = DistanceSlider.Value;

			MultipleDiningViewModel.PorkFilter = PorkSwitch.On;
			MultipleDiningViewModel.AlcoholFilter = AlcoholSwitch.On;
			MultipleDiningViewModel.HalalFilter = HalalSwitch.On;

			MultipleDiningViewModel.CategoryFilter = CategoriesChoosen;


			//MultipleDiningViewModel.RefreshCache ();
			//MultipleDiningViewModel.OnFilteredLocations (EventArgs.Empty);
		}



		private void ShowHideCategories (NSObject sender)
		{
			((UIButton)sender).SetTitle (isExpanded ? Localization.GetLocalizedValue (Feedback.Choose) : Localization.GetLocalizedValue (Feedback.Close), UIControlState.Normal);

			if (isExpanded) {

				TableView.BeginUpdates ();
				for (int i = 0; i < Enum.GetValues (typeof(DiningCategory)).GetLength (0); i++) {
					VisibleCategories.RemoveAt (0);
					TableView.DeleteRows (new []{ NSIndexPath.FromRowSection (i, 2) }, UITableViewRowAnimation.Fade);
				}
				TableView.EndUpdates ();

			} else {
				TableView.BeginUpdates ();
				for (int i = 0; i < Enum.GetValues (typeof(DiningCategory)).GetLength (0); i++) {
					VisibleCategories.Add ((DiningCategory)Enum.GetValues (typeof(DiningCategory)).GetValue (i));
					TableView.InsertRows (new []{ NSIndexPath.FromRowSection (i, 2) }, UITableViewRowAnimation.Fade);
				}
				TableView.EndUpdates ();

			}

			isExpanded = !isExpanded;

		}

		private void ResetCategories (NSObject sender)
		{
			Count.Text = "0";
			CategoriesChoosen.Clear ();
			TableView.ReloadSections (new NSIndexSet (2), UITableViewRowAnimation.Fade);
		}

		#endregion

		public override int RowsInSection (UITableView tableView, int section)
		{
			if (section == 0 || section == 1) {
				return 1;
			} else {
				return VisibleCategories.Count;
			} 
		}

		public  override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 2) {

				var cell = tableView.DequeueReusableCell (CategoryCell.Identifier);

				if (cell == null) {
					cell = new CategoryCell (UITableViewCellStyle.Default, CategoryCell.Identifier);
				}

				cell.TextLabel.Text = "\t" + Localization.GetLocalizedValue (DiningCategoryExtensions.CategoryAtIndex (indexPath.Row).ToString ());

				bool selected = CategoriesChoosen.Contains (DiningCategoryExtensions.CategoryAtIndex (indexPath.Row));
				cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

				return cell;
			} else {
				return base.GetCell (tableView, indexPath);
			}
		}

		public override bool ShouldHighlightRow (UITableView tableView, NSIndexPath rowIndexPath)
		{
			if (rowIndexPath.Section == 2) {
				return true;
			} else {
				return false;
			}
		}


		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath);
			tableView.DeselectRow (indexPath, false);

			if (indexPath.Section != 2) {
				return;
			}

			DiningCategory cat = DiningCategoryExtensions.CategoryAtIndex (indexPath.Row);

			if (CategoriesChoosen.Contains (cat)) {
				CategoriesChoosen.Remove (DiningCategoryExtensions.CategoryAtIndex (indexPath.Row));
				cell.Accessory = UITableViewCellAccessory.None;
			} else {
				CategoriesChoosen.Add (DiningCategoryExtensions.CategoryAtIndex (indexPath.Row));
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}

			Count.Text = CategoriesChoosen.Count.ToString ();
		}

		public override float GetHeightForRow (MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (indexPath.Section == 2) {
				return 44;
			} else {
				return base.GetHeightForRow (tableView, indexPath);
			}
		}


		public override int IndentationLevel (MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (indexPath.Section == 2) {
				return 0;
			} else {
				return base.IndentationLevel (tableView, indexPath);
			}
		}

	}
}
