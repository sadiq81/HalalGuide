// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.ObjCRuntime;
using System.Linq;
using System.Globalization;
using System.Drawing;
using MonoTouch.CoreImage;
using MonoTouch.CoreText;
using S3Storage.Response;
using System.Dynamic;

namespace HalalGuide.iOS
{
	public partial class AutoCompleteUITextField : UITextField
	{
		//private readonly int kHTAutoCompleteButtonWidth = 30;

		public List<string> AutoCompleteValues { get; set; }

		private int AutocompleteType { get; set; }

		private bool AutocompleteDisabled{ get; set; }

		private bool IgnoreCase{ get; set; }

		private bool NeedsClearButtonSpace{ get; set; }

		private bool ShowAutocompleteButton{ get; set; }

		private UILabel AutocompleteLabel{ get; set; }

		private PointF AutocompleteTextOffset{ get; set; }

		private string AutocompleteString{ get; set; }

		private UIButton AutocompleteButton{ get; set; }

		public AutoCompleteUITextField (IntPtr handle) : base (handle)
		{

			SetupAutocompleteTextField ();
		}

		private void SetupAutocompleteTextField ()
		{

			AutocompleteLabel = new UILabel (RectangleF.Empty);
			AutocompleteLabel.Font = Font;
			AutocompleteLabel.BackgroundColor = UIColor.Clear;
			AutocompleteLabel.TextColor = UIColor.LightGray;

			AutocompleteLabel.LineBreakMode = UILineBreakMode.Clip;

			AutocompleteLabel.Hidden = true;
			
			AddSubview (AutocompleteLabel);
			BringSubviewToFront (AutocompleteLabel);

			/*
			AutocompleteButton = new UIButton (UIButtonType.Custom);
			AutocompleteButton.AddTarget (this, new Selector ("AutoCompleteTextButton"), UIControlEvent.TouchUpInside); 
			AutocompleteButton.SetImage (UIImage.FromBundle ("autocompleteButton"), UIControlState.Normal); 

			AddSubview (AutocompleteButton);
			BringSubviewToFront (AutocompleteButton);
			*/

			AutocompleteString = "";

			IgnoreCase = true;

			NSNotificationCenter.DefaultCenter.AddObserver (this, new Selector ("UITextFieldTextDidChangeNotification"), TextFieldTextDidChangeNotification, this);
             

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			//AutocompleteButton.Frame = FrameForAutocompleteButton ();
		}

		public override  UIFont Font {
			get{ return base.Font; }
			set { 
				AutocompleteLabel.Font = value;
				base.Font = value;
			}
		}

		public override bool BecomeFirstResponder ()
		{
			//BringSubviewToFront (AutocompleteButton);

			if (!AutocompleteDisabled) {
				if (ClearsOnBeginEditing) {
					AutocompleteLabel.Text = "";
				}

				AutocompleteLabel.Hidden = false;
			}

			return base.BecomeFirstResponder ();
		}

		public override bool ResignFirstResponder ()
		{
			if (!AutocompleteDisabled) {
				AutocompleteLabel.Hidden = true;

				if (CommitAutocompleteText ()) {

					NSNotificationCenter.DefaultCenter.PostNotificationName (TextFieldTextDidChangeNotification, this);
				}
			}
			return base.ResignFirstResponder ();
		}

		private RectangleF AutocompleteRectForBounds (RectangleF bounds)
		{
			RectangleF returnRect = RectangleF.Empty;
			RectangleF textRect = TextRect (Bounds);

			NSMutableParagraphStyle paragraphStyle = new NSMutableParagraphStyle ();
			paragraphStyle.LineBreakMode = UILineBreakMode.CharacterWrap;


			UIStringAttributes stringAttributes = new UIStringAttributes { Font = Font, ParagraphStyle = paragraphStyle };
			NSAttributedString text = new NSAttributedString (Text, stringAttributes);

			//CGRect prefixTextRect = [self.text boundingRectWithSize:textRect.size options:NSStringDrawingUsesLineFragmentOrigin|NSStringDrawingUsesFontLeading attributes:attributes context:nil];
			RectangleF prefixTextRect = text.GetBoundingRect (text.Size, NSStringDrawingOptions.UsesLineFragmentOrigin, null); //TODO Check if ok
			SizeF prefixTextSize = prefixTextRect.Size;

			UIStringAttributes AutoStringAttributes = new UIStringAttributes { Font = Font, ParagraphStyle = paragraphStyle };
			NSAttributedString autoText = new NSAttributedString (AutocompleteString, AutoStringAttributes);

			RectangleF autocompleteTextRect = autoText.GetBoundingRect (new SizeF (textRect.Size.Width - prefixTextSize.Width, textRect.Size.Height), NSStringDrawingOptions.UsesLineFragmentOrigin, null);
			SizeF autocompleteTextSize = autocompleteTextRect.Size;

			returnRect = new RectangleF (textRect.X + prefixTextSize.Width + AutocompleteTextOffset.X, textRect.Y + AutocompleteTextOffset.Y, autocompleteTextSize.Width, textRect.Size.Height);

			return returnRect;

		}

		[Export ("UITextFieldTextDidChangeNotification")]
		private void UITextFieldTextDidChangeNotification ()
		{
			RefreshAutocompleteText ();
		}

		private void UpdateAutocompleteLabel ()
		{
			AutocompleteLabel.Text = AutocompleteString;
			AutocompleteLabel.SizeToFit ();
			AutocompleteLabel.Frame = AutocompleteRectForBounds (Bounds);

			//DidChangeAutocompleteText ();
		}

		private  void RefreshAutocompleteText ()
		{
			if (!AutocompleteDisabled) {

				if (Text.Length > 0) {

					var list = AutoCompleteValues.Where (w => w.StartsWith (Text)).ToList ();
					if (list != null && list.Count > 0) {
						AutocompleteString = list [0].Remove (0, Text.Length);
					} else {
						AutocompleteString = "";
					}

				} else {
					AutocompleteString = "";
				}

				/*
				if (Text.Length == 0 || Text.Length == 1) {
					UpdateAutocompleteButton (true);
				}
				*/

				UpdateAutocompleteLabel ();
			}
		}


		private bool CommitAutocompleteText ()
		{

			string currentText = Text;

			if (AutocompleteString.Length >= 0 && AutocompleteDisabled == false) {
				Text += AutocompleteString;

				AutocompleteString = "";
				UpdateAutocompleteLabel ();

				//AutoCompleteTextFieldDidAutoComplete ();

			}
			return !currentText.Equals (Text);
		}


		private void ForceRefreshAutocompleteText ()
		{
			RefreshAutocompleteText ();
		}


		private void SetAutocompleteString (string autocompleteString)
		{
			this.AutocompleteString = autocompleteString;

			//UpdateAutocompleteButton (true);
		}

		/*
		private void SetShowAutocompleteButton (bool showAutocompleteButton)
		{
			this.ShowAutocompleteButton = showAutocompleteButton;

			UpdateAutocompleteButton (true);
		}
		*/

		/*
		private RectangleF FrameForAutocompleteButton ()
		{
			RectangleF autocompletionButtonRect;

			if (ClearButtonMode == UITextFieldViewMode.Never || Text.Length == 0) {
				autocompletionButtonRect = new RectangleF (Bounds.Size.Width - kHTAutoCompleteButtonWidth, (Bounds.Size.Height / 2) - (Bounds.Size.Height - 8) / 2, kHTAutoCompleteButtonWidth, Bounds.Size.Height - 8);
			} else {
				autocompletionButtonRect = new RectangleF (Bounds.Size.Width - 25 - kHTAutoCompleteButtonWidth, (Bounds.Size.Height / 2) - (Bounds.Size.Height - 8) / 2, kHTAutoCompleteButtonWidth, Bounds.Size.Height - 8);
			}
			return autocompletionButtonRect;
		}
		*/

		[Export ("AutoCompleteTextButton")]
		private void AutoCompleteText ()
		{
			if (!AutocompleteDisabled) {
				AutocompleteLabel.Hidden = false;

				CommitAutocompleteText ();

				// This is necessary because committing the autocomplete text changes the text field's text, but for some reason UITextField doesn't post the UITextFieldTextDidChangeNotification notification on its own
				NSNotificationCenter.DefaultCenter.PostNotificationName (TextFieldTextDidChangeNotification, this);
			}
		}
		/*
		private void UpdateAutocompleteButton (bool animated)
		{
			NSAction action = new NSAction (delegate {
				if (AutocompleteString.Length > 0 && ShowAutocompleteButton) {
					AutocompleteButton.Alpha = 1;
					AutocompleteButton.Frame = FrameForAutocompleteButton ();
				} else {
					AutocompleteButton.Alpha = 0;
				}

			});

			if (animated) {
				UIView.Animate (
					duration: 0.15f,
					animation: action);
			} else {
				action ();
			}
		}
		*/

	}
}
