// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using HalalGuide.iOS.ViewController;
using HalalGuide.ViewModels;
using SimpleDBPersistence.Service;
using XUbertestersSDK;
using HalalGuide.Domain.Enum;
using HalalGuide.Util;
using Xamarin.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HalalGuide.iOS
{
	public partial class AddNewDiningViewController : KeyboardSupportedTableViewController
	{
		public AddNewDiningViewController (IntPtr handle) : base (handle)
		{
		}

		private readonly AddDiningViewModel ViewModel = ServiceContainer.Resolve<AddDiningViewModel> ();

		private CategorySelectionView CategorySelectionView { get; set; }

		public async override void ViewDidLoad ()
		{
			XUbertesters.LogInfo ("AddNewDiningController: ViewDidLoad-Start");

			SetupEventListeners ();

			await SetupUIElements ();

			XUbertesters.LogInfo ("AddNewDiningController: ViewDidLoad-End");

		}


		#region Setup

		private void SetupEventListeners ()
		{
			Road.EditingDidEnd += async (sender, e) => {
				RoadNumber.AutoCompleteValues = ViewModel.StreetNumbers (Road.Text);
				PostalCode.Text = ViewModel.PostalCode (Road.Text) ?? PostalCode.Text;
				City.Text = await ViewModel.GetCityNameFromPostalCode (PostalCode.Text) ?? City.Text;
			};
			PostalCode.EditingDidEnd += async (sender, e) => {
				string cityName = await ViewModel.GetCityNameFromPostalCode (PostalCode.Text);
				City.Text = cityName ?? City.Text;
			};
		}

		private async Task SetupUIElements ()
		{
			base.ViewDidLoad ();

			VisibleCategories = new List<DiningCategory> ();
			CategoriesChoosen = new List<DiningCategory> ();

			await ViewModel.LoadAddressNearPosition ();
			Road.AutoCompleteValues = ViewModel.StreetNames ();

		}

		#endregion

		#region Actions

		partial void Regreet (UIBarButtonItem sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: Regreet-Start");
			DismissViewController (true, null);
			XUbertesters.LogInfo ("AddNewDiningController: Regreet-End");

		}

		async partial void Save (UIBarButtonItem sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: Save-Start");

			if (String.IsNullOrEmpty (Name.Text)) {
				new UIAlertView ("Fejl", "Navn skal udfyldes", null, "Ok").Show ();
				return;
			}

			if (String.IsNullOrEmpty (Road.Text)) {
				new UIAlertView ("Fejl", "Vej skal udfyldes", null, "Ok").Show ();
				return;
			}

			if (String.IsNullOrEmpty (RoadNumber.Text)) {
				new UIAlertView ("Fejl", "Vejnummer skal udfyldes", null, "Ok").Show ();
				return;
			}

			if (String.IsNullOrEmpty (PostalCode.Text)) {
				new UIAlertView ("Fejl", "Postnummer skal udfyldes", null, "Ok").Show ();
				return;
			}

			if (String.IsNullOrEmpty (City.Text)) {
				new UIAlertView ("Fejl", "By skal udfyldes", null, "Ok").Show ();
				return;
			}

			InvokeOnMainThread (ActivityIndicator.StartAnimating);

			CreateEntityResult result = await ViewModel.CreateNewLocation (
				                            Name.Text, 
				                            Road.Text, 
				                            RoadNumber.Text, 
				                            PostalCode.Text, 
				                            City.Text, 
				                            Telephone.Text, 
				                            HomePage.Text, 
				                            PorkSwitch.On, 
				                            AlcoholSwitch.On, 
				                            HalalSwitch.On,
				                            CategorySelectionView.CategoriesChoosen);

			ActivityIndicator.StopAnimating ();

			if (result == CreateEntityResult.OK) {
				new UIAlertView ("Succes", "Dit forslag er sent til godkendelse", null, "Ok", new string[]{ "Tilfø anmeldelse" }){ WeakDelegate = this }.Show ();
			} else {
				new UIAlertView ("Fejl", result.ToString (), null, "Ok", null).Show ();
			}

			XUbertesters.LogInfo ("AddNewDiningController: Save-End");

		}

		[Export ("alertView:clickedButtonAtIndex:")]
		public virtual void Clicked (UIAlertView alertview, int buttonIndex)
		{
			switch (buttonIndex) {
			case 0:
				{
					DismissViewController (true, null);
					break;
				}
			case 1:
				{
					PerformSegue (Segue.AddReviewViewControllerSegue, this);
					break;
				}
			}
		}

		partial void AlcoholValueChanged (UISwitch sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: AlcoholValueChanged-Start");
			AlcoholImage.Image = UIImage.FromBundle (Images.Alcohol + sender.On);
			XUbertesters.LogInfo ("AddNewDiningController: AlcoholValueChanged-End");
		}

		partial void HalalValueChanged (UISwitch sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: HalalValueChanged-Start");
			HalalImage.Image = UIImage.FromBundle (Images.NonHalal + sender.On);
			XUbertesters.LogInfo ("AddNewDiningController: HalalValueChanged-End");
		}

		partial void PorkValueChanged (UISwitch sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: PorkValueChanged-Start");
			PorkImage.Image = UIImage.FromBundle (Images.Pig + sender.On);
			XUbertesters.LogInfo ("AddNewDiningController: PorkValueChanged-End");
		}

		partial void PickImage (UIButton sender)
		{
			XUbertesters.LogInfo ("AddNewDiningController: PickImage-Start");
			if (ViewModel.IsCameraAvailable ()) {

				UIActionSheet actionSheet = new UIActionSheet ("Tilfø billede", null, "Fortryd", null, "Tag med kamera", "Væg fra kamerarulle");
				actionSheet.Clicked += async delegate(object a, UIButtonEventArgs b) {
					switch (b.ButtonIndex) {
					case 0:
						{
							MediaFile file = await ViewModel.TakePicture ("../Library/Caches", "test.jpg");
							if (file != null)
								InvokeOnMainThread (() => {
									DiningImage.Image = UIImage.LoadFromData (NSData.FromFile (file.Path));
									PickImageButton.SetTitle (null, UIControlState.Normal);
								});
							break;
						}
					case 1:
						{
							MediaFile file = await ViewModel.GetPictureFromDevice ();
							if (file != null)
								InvokeOnMainThread (() => {
									DiningImage.Image = UIImage.LoadFromData (NSData.FromFile (file.Path));
									PickImageButton.SetTitle (null, UIControlState.Normal);
								});
							break;
						}
					case 2:
						{
							break;
						}

					}
				};
				actionSheet.ShowInView (View);

			} else {
				XUbertesters.LogWarn ("AddNewDiningController: noCameraFound");
				UIAlertView noCameraFound = new UIAlertView ("Fejl", "Intet kamera tilgægeligt", null, "Luk");
				noCameraFound.Show ();
			}
			XUbertesters.LogInfo ("AddNewDiningController: PickImage-End");
		}

		#endregion

		#region TableView

		private bool isExpanded { get; set; }

		private readonly string CategoryCellIdentifier = "CategoryCell";

		public List<DiningCategory> VisibleCategories { get; set; }

		public List<DiningCategory> CategoriesChoosen { get; set; }

		partial void Choose (UIButton sender)
		{

			sender.SetTitle (isExpanded ? "Vælg" : "Luk", UIControlState.Normal);

			if (isExpanded) {

				TableView.BeginUpdates ();
				for (int i = 0; i < DiningCategory.Categories.Count; i++) {
					VisibleCategories.RemoveAt (0);
					TableView.DeleteRows (new []{ NSIndexPath.FromRowSection (i, 2) }, UITableViewRowAnimation.Fade);
				}
				TableView.EndUpdates ();

			} else {
				TableView.BeginUpdates ();
				for (int i = 0; i < DiningCategory.Categories.Count; i++) {
					VisibleCategories.Add (DiningCategory.Categories [i]);
					TableView.InsertRows (new []{ NSIndexPath.FromRowSection (i, 2) }, UITableViewRowAnimation.Fade);
				}
				TableView.EndUpdates ();

			}

			isExpanded = !isExpanded;

		}

		partial void Reset (UIButton sender)
		{
			CountLabel.Text = "0";
			CategoriesChoosen.Clear ();
			TableView.ReloadSections (new NSIndexSet (2), UITableViewRowAnimation.Fade);
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			if (section == 0 || section == 1) {
				return base.RowsInSection (tableview, section);
			} else if (section == 2) {
				return VisibleCategories.Count;
			} else {
				return base.RowsInSection (tableview, section);
			}
		}

		public  override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0 || indexPath.Section == 1) {
				return base.GetCell (tableView, indexPath);
			} else if (indexPath.Section == 2) {

				CategoryCell cell = (CategoryCell)tableView.DequeueReusableCell (CategoryCellIdentifier);

				if (cell == null) {
					cell = new CategoryCell (UITableViewCellStyle.Default, CategoryCellIdentifier);
				}

				cell.TextLabel.Text = "\t" + DiningCategory.Categories [indexPath.Row].Title;

				bool selected = CategoriesChoosen.Contains (DiningCategory.Categories [indexPath.Row]);
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

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0 || indexPath.Section == 1) {
				return base.GetHeightForRow (tableView, indexPath);
			} else if (indexPath.Section == 2) {
				return 44;
			} else {
				return base.GetHeightForRow (tableView, indexPath);
			}
		}

		public override int IndentationLevel (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0 || indexPath.Section == 1) {
				return base.IndentationLevel (tableView, indexPath);
			} else if (indexPath.Section == 2) {
				return 1;
			} else {
				return base.IndentationLevel (tableView, indexPath);
			}
		}



		public override  void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath);
			tableView.DeselectRow (indexPath, false);

			if (indexPath.Section != 2) {
				return;
			}

			DiningCategory cat = DiningCategory.Categories [indexPath.Row];

			if (CategoriesChoosen.Contains (cat)) {
				CategoriesChoosen.Remove (DiningCategory.Categories [indexPath.Row]);
				cell.Accessory = UITableViewCellAccessory.None;
			} else {
				CategoriesChoosen.Add (DiningCategory.Categories [indexPath.Row]);
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			}

			CountLabel.Text = CategoriesChoosen.Count.ToString ();
		}

		#endregion
	}

}
