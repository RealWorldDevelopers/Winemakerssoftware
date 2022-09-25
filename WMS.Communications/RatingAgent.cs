
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WMS.Domain;

namespace WMS.Communications
{
    public class RatingAgent : IRatingAgent
    {
        private readonly HttpClient _httpClient;

        public RatingAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<Rating>> GetRatings()
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                List<Rating> model = new();

                var responseMessage = await _httpClient.GetAsync("api/rating").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Rating>>(jsonString, jsonSerializerSettings);
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
                return model ?? new List<Rating>();
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.GetAsync()");
                throw;
            }
        }


        public async Task<Rating> GetRating(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Rating model = new();

                var responseMessage = await _httpClient.GetAsync($"api/recipes/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Rating>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        public async Task<Rating> AddRating(Rating rating)
        {
            try
            {
                Rating model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(rating), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/ratings", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Rating>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

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
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<Rating> UpdateRating(Rating rating)
        {
            try
            {
                Rating? model = null;

                var httpContent = new StringContent(JsonConvert.SerializeObject(rating), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/ratings/{rating.Id}", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    model = JsonConvert.DeserializeObject<Rating>(jsonString, jsonSerializerSettings);                   
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }

                return model ?? rating;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

        public async Task<bool> DeleteRating(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                var responseMessage = await _httpClient.DeleteAsync($"api/rating/{id}");
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }

        }


    }

}
