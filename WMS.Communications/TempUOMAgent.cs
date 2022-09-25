
using Newtonsoft.Json;
using WMS.Domain;

namespace WMS.Communications
{
    public class TempUOMAgent : ITempUOMAgent
    {
        private readonly HttpClient _httpClient;

        public TempUOMAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IUnitOfMeasure> AddUOM(IUnitOfMeasure uom)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUOM(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IUnitOfMeasure>> GetUOMs()
        {
            try
            {
                List<UnitOfMeasure> model = new();

                var responseMessage = await _httpClient.GetAsync("api/uom/temp").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<UnitOfMeasure>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
                }
                else
                {
                    // TODO handle error
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

        public async Task<IUnitOfMeasure> GetUOM(int id)
        {
            try
            {
                UnitOfMeasure model = new();

                var responseMessage = await _httpClient.GetAsync($"api/uom/{id}").ConfigureAwait(false);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<UnitOfMeasure>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
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

        public Task UpdateUOM(IUnitOfMeasure uom)
        {
            throw new NotImplementedException();
        }
    }

}