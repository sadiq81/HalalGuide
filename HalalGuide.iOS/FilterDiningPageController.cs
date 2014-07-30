// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using HalalGuide.Domain.Enum;
using HalalGuide.Util;
using MonoTouch.CoreImage;
using System.Drawing;
using SimpleDBPersistence.SimpleDB.Model.Parameters;
using HalalGuide.ViewModels;
using SimpleDBPersistence.Service;

namespace HalalGuide.iOS
{
	public partial class FilterDiningPageController : UIViewController
	{
		private bool isExpanded { get; set; }

		private DiningViewModel ViewModel = ServiceContainer.Resolve<DiningViewModel> ();

		private static string cellIdentifier = "CategoryTableCell";

		List<DiningCategory> CategoriesShown = new List<DiningCategory> ();
		List<DiningCategory> CategoriesHidden = new List<DiningCategory> ();

		public FilterDiningPageController (IntPtr handle) : base (handle)
		{

		}

		partial void SliderValueChanged (UISlider sender)
		{
			sender.Value = (float)Math.Round (sender.Value, MidpointRounding.AwayFromZero);
			ViewModel.DistanceFilter = (int)sender.Value;
			SliderValueLabel.Text = sender.Value + " km";
		}

		partial void PorkValueChanged (UISwitch sender)
		{
			ViewModel.PorkFilter = sender.On;
		}

		partial void AlcoholValueChanged (UISwitch sender)
		{
			ViewModel.AlcoholFilter = sender.On;
		}

		partial void HalalValueChanged (UISwitch sender)
		{
			ViewModel.HalalFilter = sender.On;
		}

		partial void ResetCategory (UIButton sender)
		{
			ViewModel.CategoryFilter.Clear ();
			NumberOfCategoriesLabel.Text = "0";
			foreach (UIView view in CategoryTableView.Subviews) {
				if (view is UITableViewCell) {
					((UITableViewCell)view).Accessory = UITableViewCellAccessory.None;
				}
			}
		}

		[Export ("positionForBar:")]
		public  UIBarPosition GetPositionForBar (IUIBarPositioning barPositioning)
		{
			return UIBarPosition.TopAttached;
		}

		public  override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CategoryTableView.WeakDataSource = this;
			CategoryTableView.WeakDelegate = this;

			CategoryTableView.TableFooterView = new UIView ();

			Slider.Value = (float)ViewModel.DistanceFilter;
			SliderValueLabel.Text = ViewModel.DistanceFilter.ToString ();

			PorkSwitch.On = ViewModel.PorkFilter;
			AlcoholSwitch.On = ViewModel.AlcoholFilter;
			HalalSwitch.On = ViewModel.HalalFilter;

			NumberOfCategoriesLabel.Text = ViewModel.CategoryFilter.Count.ToString ();

		}

		partial void ChooseCategory (UIButton sender)
		{
			sender.SetTitle (isExpanded ? "Vælg" : "Luk", UIControlState.Normal);

			if (isExpanded) {

				int count = CategoriesShown.Count;

				for (int i = 0; i < count; i++) {
					CategoryTableView.BeginUpdates ();
					// insert the 'ADD NEW' row at the end of table display
					CategoryTableView.DeleteRows (new NSIndexPath[] { NSIndexPath.FromRowSection (CategoryTableView.NumberOfRowsInSection (0) - 1, 0) }, UITableViewRowAnimation.Fade);
					// create a new item and add it to our underlying data (it is not intended to be permanent)

					DiningCategory cat = CategoriesShown [CategoryTableView.NumberOfRowsInSection (0) - 1];
					CategoriesHidden.Add (cat);
					CategoriesShown.Remove (cat);
					CategoryTableView.EndUpdates (); // applies the changes
				}

			} else {

				foreach (DiningCategory category in DiningCategory.Categories) {
					CategoryTableView.BeginUpdates ();
					// insert the 'ADD NEW' row at the end of table display
					CategoryTableView.InsertRows (new NSIndexPath[] { NSIndexPath.FromRowSection (CategoryTableView.NumberOfRowsInSection (0), 0) }, UITableViewRowAnimation.Fade);
					// create a new item and add it to our underlying data (it is not intended to be permanent)
					CategoriesHidden.Remove (category);
					CategoriesShown.Add (category);
					CategoryTableView.EndUpdates (); // applies the changes
				}
			}

			UIView.Animate (
				duration : 0.2,
				animation: () => {
					CategoryTableView.Frame = new RectangleF (CategoryTableView.Frame.X, CategoryTableView.Frame.Y, 320, isExpanded ? 0 : this.View.Frame.Height - CategoryTableView.Frame.Y);
				}
			);
			isExpanded = !isExpanded;

		}

		[Export ("tableView:numberOfRowsInSection:")]
		public  int RowsInSection (UITableView tableview, int section)
		{
			return CategoriesShown.Count;
		}

		[Export ("tableView:cellForRowAtIndexPath:")]
		public  UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{

			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);

			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			}

			cell.TextLabel.Text = "\t" + CategoriesShown [indexPath.Row].Title;

			bool selected = ViewModel.CategoryFilter.Contains (CategoriesShown [indexPath.Row]);
			cell.Accessory = selected ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

			return cell;
		}

		[Export ("tableView:didSelectRowAtIndexPath:")]
		public  void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath);
			DiningCategory cat = CategoriesShown [indexPath.Row];

			if (ViewModel.CategoryFilter.Contains (cat)) {
				ViewModel.CategoryFilter.Remove (CategoriesShown [indexPath.Row]);
				cell.Accessory = UITableViewCellAccessory.None;
			} else {
				ViewModel.CategoryFilter.Add (CategoriesShown [indexPath.Row]);
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}

			NumberOfCategoriesLabel.Text = ViewModel.CategoryFilter.Count.ToString ();
		}
	}
}


