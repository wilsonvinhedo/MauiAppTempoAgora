using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "fbc6222689ba8c02376b1fd0ce252b7d";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        DateTime baseTime = DateTime.UnixEpoch;
                        DateTime sunrise = baseTime.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = baseTime.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new Tempo
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            icon = (string)rascunho["weather"][0]["icon"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString("HH:mm"),
                            sunset = sunset.ToString("HH:mm"),
                        };
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new Exception("Cidade não encontrada. Verifique o nome digitado.");
                    }
                    else
                    {
                        throw new Exception("Erro ao buscar dados da previsão. Tente novamente mais tarde.");
                    }
                }
                catch (HttpRequestException)
                {
                    throw new Exception("Sem conexão com a internet. Verifique sua rede.");
                }
                catch (Exception ex)
                {
                    // Outros erros
                    throw new Exception($"Erro inesperado: {ex.Message}");
                }
            }

            return t;
        }
    }
}
