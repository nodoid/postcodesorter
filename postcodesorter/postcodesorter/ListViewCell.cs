
using Xamarin.Forms;

namespace postcodesorter
{
    public class ListViewCell : ViewCell
    {
        public ListViewCell()
        {
            var lblPostcode = new Label
            {
                TextColor = Color.Purple
            };
            var lblDistance = new Label
            {
                TextColor = Color.Blue
            };
            lblPostcode.SetBinding(Label.TextProperty, new Binding("PostCode"));
            lblDistance.SetBinding(Label.TextProperty, new Binding("Distance"));
            View = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(12, 8),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label {Text = "Postcode : "}, lblPostcode
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label { Text = "Distance (miles): "}, lblDistance
                        }
                    }
                }
            };
        }
    }
}
