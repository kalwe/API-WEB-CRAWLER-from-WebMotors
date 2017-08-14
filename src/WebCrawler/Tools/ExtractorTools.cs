using System;
using System.Net;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VeiculosWebApi.Models;

namespace WebCrawler.Tools
{
    public class ExtractorTools : WebTools
    {
        const string urlAbsolute = "http://www.webmotors.com.br/";
        // const string urlRelative = "/marcasativas/";

        public string UrlRelative { get; set;}

        public string Categoria { get; set; }

        public List<Marca> Marcas { get; set; }
        public List<Modelo> Modelos { get; set; }

        // Extrai as marcas de veiculos
        public async Task ExtractMarcas()
        {
            // Preper url and make http request 
            string uriBase = urlAbsolute + Categoria + "/marcasativas/";
            var uri = new Uri(uriBase);

            // TextInfo ti = CultureInfo.CurrentCulture.TextInfo; 
            // Retorna a resposta http da uri de forma assincrona
            await GetUrlResponse(uri);

            if (HttpResponseStatusOK(WebResponse))
            {
                // Extrai do stream response ultilizando regex com filtros patterns
                using (StreamReader stream = GetHttpResponseStream(WebResponse))
                {
                    string strResponse = await stream.ReadToEndAsync();

                    // definicao dos patterns
                    string patternCommon = @"""Common""\:\[([\{""N"":""(\w+?|\W+?)""\,\}]+)\]";
                    string patternPrincipal = @"\[[{""N"":""(\w+?|\W+?)]+?\],""";
                    string patternGetField = @":"".+?""";
                    string patternClean = @"(:|""|[|]|{|})";

                    Regex filtroCommon = new Regex(patternCommon);
                    Regex filtroPrincipal = new Regex(patternPrincipal);
                    Regex filtroField = new Regex(patternGetField);

                    // filtra o strResponse com o filtro 'patternCommon' e 'patternPrincipal'
                    var commonResult = filtroCommon.Match(strResponse);
                    var principalResult = filtroPrincipal.Match(strResponse);

                    // seta com os resultados encontrados no filtro 'patternGetField'
                    MatchCollection matches = filtroField.Matches(commonResult.ToString());
                    MatchCollection matchesPrincipal = filtroField.Matches(principalResult.ToString());

                    Marcas = new List<Marca>();
                    foreach (var item in matches)
                    {
                        var name = Regex.Replace(item.ToString(), patternClean, "");

                        var marca = new Marca()
                        {
                            Id = null,
                            CreateTimeStamp = $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}",
                            Active = true,
                            ModificationTimeStamp = null,
                            Name = name,
                            Principal = false,
                            Categoria = $"categorias/{Categoria}"
                        };

                        foreach (var principal in matchesPrincipal)
                        {
                            var _name = Regex.Replace(principal.ToString(), patternClean, "");
                            if (name == _name)
                                marca.Principal = true;
                        }

                        Marcas.Add(marca);
                    }
                }
            }
        }

        // Extrai todos os modelos de uma marca
        public async Task ExtractModelos(string marca)
        {
            string uriBase = urlAbsolute + Categoria + "/modelosativos?marca=" + marca;

            var uri = new Uri(uriBase);

            await GetUrlResponse(uri);

            using (WebResponse)
            {
                if (HttpResponseStatusOK(WebResponse))
                {
                    using(var stream = GetHttpResponseStream(WebResponse))
                    {
                        var strResponse = await stream.ReadToEndAsync();

                        var patternModelo = @":""(\w+?|\W+?|0-9|\s)+?""";
                        var patternClean = @"(:|"")";

                        var filterModelo = new Regex(patternModelo);
                        // var filterClean = new Regex(patternClean);

                        var matches = filterModelo.Matches(strResponse);

                        Modelos = new List<Modelo>();
                        foreach(var item in matches)
                        {
                            // Remove special characters
                            var name = Regex.Replace(item.ToString(), patternClean, "");

                            var modelo = new Modelo()
                            {
                                Id = null,
                                Active = true,
                                CreateTimeStamp = $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}",
                                ModificationTimeStamp = null,
                                Name = name,
                                Marca = null,
                                Categoria = $"categorias/{Categoria}"
                            };
                            Modelos.Add(modelo);
                        }
                    }
                }
            }

        }
    }
}
