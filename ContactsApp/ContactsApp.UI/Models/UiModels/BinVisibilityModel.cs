using System;
using System.Windows;

using ContactsApp.UI.Models.Base;
using ContactsApp.UI.WpfExtesions;

namespace ContactsApp.UI.Models.UiModels
{
	public class BinVisibilityModel : NotificationModel
    {
		private Visibility _visibility;

		public Visibility Visibility
		{
			get => _visibility;
			set => this.SetVisibility(value);
		}

		public BinVisibilityModel(bool isVisible)
		{
			Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
		}

		protected void SetVisibility(Visibility value)
		{
			switch (value)
			{
				case Visibility.Visible:
				case Visibility.Hidden:
					this.SetValue(ref _visibility, value, nameof(Visibility));
					break;
				default:
					throw new NotImplementedException("colapsed visibility not supported");
			}
		}
	}
}