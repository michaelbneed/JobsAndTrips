using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Trip.ViewModels
{
	public class SiteMapViewModel : BaseViewModel
	{
		public SiteMapViewModel()
		{
			Title = "Site Map";

			//OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
		}

		public ICommand OpenWebCommand { get; }
	}
}