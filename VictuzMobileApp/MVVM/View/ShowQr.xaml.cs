namespace VictuzMobileApp.MVVM.View;

public partial class ShowQr : ContentPage
{
	public ShowQr()
    {
        InitializeComponent();
        GenerateQRCode("Victuz");
    }

    private void GenerateQRCode(string data)
    {
        string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(data)}";
        QR.Source = ImageSource.FromUri(new Uri(qrUrl));
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}