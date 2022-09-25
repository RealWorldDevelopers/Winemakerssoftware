
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WMS.Domain;

namespace WMS.Communications
{
    public class VarietyAgent : IVarietyAgent
    {
        private readonly HttpClient _httpClient;

        public VarietyAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Variety> AddVariety(Variety variety)
        {
            try
            {
                Variety model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(variety), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/Varieties", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Variety>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.AddAsync(TransactionsModel dtoList) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model;
            }
            catch (Exception e)
            {
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<bool> DeleteVariety(int id)
        {
            try
            {
                var responseMessage = await _httpClient.DeleteAsync($"api/Varieties/{id}").ConfigureAwait(false);
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                //await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }
        }

        public async Task<IEnumerable<Variety>> GetVarieties()
        {
            try
            {
                List<Variety> model = new();

                var responseMessage = await _httpClient.GetAsync("api/Varieties").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Variety>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
                }
                else
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.GetAsync() - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.GetAsync()");
                throw;
            }
        }

        public async Task<Variety> GetVariety(int id)
        {
            try
            {
                Variety model = new();

                var responseMessage = await _httpClient.GetAsync($"api/Varieties/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Variety>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.GetAsync() - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.GetAsync()");
                throw;
            }
        }

        public async Task<Variety> UpdateVariety(Variety variety)
        {
            try
            {
                Variety? model = null;

                var httpContent = new StringContent(JsonConvert.SerializeObject(variety), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/Varieties/{variety.Id}", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Variety>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }

                return model ?? variety;
            }
            catch (Exception e)
            {
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

    }

}
