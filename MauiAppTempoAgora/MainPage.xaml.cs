using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); // Isso será gerado pelo compilador de XAML
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_cidade.Text))
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
                else
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Descrição: {t.description} \n" +
                                                $"Latitude: {t.lat} \n" +
                                                $"Longitude: {t.lon} \n" +
                                                $"Nascer do Sol: {t.sunrise} \n" +
                                                $"Por do Sol: {t.sunset} \n" +
                                                $"Temp Máx: {t.temp_max} \n" +
                                                $"Temp Min: {t.temp_min} \n" +
                                                $"Vento: {t.speed} m/s \n" +
                                                $"Visibilidade: {t.visibility} metros";

                        lbl_res.Text = dados_previsao;

                        string urlIcone = $"https://openweathermap.org/img/wn/{t.icon}@2x.png";
                        img_clima.Source = ImageSource.FromUri(new Uri(urlIcone));
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
