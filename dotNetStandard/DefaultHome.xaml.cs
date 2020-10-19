using Atomus.Ads.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atomus.Page.Home
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DefaultHome : ContentPage, IAction
    {
        private AtomusPageEventHandler beforeActionEventHandler;
        private AtomusPageEventHandler afterActionEventHandler;

        #region Init
        public DefaultHome()
        {
            InitializeComponent();

            this.SetView();
        }

        private void SetView()
        {
            switch (this.GetAttribute("PageType"))
            {
                case "WebView":
                    this.SetWebView();
                    break;
            }
        }
        #endregion

        #region IO
        object IAction.ControlAction(ICore sender, AtomusPageArgs e)
        {
            try
            {
                this.beforeActionEventHandler?.Invoke(this, e);

                switch (e.Action)
                {
                    case "WebVIew.Home":
                        if (((StackLayout)this.Content).Children.Count > 0 && ((StackLayout)this.Content).Children[0] is WebView)
                            ((WebView)((StackLayout)this.Content).Children[0]).Source = this.GetAttribute("WebView.Source");
                        return true;

                    case "WebVIew.Back":
                        if (((StackLayout)this.Content).Children.Count > 0 && ((StackLayout)this.Content).Children[0] is WebView && ((WebView)((StackLayout)this.Content).Children[0]).CanGoBack)
                            ((WebView)((StackLayout)this.Content).Children[0]).GoBack();
                        return true;

                    case "WebVIew.Forward":
                        if (((StackLayout)this.Content).Children.Count > 0 && ((StackLayout)this.Content).Children[0] is WebView && ((WebView)((StackLayout)this.Content).Children[0]).CanGoForward)
                            ((WebView)((StackLayout)this.Content).Children[0]).GoForward();
                        return true;

                    default:
                        throw new AtomusException("'{0}'은 처리할 수 없는 Action 입니다.".Translate(e.Action));
                }

                //return true;
            }
            finally
            {
                this.afterActionEventHandler?.Invoke(this, e);
            }
        }
        #endregion

        #region Event
        event AtomusPageEventHandler IAction.BeforeActionEventHandler
        {
            add
            {
                this.beforeActionEventHandler += value;
            }
            remove
            {
                this.beforeActionEventHandler -= value;
            }
        }

        event AtomusPageEventHandler IAction.AfterActionEventHandler
        {
            add
            {
                this.afterActionEventHandler += value;
            }
            remove
            {
                this.afterActionEventHandler -= value;
            }
        }
        #endregion

        #region Etc

        private void SetWebView()
        {
            StackLayout stackLayout;
            WebView webView;
            AdsBannerControl adsBannerControl;

            stackLayout = new StackLayout();
            stackLayout.Margin = new Thickness(0);

            webView = new WebView();
            webView.Source = this.GetAttribute("WebView.Source");
            webView.VerticalOptions = LayoutOptions.FillAndExpand;
            webView.HorizontalOptions = LayoutOptions.FillAndExpand;
            stackLayout.Children.Add(webView);

            adsBannerControl = new AdsBannerControl();
            //adsBannerControl.AdUnitId = "ca-app-pub-1533168241290971/3724025077";
            stackLayout.Children.Add(adsBannerControl);

            this.Content = stackLayout;
        }
        #endregion
    }
}