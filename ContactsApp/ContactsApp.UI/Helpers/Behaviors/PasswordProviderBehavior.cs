using System.Windows;
using System.Security;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ContactsApp.UI.Helpers.Behaviors
{
	public class PasswordProviderBehavior : Behavior<PasswordBox>
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.Register(
				"Password",
				typeof(SecureString),
				typeof(PasswordProviderBehavior),
				new FrameworkPropertyMetadata(OnPasswordChanged));

		public SecureString Password
		{
			get => this.GetValue(PasswordProperty) as SecureString;
			set => this.SetValue(PasswordProperty, value);
		}

		protected override void OnAttached()
		{
			AssociatedObject.PasswordChanged += this.AssociatedObjectOnPasswordChanged;
			base.OnAttached();
		}
		protected override void OnDetaching()
		{
			AssociatedObject.PasswordChanged -= this.AssociatedObjectOnPasswordChanged;
			base.OnDetaching();
		}

		private void AssociatedObjectOnPasswordChanged(object sender, RoutedEventArgs e)
		{
			Password = AssociatedObject.SecurePassword;
		}

		private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var box = (sender as PasswordProviderBehavior)?.AssociatedObject;

			if (box != null)
			{
				if (e.NewValue is SecureString stringZero && stringZero.Length == 0)
				{
					box.Password = string.Empty;
				}
			}
		}
	}
}