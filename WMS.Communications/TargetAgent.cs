
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WMS.Domain;

namespace WMS.Communications
{
    public class TargetAgent : ITargetAgent
    {
        private readonly HttpClient _httpClient;

        public TargetAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<Target>> GetTargets()
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                List<Target> model = new();

                var responseMessage = await _httpClient.GetAsync("api/Targets").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Target>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
                }
                else
                {
                    // TODO add logging
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

        public async Task<IEnumerable<Target>> GetTargets(int start, int length)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                IEnumerable<Target> model = null;

                var responseMessage = await _httpClient.GetAsync("api/Targets").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Target>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
                }
                else
                {
                    // TODO add logging
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

        public async Task<Target> GetTarget(int id)
        {
            try
            {
                Target model = new();

                var responseMessage = await _httpClient.GetAsync($"api/Targets/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Target>(jsonString, jsonSerializerSettings);
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

        public async Task<Target> AddTarget(Target target)
        {
            try
            {
                Target model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/Targets", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Target>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO add logging
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
                return model ?? target;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<Target> UpdateTarget(Target target)
        {
            try
            {
                Target? model = null;

                var httpContent = new StringContent(JsonConvert.SerializeObject(target), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/Targets/{target.Id}", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    model = JsonConvert.DeserializeObject<Target>(jsonString);
                }
                else
                {
                    // TODO add logging
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

                return model ?? target;
            }
            catch (Exception e)
            {
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

        public async Task<bool> DeleteTarget(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                var responseMessage = await _httpClient.DeleteAsync($"api/Targets/{id}");
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                //await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }

        }


    }

}
